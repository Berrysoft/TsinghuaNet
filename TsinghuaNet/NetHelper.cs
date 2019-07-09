using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TsinghuaNet.Models;

namespace TsinghuaNet
{
    internal class NetHelper : NetHelperBase, IConnect
    {
        private const string LogUri = "http://net.tsinghua.edu.cn/do_login.php";
        private const string FluxUri = "http://net.tsinghua.edu.cn/rad_user_info.php";
        private const string LogoutData = "action=logout";

        public NetHelper(string username, string password, HttpClient client)
            : base(username, password, client)
        { }

        public async Task<LogResponse> LoginAsync() =>
            LogResponse.ParseFromNet(await PostAsync(LogUri, new Dictionary<string, string>
            {
                ["action"] = "login",
                ["ac_id"] = "1",
                ["username"] = Username,
                ["password"] = "{MD5_HEX}" + CryptographyHelper.GetMD5(Password)
            }));

        public async Task<LogResponse> LogoutAsync() => LogResponse.ParseFromNet(await PostAsync(LogUri, LogoutData));

        public async Task<FluxUser> GetFluxAsync() => FluxUser.Parse(await PostAsync(FluxUri));
    }
}
