using System;
using Xamarin.Forms;

namespace TsinghuaNet.CrossPlatform.Models
{
    public class DeviceTimer
    {
        private volatile bool running;
        public TimeSpan Interval { get; set; }

        public DeviceTimer() : this(TimeSpan.FromMilliseconds(1)) { }
        public DeviceTimer(TimeSpan interval) : this(interval, null) { }
        public DeviceTimer(TimeSpan interval, EventHandler action)
        {
            Interval = interval;
            if (action != null) Tick += action;
        }

        public event EventHandler Tick;
        protected void OnTick(EventArgs e) => Tick?.Invoke(this, e);

        private bool TickInternal()
        {
            if (running)
                OnTick(EventArgs.Empty);
            return running;
        }

        public void Start()
        {
            if (!running)
            {
                running = true;
                Device.StartTimer(Interval, TickInternal);
            }
        }

        public void Stop()
        {
            running = false;
        }
    }
}
