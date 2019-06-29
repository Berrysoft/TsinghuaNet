namespace TsinghuaNet.Eto.ViewModels
{
    public class NetSettings : TsinghuaNet.Models.NetSettings
    {
        private bool useTimer;
        public bool UseTimer
        {
            get
            {
                return useTimer;
            }
            set
            {
                SetProperty(ref useTimer, value);
            }
        }
    }
}
