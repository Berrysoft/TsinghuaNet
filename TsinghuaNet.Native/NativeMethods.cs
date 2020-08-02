using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace TsinghuaNet.Native
{
    public static unsafe class NativeMethods
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly HttpClient NoProxyClient = new HttpClient(new SocketsHttpHandler() { UseProxy = false });

        static NativeMethods()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static HttpClient GetClient(int useProxy) => useProxy != 0 ? Client : NoProxyClient;

        private static IConnect? GetHelper(in NetCredential cred)
        {
            string? username = Marshal.PtrToStringUTF8(cred.Username);
            string? password = Marshal.PtrToStringUTF8(cred.Password);
            return cred.State switch
            {
                NetState.Net => new NetHelper(username, password, GetClient(cred.UseProxy)),
                NetState.Auth4 => new Auth4Helper(username, password, GetClient(cred.UseProxy)),
                NetState.Auth6 => new Auth6Helper(username, password, GetClient(cred.UseProxy)),
                _ => null
            };
        }

        private static IUsereg GetUseregHelper(in NetCredential cred)
        {
            string? username = Marshal.PtrToStringUTF8(cred.Username);
            string? password = Marshal.PtrToStringUTF8(cred.Password);
            return new UseregHelper(username, password, GetClient(cred.UseProxy));
        }

        private static IntPtr WriteString(string? message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    return Marshal.StringToCoTaskMemUTF8(message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            return IntPtr.Zero;
        }

        private static string? LastError;

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_last_err")]
        public static IntPtr GetLastError()
        {
            return WriteString(LastError);
        }

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_string_free")]
        public static void StringFree(IntPtr str)
        {
            Marshal.ZeroFreeCoTaskMemUTF8(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int UseHelper(in NetCredential cred, Func<IConnect, int> action)
        {
            try
            {
                IConnect? helper = GetHelper(in cred);
                if (helper != null)
                {
                    return action(helper);
                }
                return 0;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int UseUseregHelper(in NetCredential cred, Func<IUsereg, int> action)
        {
            try
            {
                IUsereg helper = GetUseregHelper(in cred);
                return action(helper);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_login")]
        public static int Login(in NetCredential cred) => UseHelper(in cred, helper =>
        {
            var task = helper.LoginAsync();
            task.Wait();
            var response = task.Result;
            if (!response.Succeed)
            {
                LastError = response.Message;
                return -1;
            }
            return 0;
        });

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_logout")]
        public static int Logout(in NetCredential cred) => UseHelper(in cred, helper =>
        {
            var task = helper.LogoutAsync();
            task.Wait();
            var response = task.Result;
            if (!response.Succeed)
            {
                LastError = response.Message;
                return -1;
            }
            return 0;
        });

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_status")]
        public static int Status(in NetCredential cred, NetFlux* flux) => UseHelper(in cred, helper =>
        {
            var task = helper.GetFluxAsync();
            task.Wait();
            var response = task.Result;
            if (flux != null)
            {
                flux->Username = WriteString(response.Username);
                flux->Flux = response.Flux.Bytes;
                flux->OnlineTime = (long)response.OnlineTime.TotalSeconds;
                flux->Balance = (double)response.Balance;
            }
            return 0;
        });

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_login")]
        public static int UseregLogin(in NetCredential cred) => UseUseregHelper(in cred, helper =>
        {
            var task = helper.LoginAsync();
            task.Wait();
            var response = task.Result;
            if (!response.Succeed)
            {
                LastError = response.Message;
                return -1;
            }
            return 0;
        });

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_logout")]
        public static int UseregLogout(in NetCredential cred) => UseUseregHelper(in cred, helper =>
        {
            var task = helper.LogoutAsync();
            task.Wait();
            var response = task.Result;
            if (!response.Succeed)
            {
                LastError = response.Message;
                return -1;
            }
            return 0;
        });

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_drop")]
        public static int UseregDrop(in NetCredential cred, long addr) => UseUseregHelper(in cred, helper =>
        {
            var task = helper.LogoutAsync(new IPAddress(addr));
            task.Wait();
            var response = task.Result;
            if (!response.Succeed)
            {
                LastError = response.Message;
                return -1;
            }
            return 0;
        });

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_users")]
        public static int UseregUsers(in NetCredential cred, delegate* cdecl<in NetUser, IntPtr, int> callback, IntPtr data) => UseUseregHelper(in cred, helper =>
        {
            var task = helper.GetUsersAsync().ToArrayAsync();
            var users = task.GetAwaiter().GetResult();
            if (callback != null)
            {
                for (int i = 0; i < users.Length; i++)
                {
                    ref var u = ref users[i];
                    NetUser user;
#pragma warning disable 0618
                    user.Address = u.Address.Address;
#pragma warning restore 0618
                    user.LoginTime = new DateTimeOffset(u.LoginTime).ToUnixTimeSeconds();
                    fixed (byte* ps = u.MacAddress.GetAddressBytes())
                    {
                        Unsafe.CopyBlock(user.MacAddress, ps, 6);
                    }
                    if (callback(in user, data) == 0)
                    {
                        break;
                    }
                }
            }
            return users.Length;
        });

        [UnmanagedCallersOnly(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_details")]
        public static int UseregDetails(in NetCredential cred, Models.NetDetailOrder order, int descending, delegate* cdecl<in NetDetail, IntPtr, int> callback, IntPtr data) => UseUseregHelper(in cred, helper =>
        {
            var task = helper.GetDetailsAsync(order, descending != 0).ToArrayAsync();
            var details = task.GetAwaiter().GetResult();
            if (callback != null)
            {
                for (int i = 0; i < details.Length; i++)
                {
                    ref var d = ref details[i];
                    NetDetail detail;
                    detail.LoginTime = new DateTimeOffset(d.LoginTime).ToUnixTimeSeconds();
                    detail.LogoutTime = new DateTimeOffset(d.LogoutTime).ToUnixTimeSeconds();
                    detail.Flux = d.Flux.Bytes;
                    if (callback(in detail, data) == 0)
                    {
                        break;
                    }
                }
            }
            return details.Length;
        });
    }
}
