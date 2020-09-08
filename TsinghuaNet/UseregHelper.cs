using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using HtmlAgilityPack;
using TsinghuaNet.Models;

namespace TsinghuaNet
{
    internal class UseregHelper : NetHelperBase, IUsereg
    {
        private const string BaseUri = "http://usereg.tsinghua.edu.cn/";
        private const string LogUri = BaseUri + "do.php";
        private const string InfoUri = BaseUri + "online_user_ipv4.php";
        private const string DetailUri = BaseUri + "user_detail_list.php?action=query&desc={6}&order={5}&start_time={0}-{1:D2}-01&end_time={0}-{1:D2}-{2:D2}&page={3}&offset={4}";
        private const string ConnectUri = BaseUri + "ip_login.php";
        private const string LogoutData = "action=logout";
        private const string DropData = "action=drop&user_ip={0}";

        public UseregHelper(string username, string password, HttpClient client)
            : base(username, password, client)
        { }

        public async Task<LogResponse> LoginAsync() => LogResponse.ParseFromUsereg(await PostAsync(LogUri, new Dictionary<string, string>
        {
            ["action"] = "login",
            ["user_login_name"] = Username,
            ["user_password"] = CryptographyHelper.GetMD5(Password)
        }));

        public async Task<LogResponse> LoginAsync(IPAddress ip) => LogResponse.ParseFromUsereg(await PostAsync(ConnectUri, new Dictionary<string, string>
        {
            ["user_ip"] = ip.ToString(),
            ["drop"] = "0"
        }));

        public async Task<LogResponse> LogoutAsync() => LogResponse.ParseFromUsereg(await PostAsync(LogUri, LogoutData));

        public async Task<LogResponse> LogoutAsync(IPAddress ip) => LogResponse.ParseFromUsereg(await PostAsync(InfoUri, string.Format(DropData, ip.ToString())));

        public async IAsyncEnumerable<NetUser> GetUsersAsync()
        {
            string userhtml = await GetAsync(InfoUri);
            var doc = new HtmlDocument();
            doc.LoadHtml(userhtml);
            foreach (var tr in doc.DocumentNode.SelectNodes("//tr[@align='center']").Skip(1))
            {
                var tds = tr.Elements("td").Skip(1).Select(td => td.FirstChild?.InnerText).ToArray();
                yield return new NetUser(
                    IPAddress.Parse(tds[0]),
                    DateTime.ParseExact(tds[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    string.IsNullOrEmpty(tds[6]) ?
                        PhysicalAddress.None :
                        PhysicalAddress.Parse(
                            tds[6].
                            Replace(".", "").
                            Replace("-", "").
                            Replace(":", "").
                            ToUpperInvariant()));
            }
        }

        private readonly static Dictionary<NetDetailOrder, string> OrderQueryMap = new Dictionary<NetDetailOrder, string>
        {
            [NetDetailOrder.LoginTime] = "user_login_time",
            [NetDetailOrder.LogoutTime] = "user_drop_time",
            [NetDetailOrder.Flux] = "user_in_bytes",
        };

        public async IAsyncEnumerable<NetDetail> GetDetailsAsync(NetDetailOrder order, bool descending)
        {
            const int offset = 100;
            DateTime now = DateTime.Now;
            for (int i = 1; ; i++)
            {
                string detailhtml = await GetAsync(string.Format(DetailUri, now.Year, now.Month, now.Day, i, offset, OrderQueryMap[order], descending ? "DESC" : string.Empty));
                var doc = new HtmlDocument();
                doc.LoadHtml(detailhtml);
                int count = 0;
                foreach (var tr in doc.DocumentNode.SelectNodes("//tr[@align='center']").Skip(1))
                {
                    var tds = tr.Elements("td").Skip(1).Select(td => td.FirstChild?.InnerText).ToArray();
                    var loginTime = DateTime.ParseExact(tds[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    if (loginTime.Month == now.Month)
                    {
                        count++;
                        yield return new NetDetail(
                            loginTime,
                            DateTime.ParseExact(tds[2], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            ByteSize.Parse(tds[4]));
                    }
                }
                if (count < offset) break;
            }
        }
    }
}
