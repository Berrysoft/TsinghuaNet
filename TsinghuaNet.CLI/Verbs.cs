using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using TsinghuaNet.Helpers;

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

    abstract class NetVerbBase : VerbBase
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

        public override async Task RunAsync()
        {
            using (var helper = await this.GetHelperAsync())
            {
                if (helper != null)
                {
                    var res = await helper.LoginAsync();
                    Console.WriteLine(res.Message);
                }
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
            using (var helper = await this.GetHelperAsync())
            {
                if (helper != null)
                {
                    var res = await helper.LogoutAsync();
                    Console.WriteLine(res.Message);
                }
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
            using (var helper = await this.GetHelperAsync())
            {
                if (helper != null)
                {
                    var flux = await helper.GetFluxAsync();
                    Console.WriteLine("用户：{0}", flux.Username);
                    Console.WriteLine("流量：{0}", flux.Flux);
                    Console.WriteLine("时长：{0}", flux.OnlineTime);
                    Console.WriteLine("流量：{0}", StringHelper.GetCurrencyString(flux.Balance));
                }
            }
        }
    }

    [Verb("online", HelpText = "查询在线IP")]
    class OnlineVerb : VerbBase
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
            using (var helper = this.GetUseregHelper())
            {
                var res = await helper.LoginAsync();
                if (res.Succeed)
                {
                    var users = await helper.GetUsersAsync();
                    Console.WriteLine("|       IP       |       登录时间       |   客户端   |");
                    Console.WriteLine(new string('=', 54));
                    foreach (var user in users)
                        Console.WriteLine("| {0,-14} | {1,-20} | {2,-10} |", user.Address, user.LoginTime.ToString(DateTimeFormat), user.Client);
                }
                else
                    Console.WriteLine(res.Message);
            }
        }
    }

    [Verb("drop", HelpText = "下线IP")]
    class DropVerb : VerbBase
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
            using (var helper = this.GetUseregHelper())
            {
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
    }

    [Verb("detail", HelpText = "流量明细")]
    class DetailVerb : VerbBase
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
            using (var helper = this.GetUseregHelper())
            {
                var res = await helper.LoginAsync();
                if (res.Succeed)
                {
                    if (Grouping)
                    {
                        var details = await helper.GetDetailsAsync(NetDetailOrder.LogoutTime, false);
                        var now = DateTime.Now;
                        Console.WriteLine("|    日期    |    流量    |");
                        Console.WriteLine(new string('=', 27));
                        var query = from d in details group d.Flux by d.LogoutTime.Day into g select new { Day = g.Key, TotalFlux = g.Sum() };
                        var orderedQuery = (int)Order == (int)NetDetailOrder.Flux ? query.OrderBy(d => d.TotalFlux, Descending) : query.OrderBy(d => d.Day, Descending);
                        foreach (var p in orderedQuery)
                            Console.WriteLine("| {0,-10} | {1,10} |", new DateTime(now.Year, now.Month, p.Day).ToString(DateFormat), p.TotalFlux);
                    }
                    else
                    {
                        var details = await helper.GetDetailsAsync((NetDetailOrder)Order, Descending);
                        Console.WriteLine("|       登录时间       |       注销时间       |    流量    |");
                        Console.WriteLine(new string('=', 60));
                        foreach (var d in details)
                            Console.WriteLine("| {0,-20} | {1,-20} | {2,10} |", d.LoginTime.ToString(DateTimeFormat), d.LogoutTime.ToString(DateTimeFormat), d.Flux);
                    }
                }
                else
                    Console.WriteLine(res.Message);
            }
        }
    }

    [Verb("suggestion", HelpText = "获取建议的连接方式")]
    class SuggestionVerb : VerbBase
    {
        public override async Task RunAsync()
        {
            Console.WriteLine(StringHelper.GetNetStateString(await VerbHelper.Status.SuggestAsync()));
        }
    }

    [Verb("savecred", HelpText = "保存用户名和密码")]
    class SaveCredentialVerb : VerbBase
    {
        public override Task RunAsync() => Task.Run(() => VerbHelper.Credential = VerbHelper.ReadCredential());
    }

    [Verb("deletecred", HelpText = "删除已保存的用户名和密码")]
    class DeleteCredentialVerb : VerbBase
    {
        public override Task RunAsync() => Task.Run(SettingsHelper.DeleteSettings);
    }
}
