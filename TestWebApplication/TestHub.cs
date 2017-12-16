using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace TestWebApplication
{
    public class TestHub : Hub
    {
        public string Test(string s)
        {
            return s.Reverse().ToString();
        }
    }
}
