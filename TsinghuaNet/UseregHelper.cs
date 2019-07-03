using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace TsinghuaNet
{
    public struct NetUser
    {
        public NetUser(IPAddress address, DateTime loginTime, string client)
        {
            Address = address;
            LoginTime = loginTime;
            Client = client;
        }

        public IPAddress Address { get; }

        public DateTime LoginTime { get; }

        public string Client { get; }

        public static bool operator ==(NetUser u1, NetUser u2) => u1.Address.Equals(u2.Address) && u1.LoginTime == u2.LoginTime && u1.Client == u2.Client;

        public static bool operator !=(NetUser u1, NetUser u2) => !(u1 == u2);

        public override bool Equals(object obj)
            => obj is NetUser user && this == user;

        public override int GetHashCode() => (Address?.GetHashCode() ?? 0) ^ LoginTime.GetHashCode() ^ (Client?.GetHashCode() ?? 0);
    }

    public struct NetDetail
    {
        public NetDetail(DateTime login, DateTime logout, ByteSize flux)
        {
            LoginTime = login;
            LogoutTime = logout;
            Flux = flux;
        }

        public DateTime LoginTime { get; }

        public DateTime LogoutTime { get; }

        public ByteSize Flux { get; }

        public static bool operator ==(NetDetail d1, NetDetail d2) => d1.LoginTime == d2.LoginTime && d1.LogoutTime == d2.LogoutTime && d1.Flux == d2.Flux;

        public static bool operator !=(NetDetail d1, NetDetail d2) => !(d1 == d2);

        public override bool Equals(object obj)
            => obj is NetDetail other && this == other;

        public override int GetHashCode() => LoginTime.GetHashCode() ^ LogoutTime.GetHashCode() ^ Flux.GetHashCode();
    }

    public enum NetDetailOrder
    {
        LoginTime,
        LogoutTime,
        Flux
    }

    public class UseregHelper : NetHelperBase, ILog
    {
        private const string LogUri = "http://usereg.tsinghua.edu.cn/do.php";
        private const string InfoUri = "http://usereg.tsinghua.edu.cn/online_user_ipv4.php";
        private const string DetailUri = "http://usereg.tsinghua.edu.cn/user_detail_list.php?action=query&desc={6}&order={5}&start_time={0}-{1}-01&end_time={0}-{1}-{2}&page={3}&offset={4}";
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

        public async Task<LogResponse> LogoutAsync() => LogResponse.ParseFromUsereg(await PostAsync(LogUri, LogoutData));

        public async Task<LogResponse> LogoutAsync(IPAddress ip) => LogResponse.ParseFromUsereg(await PostAsync(InfoUri, string.Format(DropData, ip.ToString())));

        public async IAsyncEnumerable<NetUser> GetUsersAsync()
        {
            string userhtml = await GetAsync(InfoUri);
            var doc = new HtmlDocument();
            doc.LoadHtml(userhtml);
            foreach (var u in
                from tr in doc.DocumentNode.Element("html").Element("body").Element("table").Element("tr").Elements("td").Last().Elements("table").ElementAt(1).Elements("tr").Skip(1)
                let tds = (from td in tr.Elements("td").Skip(1)
                           select td.FirstChild?.InnerText).ToArray()
                select new NetUser(
                    IPAddress.Parse(tds[0]),
                    DateTime.ParseExact(tds[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    tds[10]))
            {
                yield return u;
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
                string detailhtml = await GetAsync(string.Format(DetailUri, now.Year, now.Month.ToString().PadLeft(2, '0'), now.Day, i, offset, OrderQueryMap[order], descending ? "DESC" : string.Empty));
                var doc = new HtmlDocument();
                doc.LoadHtml(detailhtml);
                bool cont = false;
                foreach (var d in
                    from tr in doc.DocumentNode.Element("html").Element("body").Element("table").Element("tr").Elements("td").Last().Elements("table").Last().Elements("tr").Skip(1)
                    let tds = (from td in tr.Elements("td").Skip(1)
                               select td.FirstChild?.InnerText).ToArray()
                    select new NetDetail(
                        DateTime.ParseExact(tds[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        DateTime.ParseExact(tds[2], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        ByteSize.Parse(tds[4])))
                {
                    cont = true;
                    yield return d;
                }
                if (!cont) break;
            }
        }
    }
}
