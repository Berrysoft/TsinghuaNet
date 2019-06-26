using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TsinghuaNet
{
    /// <summary>
    /// Exposes methods to login, logout and get flux from http://net.tsinghua.edu.cn/
    /// </summary>
    public class NetHelper : NetHelperBase, IConnect
    {
        private const string LogUri = "http://net.tsinghua.edu.cn/do_login.php";
        private const string FluxUri = "http://net.tsinghua.edu.cn/rad_user_info.php";
        private const string LogoutData = "action=logout";
        /// <summary>
        /// Initializes a new instance of the <see cref="NetHelper"/> class.
        /// </summary>
        /// <param name="username">The username to login.</param>
        /// <param name="password">The password to login.</param>
        /// <param name="client">A user-specified instance of <see cref="HttpClient"/>.</param>
        public NetHelper(string username, string password, HttpClient client)
            : base(username, password, client)
        { }
        /// <summary>
        /// Login to the network.
        /// </summary>
        /// <returns>The response of the website.</returns>
        public async Task<LogResponse> LoginAsync() =>
            LogResponse.ParseFromNet(await PostAsync(LogUri, new Dictionary<string, string>
            {
                ["action"] = "login",
                ["ac_id"] = "1",
                ["username"] = Username,
                ["password"] = "{MD5_HEX}" + CryptographyHelper.GetMD5(Password)
            }));
        /// <summary>
        /// Logout from the network.
        /// </summary>
        /// <returns>The response of the website.</returns>
        public async Task<LogResponse> LogoutAsync() => LogResponse.ParseFromNet(await PostAsync(LogUri, LogoutData));
        /// <summary>
        /// Get information of the user online.
        /// </summary>
        /// <returns>An instance of <see cref="FluxUser"/> class of the current user.</returns>
        public async Task<FluxUser> GetFluxAsync() => FluxUser.Parse(await PostAsync(FluxUri));
    }
}
