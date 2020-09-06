using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using TsinghuaNet.Models;

namespace TsinghuaNet.CLI
{
    enum OptionNetState
    {
        Auto,
        Net,
        Auth4,
        Auth6
    }

    enum OptionNetDetailOrder
    {
        Login = 0,
        LoginTime = Login,
        Logout = 1,
        LogoutTime = Logout,
        Flux = 2
    }

    abstract class VerbBase
    {
        protected const string DateTimeFormat = "yyyy-M-d h:mm:ss";
        protected const string DateFormat = "yyyy-M-d";

        public abstract Task RunAsync();
    }

    abstract class WebVerbBase : VerbBase
    {
        [Option('p', "proxy", Required = false, Default = false, HelpText = "使用系统代理")]
        public bool UseProxy { get; set; }
    }

    abstract class NetVerbBase : WebVerbBase
    {
        [Option('s', "host", Required = false, Default = OptionNetState.Auto, HelpText = "连接方式：[auto], net, auth4, auth6")]
        public OptionNetState Host { get; set; }
    }

    [Verb("login", HelpText = "登录")]
    class LoginVerb : NetVerbBase
    {
        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("使用默认（自动判断）方式登录", new LoginVerb());
                yield return new Example("使用auth4方式登录", new LoginVerb() { Host = OptionNetState.Auth4 });
            }
        }

        [Option('r', "relog", Required = false, Default = false, HelpText = "重新登录")]
        public bool Relog { get; set; }

        public override async Task RunAsync()
        {
            var helper = await this.GetHelperAsync();
            if (helper != null)
            {
                if (Relog) await helper.LogoutAsync();
                var res = await helper.LoginAsync();
                Console.WriteLine(res.Message);
            }
        }
    }

    [Verb("logout", HelpText = "注销")]
    class LogoutVerb : NetVerbBase
    {
        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("使用默认（自动判断）方式注销", new LogoutVerb());
                yield return new Example("使用auth4方式注销", new LogoutVerb() { Host = OptionNetState.Auth4 });
            }
        }

        public override async Task RunAsync()
        {
            var helper = await this.GetHelperAsync();
            if (helper != null)
            {
                var res = await helper.LogoutAsync();
                Console.WriteLine(res.Message);
            }
        }
    }

    [Verb("status", HelpText = "查看在线状态")]
    class StatusVerb : NetVerbBase
    {
        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("使用默认（自动判断）方式", new StatusVerb());
                yield return new Example("使用auth4方式", new StatusVerb() { Host = OptionNetState.Auth4 });
            }
        }

        public override async Task RunAsync()
        {
            var helper = await this.GetHelperAsync();
            if (helper != null)
            {
                var flux = await helper.GetFluxAsync();
                Console.WriteLine("用户：{0}", flux.Username);
                Console.WriteLine("流量：{0}", flux.Flux);
                Console.WriteLine("时长：{0}", flux.OnlineTime);
                Console.WriteLine(string.Format(CultureInfo.GetCultureInfo("zh-CN"), "流量：{0:C2}", flux.Balance));
            }
        }
    }

    [Verb("online", HelpText = "查询在线IP")]
    class OnlineVerb : WebVerbBase
    {
        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("查询", new OnlineVerb());
            }
        }

        public override async Task RunAsync()
        {
            var helper = this.GetUseregHelper();
            var res = await helper.LoginAsync();
            if (res.Succeed)
            {
                var users = helper.GetUsersAsync();
                Console.WriteLine("|       IP       |       登录时间       |   Mac 地址   |");
                Console.WriteLine(new string('=', 56));
                await foreach (var user in users)
                    Console.WriteLine("| {0,-14} | {1,-20} | {2,-12} |", user.Address, user.LoginTime.ToString(DateTimeFormat), user.MacAddress);
            }
            else
                Console.WriteLine(res.Message);
        }
    }

    [Verb("connect", HelpText = "连线IP")]
    class ConnectVerb : WebVerbBase
    {
        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("连线一个IP", new ConnectVerb() { Address = "IP地址" });
            }
        }

        [Option('a', "address", Required = true, HelpText = "IP地址")]
        public string Address { get; set; }

        public override async Task RunAsync()
        {
            var ip = IPAddress.Parse(Address);
            var helper = this.GetUseregHelper();
            var res = await helper.LoginAsync();
            if (res.Succeed)
            {
                res = await helper.LoginAsync(ip);
                if (!res.Succeed)
                    Console.WriteLine(res.Message);
            }
            else
                Console.WriteLine(res.Message);
        }
    }

    [Verb("drop", HelpText = "下线IP")]
    class DropVerb : WebVerbBase
    {
        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("下线一个IP", new DropVerb() { Address = "IP地址" });
            }
        }

        [Option('a', "address", Required = true, HelpText = "IP地址")]
        public string Address { get; set; }

        public override async Task RunAsync()
        {
            var ip = IPAddress.Parse(Address);
            var helper = this.GetUseregHelper();
            var res = await helper.LoginAsync();
            if (res.Succeed)
            {
                res = await helper.LogoutAsync(ip);
                if (!res.Succeed)
                    Console.WriteLine(res.Message);
            }
            else
                Console.WriteLine(res.Message);
        }
    }

    [Verb("detail", HelpText = "流量明细")]
    class DetailVerb : WebVerbBase
    {
        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("使用默认排序（注销时间，升序）查询明细", new DetailVerb());
                yield return new Example("使用登陆时间（升序）查询明细", new DetailVerb() { Order = OptionNetDetailOrder.Login });
                yield return new Example("使用流量降序查询明细", new DetailVerb() { Order = OptionNetDetailOrder.Flux, Descending = true });
                yield return new Example("使用流量降序查询明细，并按注销日期组合", new DetailVerb() { Order = OptionNetDetailOrder.Flux, Descending = true, Grouping = true });
            }
        }

        [Option('o', "order", Required = false, Default = OptionNetDetailOrder.Logout, HelpText = "排序指标：[logout<time>], login<time>, flux")]
        public OptionNetDetailOrder Order { get; set; }
        [Option('d', "descending", Required = false, HelpText = "降序")]
        public bool Descending { get; set; }
        [Option('g', "grouping", Required = false, HelpText = "按注销日期组合")]
        public bool Grouping { get; set; }

        public override async Task RunAsync()
        {
            var helper = this.GetUseregHelper();
            var res = await helper.LoginAsync();
            if (res.Succeed)
            {
                ByteSize totalFlux = default;
                if (Grouping)
                {
                    var details = helper.GetDetailsAsync(NetDetailOrder.LogoutTime, false);
                    var now = DateTime.Now;
                    var query = details.GroupBy(d => d.LoginTime.Day, d => d.Flux).SelectAwait(async (g) => new { Day = g.Key, TotalFlux = await g.SumAsync() });
                    var orderedQuery = (int)Order == (int)NetDetailOrder.Flux ? query.OrderBy(d => d.TotalFlux, Descending) : query.OrderBy(d => d.Day, Descending);
                    Console.WriteLine("|    日期    |    流量    |");
                    string separater = new string('=', 27);
                    Console.WriteLine(separater);
                    await foreach (var p in orderedQuery)
                    {
                        Console.WriteLine("| {0,-10} | {1,10} |", new DateTime(now.Year, now.Month, p.Day).ToString(DateFormat), p.TotalFlux);
                        totalFlux += p.TotalFlux;
                    }
                    Console.WriteLine(separater);
                }
                else
                {
                    var details = helper.GetDetailsAsync((NetDetailOrder)Order, Descending);
                    Console.WriteLine("|       登录时间       |       注销时间       |    流量    |");
                    string separater = new string('=', 60);
                    Console.WriteLine(separater);
                    await foreach (var d in details)
                    {
                        Console.WriteLine("| {0,-20} | {1,-20} | {2,10} |", d.LoginTime.ToString(DateTimeFormat), d.LogoutTime.ToString(DateTimeFormat), d.Flux);
                        totalFlux += d.Flux;
                    }
                    Console.WriteLine(separater);
                }
                Console.WriteLine("总流量：{0}", totalFlux);
            }
            else
            {
                Console.WriteLine(res.Message);
            }
        }
    }

    [Verb("suggestion", HelpText = "获取建议的连接方式")]
    class SuggestionVerb : VerbBase
    {
        private static string GetNetStateString(NetState state)
        {
            return state switch
            {
                NetState.Net => "Net",
                NetState.Auth4 => "Auth4",
                NetState.Auth6 => "Auth6",
                _ => "不登录",
            };
        }

        public override async Task RunAsync()
        {
            Console.WriteLine(GetNetStateString(await VerbHelper.Status.SuggestAsync()));
        }
    }

    [Verb("savecred", HelpText = "保存用户名和密码")]
    class SaveCredentialVerb : VerbBase
    {
        public override Task RunAsync() => Task.Run(() => VerbHelper.Settings = VerbHelper.ReadCredential());
    }

    [Verb("deletecred", HelpText = "删除已保存的用户名和密码")]
    class DeleteCredentialVerb : VerbBase
    {
        public override Task RunAsync() => Task.Run(() =>
        {
            Console.Write("是否要删除设置文件？[y/N]");
            var de = Console.ReadLine();
            if (string.Equals(de, "y", StringComparison.OrdinalIgnoreCase))
            {
                if (SettingsHelper.Helper.DeleteSettings())
                {
                    Console.WriteLine("已删除");
                }
            }
        });
    }

    [Verb("about", HelpText = "关于本客户端")]
    class AboutVerb : VerbBase
    {
        private static readonly List<PackageBox> Packages = new List<PackageBox>
        {
            new PackageBox("CommandLineParser", "MIT"),
            new PackageBox("Fody", "MIT"),
            new PackageBox("HtmlAgilityPack", "MIT"),
            new PackageBox("PropertyChanged.Fody", "MIT"),
            new PackageBox("Refractored.MvvmHelpers", "MIT"),
            new PackageBox("System.Linq.Async", "Apache-2.0")
        };

        public override Task RunAsync() => Task.Run(() =>
        {
            Console.WriteLine("清华大学校园网客户端");
            Console.WriteLine("TsinghuaNet.CLI");
            Console.WriteLine(Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright);
            Console.WriteLine();
            Console.WriteLine("使用的开源库：");
            Console.WriteLine("|         名称         |  开源许可  |");
            Console.WriteLine(new string('=', 37));
            foreach (var p in Packages)
            {
                Console.WriteLine("| {0,-20} | {1,-10} |", p.Name, p.License);
            }
        });
    }
}
