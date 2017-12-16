using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace TestWebApplication
{
    public class HeartbeatTest
    {
        private readonly IHubContext _proxy;
        private readonly Timer _timer;

        public HeartbeatTest()
        {
            _proxy = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
            _timer = new Timer(Tick, null, 5000, 1000);
        }

        private void Tick(object state)
        {
            _proxy.Clients.All.heartbeat();
        }
    }
}
