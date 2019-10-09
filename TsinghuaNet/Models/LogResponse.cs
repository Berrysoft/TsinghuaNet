﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace TsinghuaNet.Models
{
    public readonly struct LogResponse
    {
        public LogResponse(bool succeed, string message)
        {
            Succeed = succeed;
            Message = message;
        }

        public readonly bool Succeed;

        public readonly string Message;

        public static bool operator ==(LogResponse r1, LogResponse r2) => r1.Succeed == r2.Succeed && r1.Message == r2.Message;
        public static bool operator !=(LogResponse r1, LogResponse r2) => !(r1 == r2);

        public override bool Equals(object obj)
            => obj is LogResponse res && this == res;

        public override int GetHashCode() => Succeed.GetHashCode() ^ (Message?.GetHashCode() ?? 0);

        internal static LogResponse ParseFromNet(string response)
        {
            return new LogResponse(response == "Login is successful." || response == "Logout is successful." || response == "IP has been online, please logout.", response);
        }

        private static readonly Dictionary<string, string> PortalError = new Dictionary<string, string>
        {
            ["E3001"] = "流量或时长已用尽",
            ["E3002"] = "计费策略条件不匹配",
            ["E3003"] = "控制策略条件不匹配",
            ["E3004"] = "余额不足",
            ["E3005"] = "在线变更计费策略",
            ["E3006"] = "在线变更控制策略",
            ["E3007"] = "超时",
            ["E3008"] = "连线数超额，挤出在线表。",
            ["E3009"] = "有代理行为",
            ["E3010"] = "无流量超时",
            ["E3101"] = "心跳包超时",
            ["E4001"] = "Radius表DM下线",
            ["E4002"] = "DHCP表DM下线",
            ["E4003"] = "Juniper IPOE COA上线",
            ["E4004"] = "Juniper IPOE COA下线",
            ["E4005"] = "proxy表DM下线",
            ["E4006"] = "COA在线更改带宽",
            ["E4007"] = "本地下线",
            ["E4008"] = "虚拟下线",
            ["E4009"] = "策略切换时下发COA",
            ["E4011"] = "结算时虚拟下线",
            ["E4012"] = "下发COA",
            ["E4101"] = "来自radius模块的DM下线(挤出在线表)",
            ["E4102"] = "来自系统设置(8081)的DM下线",
            ["E4103"] = "来自后台管理(8080)的DM下线",
            ["E4104"] = "来自自服务(8800)的DM下线",
            ["E4112"] = "来自系统设置(8081)的本地下线",
            ["E4113"] = "来自后台管理(8080)的本地下线",
            ["E4114"] = "来自自服务(8800)的本地下线",
            ["E4122"] = "来自系统设置(8081)的虚拟下线",
            ["E4123"] = "来自后台管理(8080)的虚拟下线",
            ["E4124"] = "来自自服务(8800)的虚拟下线",
            ["E2531"] = "用户不存在",
            ["E2532"] = "两次认证的间隔太短",
            ["E2533"] = "尝试次数过于频繁",
            ["E2534"] = "有代理行为被暂时禁用",
            ["E2535"] = "认证系统已关闭",
            ["E2536"] = "系统授权已过期",
            ["E2553"] = "密码错误",
            ["E2601"] = "不是专用客户端",
            ["E2606"] = "用户被禁用",
            ["E2611"] = "MAC绑定错误",
            ["E2612"] = "MAC在黑名单中",
            ["E2613"] = "NAS PORT绑定错误",
            ["E2614"] = "VLAN ID绑定错误",
            ["E2615"] = "IP绑定错误",
            ["E2616"] = "已欠费",
            ["E2620"] = "已经在线了",
            ["E2806"] = "找不到符合条件的产品",
            ["E2807"] = "找不到符合条件的计费策略",
            ["E2808"] = "找不到符合条件的控制策略",
            ["E2833"] = "IP地址异常，请重新拿地址",
            ["E5990"] = "数据不完整",
            ["E5991"] = "无效的参数",
            ["E5992"] = "找不到这个用户",
            ["E5993"] = "用户已存在",
            ["E5001"] = "用户创建成功",
            ["E5002"] = "用户创建失败",
            ["E5010"] = "修改用户成功",
            ["E5011"] = "修改用户失败",
            ["E5020"] = "修改用户成功",
            ["E5021"] = "修改用户失败",
            ["E5030"] = "转组成功",
            ["E5031"] = "转组失败",
            ["E5040"] = "购买套餐成功",
            ["E5041"] = "购买套餐失败",
            ["E5042"] = "找不到套餐",
            ["E5050"] = "绑定MAC认证成功",
            ["E5051"] = "解绑MAC认证成功",
            ["E5052"] = "绑定MAC成功",
            ["E5053"] = "解绑MAC成功",
            ["E5054"] = "绑定nas port成功",
            ["E5055"] = "解绑nas port成功",
            ["E5056"] = "绑定vlan id成功",
            ["E5057"] = "解绑vlan id成功",
            ["E5058"] = "绑定ip成功",
            ["E5059"] = "解绑ip成功",
            ["E6001"] = "用户缴费成功",
            ["E6002"] = "用户缴费失败",
            ["E7001"] = "用户不存在",
            ["E7002"] = "添加待结算队列失败",
            ["E7003"] = "结算成功",
            ["E7004"] = "添加已结算队列失败",
            ["E7005"] = "扣除产品实例结算金额失败",
            ["E7006"] = "没有找到产品实例",
            ["E7007"] = "没有对该用户进行手动结算的权限",
            ["E7008"] = "没有对该产品进行手动结算的权限",
            ["E7009"] = "由于使用流量小于该产品结算设置而不扣费",
            ["E7010"] = "由于使用时长小于该产品结算设置而不扣费",
            ["E7011"] = "由于产品余额不足，根据结算设置而不扣费",
            ["E7012"] = "由于产品余额不足，根据结算设置余额扣为0",
            ["E7013"] = "由于产品余额不足，根据结算设置余额扣为负值",
            ["E7014"] = "删除过期套餐操作成功",
            ["E7015"] = "删除过期套餐操作失败",
            ["E7016"] = "自动购买套餐成功",
            ["E7017"] = "自动购买套餐失败",
            ["E7018"] = "产品结算模式错误",
            ["vcode_error"] = "验证码错误",
        };

        internal static LogResponse ParseFromAuth(byte[] response)
        {
            try
            {
                var jsonstr = response.AsMemory().Slice(9, response.Length - 10);
                JsonDocument json = JsonDocument.Parse(jsonstr);
                JsonElement root = json.RootElement;
                string error = root.GetProperty("error").GetString();
                string ecode = root.GetProperty("ecode").ToString();
                if (PortalError.TryGetValue(ecode, out string message))
                {
                    return new LogResponse(error == "ok", message);
                }
                else
                {
                    string error_msg = root.GetProperty("error_msg").GetString();
                    return new LogResponse(error == "ok", $"error: {error}; error_msg: {error_msg}");
                }
            }
            catch (Exception)
            {
                return new LogResponse(false, Encoding.UTF8.GetString(response));
            }
        }

        internal static LogResponse ParseFromUsereg(string response)
        {
            return new LogResponse(response == "ok", response);
        }
    }
}
