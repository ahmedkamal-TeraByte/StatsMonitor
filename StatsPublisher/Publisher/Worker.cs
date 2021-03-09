using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Publisher
{
    public class Worker : BackgroundService
    {
        private Timer _timer;
        private object lockObject=new object();
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;
        private const string URL = "https://localhost:44354/usage/publish";

        public Worker()
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            _clientFactory = serviceProvider.GetService<IHttpClientFactory>();
            _client = _clientFactory.CreateClient();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer= new Timer(4000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (lockObject)
            {
                var stats = GetStats();
                Console.WriteLine( stats.ToString());
                JObject recordObject = new JObject();
                recordObject.Add("CpuUsage", stats.CpuUsage);
                recordObject.Add("MemoryUsage", stats.MemoryUsage);
                recordObject.Add("Time", stats.Time);

                var request = new HttpRequestMessage(HttpMethod.Post, URL);

                try
                {
                    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                catch (Exception exc)
                {
                    return;
                }

                JObject obj = new JObject();
                obj.Add("stats", recordObject);
                string jsonString = recordObject.ToString();
                request.Content = new StringContent(jsonString,
                                      Encoding.UTF8,
                                      "application/json");

                try
                {
                    //send the request
                    var response = Task.Run(async () => await _client.SendAsync(request)).Result;

                    //if the request was successfull
                    //extract the response 
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //extract the response of request
                        var responseString = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                        var result = JObject.Parse(responseString);
                        //parse the response and return for handling
                        JArray resultArr = JArray.Parse(result["result"].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private Stats GetStats()
        {
            var counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var stats = new Stats();
            var cpuUsage = (int)counter.NextValue();
            System.Threading.Thread.Sleep(1000);
            cpuUsage = (int)counter.NextValue();
            stats.CpuUsage = cpuUsage;

            var memCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            var memUsage = (int)memCounter.NextValue();
            memUsage = (int)memCounter.NextValue();
            stats.MemoryUsage = memUsage;

            stats.Time = DateTime.UtcNow;
            return stats;
        }
    }

    public class Stats
    {
        public int MemoryUsage { get; set; }
        public int CpuUsage { get; set; }
        public DateTime Time { get; set; }

        public override string ToString()
        {
            return $"Memory= {MemoryUsage}, CPU = {CpuUsage}";
        }
    }
}
