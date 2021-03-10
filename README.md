# StatsMonitor

<b>Stats_Monitor</b> is supposed to be helping in monitoring of stats of remote machine such as Cloud VMs.
This solution contains two projects. 
  - StatsPublisher
  - StatsCollector
 
<b>StatsPublisher</b> is an agent that will run on host machine as a service. It will collect Memory and CPU usage of that machine and send it to StatsCollecter by HTTP Post request. It can be modified to send other stats that are available in "Perfomance Monitor". Request interval is configurable.

<b>StatsCollector</b> runs on another machine which recieves Post Requests from StatsPublisher and stores them in database. 
  - It can be modified to pubish these stats on Prometheus or other monitoring tool.

I stored these stats in SQL server and then used <b>Grafaana</b> to collect from database and show as Graph. 
