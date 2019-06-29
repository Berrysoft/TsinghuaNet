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

        private const string settingsFilename = "settings.json";
        private string SettingsPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "TsinghuaNet.Eto", settingsFilename);
            }
        }

        private static JToken GetSettings(JObject json, string key, JToken def)
        {
            if (json.ContainsKey(key))
                return json[key];
            else
                return def;
        }

        public override void LoadSettings()
        {
            Settings = new NetSettings();
            if (File.Exists(SettingsPath))
            {
                using (StreamReader stream = new StreamReader(SettingsPath))
                using (JsonTextReader reader = new JsonTextReader(stream))
                {
                    var json = JObject.Load(reader);
                    Credential.Username = (string)GetSettings(json, "username", string.Empty);
                    Credential.Password = Encoding.UTF8.GetString(Convert.FromBase64String((string)GetSettings(json, "password", string.Empty)));
                    Credential.State = (NetState)(int)GetSettings(json, "state", (int)NetState.Unknown);
                    Settings.AutoLogin = (bool)GetSettings(json, "autoLogin", true);
                    Settings.UseTimer = (bool)GetSettings(json, "useTimer", true);
                    Settings.EnableFluxLimit = (bool)GetSettings(json, "enableFluxLimit", true);
                    Settings.FluxLimit = ByteSize.FromGigaBytes((double)GetSettings(json, "fluxLimit", 20.0));
                }
            }
        }

        private void CreateSettingsFolder()
        {
            DirectoryInfo home = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            home.CreateSubdirectory(Path.Combine(".config", "TsinghuaNet.Eto"));
        }

        public override void SaveSettings()
        {
            JObject json = new JObject();
            json["username"] = Credential.Username ?? string.Empty;
            json["password"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(Credential.Password ?? string.Empty));
            json["state"] = (int)Credential.State;
            json["autoLogin"] = Settings.AutoLogin;
            json["useTimer"] = Settings.UseTimer;
            json["enableFluxLimit"] = Settings.EnableFluxLimit;
            json["fluxLimit"] = Settings.FluxLimit.GigaBytes;
            CreateSettingsFolder();
            using (StreamWriter stream = new StreamWriter(SettingsPath))
            using (JsonTextWriter writer = new JsonTextWriter(stream))
            {
                json.WriteTo(writer);
            }
        }

        protected override void OnSuggestStateChanged()
        {
            base.OnSuggestStateChanged();
            Credential.State = SuggestState;
        }

        private TimeSpan onlineTime;
        public TimeSpan OnlineTime
        {
            get
            {
                return onlineTime;
            }
            set
            {
                SetProperty(ref onlineTime, value);
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

        private string response;
        public string Response
        {
            get => response;
            set => SetProperty(ref response, value);
        }

        private void Model_ReceivedResponse(object sender, LogResponse res) => Response = res.Message;
    }
}
