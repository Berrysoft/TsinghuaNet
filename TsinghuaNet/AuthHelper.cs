using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TsinghuaNet.Models;

namespace TsinghuaNet
{
    internal abstract class AuthHelper : NetHelperBase, IConnect
    {
        private const string LogUri = "https://auth{0}.tsinghua.edu.cn/cgi-bin/srun_portal";
        private const string FluxUri = "https://auth{0}.tsinghua.edu.cn/rad_user_info.php";
        private const string ChallengeUri = "https://auth{0}.tsinghua.edu.cn/cgi-bin/get_challenge?username={1}&double_stack=1&ip&callback=callback";
        private readonly int version;
        private readonly NetSettingsBase settings;

        public AuthHelper(string username, string password, HttpClient client, int version, NetSettingsBase settings = null)
            : base(username, password, client)
        {
            this.version = version;
            this.settings = settings;
        }

        private async Task<LogResponse> LogAsync(Func<int, Task<Dictionary<string, string>>> f)
        {
            LogResponse response = default;
            string uri = string.Format(LogUri, version);
            if (settings != null)
            {
                foreach (int ac_id in settings.AcIds)
                {
                    response = LogResponse.ParseFromAuth(await PostReturnBytesAsync(uri, await f(ac_id)));
                    if (response.Succeed)
                        break;
                }
            }
            if (!response.Succeed)
            {
                int ac_id = await GetAcId();
                if (ac_id > 0)
                {
                    if (settings != null) settings.AcIds.Add(ac_id);
                    return LogResponse.ParseFromAuth(await PostReturnBytesAsync(uri, await f(ac_id)));
                }
            }
            return response;
        }

        public Task<LogResponse> LoginAsync() => LogAsync(GetLoginDataAsync);

        private static readonly Regex AcIdRegex = new Regex(@"/index_([0-9]+)\.html");
        private async Task<int> GetAcId()
        {
            var response = await client.GetAsync(version == 4 ? "http://3.3.3.3/" : "http://[333::3]");
            var match = AcIdRegex.Match(await response.Content.ReadAsStringAsync());
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            return -1;
        }

        public async Task<FluxUser> GetFluxAsync() => FluxUser.Parse(await PostAsync(string.Format(FluxUri, version)), 2);

        private async Task<string> GetChallengeAsync()
        {
            byte[] result = await GetBytesAsync(string.Format(ChallengeUri, version, Username));
            JsonDocument json = JsonDocument.Parse(result.AsMemory().Slice(9, result.Length - 10));
            return json.RootElement.GetProperty("challenge").GetString();
        }

        private const string LoginInfoJson = "{{\"username\": \"{0}\", \"password\": \"{1}\", \"ip\": \"\", \"acid\": \"{2}\", \"enc_ver\": \"srun_bx1\"}}";
        private const string ChkSumData = "{0}{1}{0}{2}{0}{4}{0}{0}200{0}1{0}{3}";
        private async Task<Dictionary<string, string>> GetLoginDataAsync(int ac_id)
        {
            string token = await GetChallengeAsync();
            string passwordMD5 = CryptographyHelper.GetHMACMD5(token);
            string info = "{SRBX1}" + CryptographyHelper.Base64Encode(CryptographyHelper.XXTeaEncrypt(string.Format(LoginInfoJson, Username, Password, ac_id), token));
            return new Dictionary<string, string>
            {
                ["action"] = "login",
                ["ac_id"] = ac_id.ToString(),
                ["double_stack"] = "1",
                ["n"] = "200",
                ["type"] = "1",
                ["username"] = Username,
                ["password"] = "{MD5}" + passwordMD5,
                ["info"] = info,
                ["chksum"] = CryptographyHelper.GetSHA1(string.Format(ChkSumData, token, Username, passwordMD5, info, ac_id)),
                ["callback"] = "callback"
            };
        }

        public async Task<LogResponse> LogoutAsync()
        {
            string uri = string.Format(LogUri, version);
            return LogResponse.ParseFromAuth(await PostReturnBytesAsync(uri, new Dictionary<string, string>
            {
                ["action"] = "logout",
                ["ac_id"] = "1",
                ["double_stack"] = "1",
                ["username"] = Username,
                ["callback"] = "callback"
            }));
        }
    }

    internal class Auth4Helper : AuthHelper
    {
        public Auth4Helper(string username, string password, HttpClient client, NetSettingsBase settings = null)
            : base(username, password, client, 4, settings)
        { }
    }

    internal class Auth6Helper : AuthHelper
    {
        public Auth6Helper(string username, string password, HttpClient client, NetSettingsBase settings = null)
            : base(username, password, client, 6, settings)
        { }
    }
}
