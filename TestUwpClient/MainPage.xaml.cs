using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.AspNet.SignalR.Client;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestUwpClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IHubProxy _hub;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var sr = new HubConnection("http://localhost:31264")
            {
                TraceLevel = TraceLevels.All,
                TraceWriter = new ActionWriter(Log)
            };
            _hub = sr.CreateHubProxy("TestHub");
            _hub.On("heartbeat", () => Log("heartbeat recv"));
            sr.Start();
        }

        private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {
            _hub.Invoke<string>("Test", Guid.NewGuid().ToString());
        }

        private void Log(string msg)
        {
            Dispatch(() =>
            {
                Debug.WriteLine(msg);
                LogItems.Items?.Insert(0, msg);
            });
        }

        private async void Dispatch(Action a)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, a.Invoke);
        }
    }
}
