using System;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace TsinghuaNet.Native
{
    public static unsafe class NativeMethods
    {
        private static readonly HttpClient Http = new HttpClient();

        private static IConnect GetHelper(NetCredential* cred)
        {
            string username = cred->Username == null ? null : Marshal.PtrToStringUTF8(cred->Username);
            string password = cred->Password == null ? null : Marshal.PtrToStringUTF8(cred->Password);
            return cred->State switch
            {
                NetState.Net => new NetHelper(username, password, Http),
                NetState.Auth4 => new Auth4Helper(username, password, Http),
                NetState.Auth6 => new Auth6Helper(username, password, Http),
                _ => null
            };
        }

        private static IUsereg GetUseregHelper(NetCredential* cred)
        {
            string username = cred->Username == null ? null : Marshal.PtrToStringUTF8(cred->Username);
            string password = cred->Password == null ? null : Marshal.PtrToStringUTF8(cred->Password);
            return new UseregHelper(username, password, Http);
        }

        private static void WriteString(string message, IntPtr pout, int len)
        {
            if (pout != null && len > 0)
            {
                var msgdata = Encoding.UTF8.GetBytes(message);
                Marshal.Copy(msgdata, 0, pout, Math.Min(len, msgdata.Length));
            }
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_login")]
        public static int Login(NetCredential* cred, IntPtr message, int len)
        {
            IConnect helper = GetHelper(cred);
            if (helper != null)
            {
                var task = helper.LoginAsync();
                task.Wait();
                var response = task.Result;
                if (response.Succeed)
                {
                    WriteString(response.Message, message, len);
                    return 0;
                }
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_logout")]
        public static int Logout(NetCredential* cred, IntPtr message, int len)
        {
            IConnect helper = GetHelper(cred);
            if (helper != null)
            {
                var task = helper.LogoutAsync();
                task.Wait();
                var response = task.Result;
                if (response.Succeed)
                {
                    WriteString(response.Message, message, len);
                    return 0;
                }
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_status")]
        public static int Status(NetCredential* cred, NetFlux* flux)
        {
            IConnect helper = GetHelper(cred);
            if (helper != null)
            {
                var task = helper.GetFluxAsync();
                task.Wait();
                var response = task.Result;
                if (flux != null)
                {
                    WriteString(response.Username, flux->Username, flux->UsernameLength);
                    flux->Flux = response.Flux.Bytes;
                    flux->OnlineTime = (long)response.OnlineTime.TotalSeconds;
                    flux->Balance = (double)response.Balance;
                }
                return 0;
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_login")]
        public static int UseregLogin(NetCredential* cred, IntPtr message, int len)
        {
            IUsereg helper = GetUseregHelper(cred);
            var task = helper.LoginAsync();
            task.Wait();
            var response = task.Result;
            if (response.Succeed)
            {
                WriteString(response.Message, message, len);
                return 0;
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_logout")]
        public static int UseregLogout(NetCredential* cred, IntPtr message, int len)
        {
            IUsereg helper = GetUseregHelper(cred);
            var task = helper.LogoutAsync();
            task.Wait();
            var response = task.Result;
            if (response.Succeed)
            {
                WriteString(response.Message, message, len);
                return 0;
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_drop")]
        public static int UseregDrop(NetCredential* cred, int addr, IntPtr message, int len)
        {
            IUsereg helper = GetUseregHelper(cred);
            var task = helper.LogoutAsync(new IPAddress(addr));
            task.Wait();
            var response = task.Result;
            if (response.Succeed)
            {
                WriteString(response.Message, message, len);
                return 0;
            }
            return -1;
        }
    }
}
