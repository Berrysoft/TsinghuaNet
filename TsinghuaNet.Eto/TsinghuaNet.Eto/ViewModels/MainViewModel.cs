using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TsinghuaNet.Helpers;
using TsinghuaNet.Models;

namespace TsinghuaNet.Eto.ViewModels
{
    public class MainViewModel : NetViewModel
    {
        public MainViewModel() : base()
        {
            Status = new NetPingStatus();
            ReceivedResponse += Model_ReceivedResponse;
            timer = new UITimer(OnlineTimerTick);
            timer.Interval = 1;
            if (Settings.AutoLogin)
                Login();
            else
                Refresh();
        }

        public new NetSettings Settings
        {
            get => (NetSettings)base.Settings;
            set => base.Settings = value;
        }

        private const string SettingsFilename = "settings.json";
        private const string ProjectName = "TsinghuaNet.Eto";

        public override void LoadSettings()
        {
            Settings = new NetSettings();
            var path = SettingsFileHelper.GetSettingsPath(ProjectName, SettingsFilename);
            if (File.Exists(path))
            {
                using (StreamReader stream = new StreamReader(path))
                using (JsonTextReader reader = new JsonTextReader(stream))
                {
                    var json = JObject.Load(reader);
                    Credential.Username = (string)SettingsFileHelper.GetSettings(json, "username", string.Empty);
                    Credential.Password = Encoding.UTF8.GetString(Convert.FromBase64String((string)SettingsFileHelper.GetSettings(json, "password", string.Empty)));
                    Credential.State = (NetState)(int)SettingsFileHelper.GetSettings(json, "state", (int)NetState.Unknown);
                    Settings.AutoLogin = (bool)SettingsFileHelper.GetSettings(json, "autoLogin", true);
                    Settings.UseTimer = (bool)SettingsFileHelper.GetSettings(json, "useTimer", true);
                    Settings.EnableFluxLimit = (bool)SettingsFileHelper.GetSettings(json, "enableFluxLimit", true);
                    Settings.FluxLimit = ByteSize.FromGigaBytes((double)SettingsFileHelper.GetSettings(json, "fluxLimit", 20.0));
                }
            }
        }

        public override void SaveSettings()
        {
            JObject json = new JObject
            {
                ["username"] = Credential.Username ?? string.Empty,
                ["password"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(Credential.Password ?? string.Empty)),
                ["state"] = (int)Credential.State,
                ["autoLogin"] = Settings.AutoLogin,
                ["useTimer"] = Settings.UseTimer,
                ["enableFluxLimit"] = Settings.EnableFluxLimit,
                ["fluxLimit"] = Settings.FluxLimit.GigaBytes
            };
            SettingsFileHelper.CreateSettingsFolder(ProjectName);
            using (StreamWriter stream = new StreamWriter(SettingsFileHelper.GetSettingsPath(ProjectName, SettingsFilename)))
            using (JsonTextWriter writer = new JsonTextWriter(stream))
            {
                json.WriteTo(writer);
            }
        }

        private UITimer timer;
        private void OnlineTimerTick(object sender, EventArgs e)
        {
            OnlineTime += TimeSpan.FromSeconds(1);
        }

        protected override async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            var res = await base.RefreshAsync(helper);
            timer.Stop();
            OnlineTime = OnlineUser.OnlineTime;
            if (Settings.UseTimer && !string.IsNullOrEmpty(OnlineUser.Username))
                timer.Start();
            if (Settings.EnableFluxLimit && OnlineUser.Flux > Settings.FluxLimit)
                res = new LogResponse(false, $"流量已使用超过{Settings.FluxLimit}");
            return res;
        }

        private void Model_ReceivedResponse(object sender, LogResponse res) => Response = res.Message;
    }
}
