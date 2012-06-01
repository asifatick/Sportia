using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace GrandAStudio.Sportia
{
    public class XNAAsyncDispatcher : IApplicationService
    {
        private DispatcherTimer frameworkDispatcherTimer;

        public XNAAsyncDispatcher(TimeSpan dispatchInterval)
        {
            this.frameworkDispatcherTimer = new DispatcherTimer();
            this.frameworkDispatcherTimer.Tick
                 += new EventHandler(frameworkDispatcherTimer_Tick);
            this.frameworkDispatcherTimer.Interval = dispatchInterval;
        }

        void IApplicationService.StartService(ApplicationServiceContext context)
        { this.frameworkDispatcherTimer.Start(); }
        void IApplicationService.StopService()
        { this.frameworkDispatcherTimer.Stop(); }
        void frameworkDispatcherTimer_Tick(object sender, EventArgs e)
        { FrameworkDispatcher.Update(); }
    }
}
