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

        private static IConnect GetHelper(in NetCredential cred)
        {
            string username = cred.Username == null ? null : Marshal.PtrToStringUTF8(cred.Username);
            string password = cred.Password == null ? null : Marshal.PtrToStringUTF8(cred.Password);
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
            string username = cred.Username == null ? null : Marshal.PtrToStringUTF8(cred.Username);
            string password = cred.Password == null ? null : Marshal.PtrToStringUTF8(cred.Password);
            return new UseregHelper(username, password, GetClient(cred.UseProxy));
        }

        private static int WriteString(string message, IntPtr pout, int len)
        {
            if (pout != null && len > 0)
            {
                return Encoding.UTF8.GetBytes(message, new Span<byte>(pout.ToPointer(), len));
            }
            return 0;
        }

        private static string LastError;

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_last_err")]
        public static int GetLastError(IntPtr message, int len)
        {
            if (LastError != null)
            {
                return WriteString(LastError, message, len);
            }
            return 0;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_login")]
        public static int Login(in NetCredential cred)
        {
            try
            {
                IConnect helper = GetHelper(cred);
                if (helper != null)
                {
                    var task = helper.LoginAsync();
                    task.Wait();
                    var response = task.Result;
                    if (!response.Succeed)
                    {
                        LastError = response.Message;
                        return -1;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_logout")]
        public static int Logout(in NetCredential cred)
        {
            try
            {
                IConnect helper = GetHelper(cred);
                if (helper != null)
                {
                    var task = helper.LogoutAsync();
                    task.Wait();
                    var response = task.Result;
                    if (!response.Succeed)
                    {
                        LastError = response.Message;
                        return -1;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_status")]
        public static int Status(in NetCredential cred, ref NetFlux flux)
        {
            try
            {
                IConnect helper = GetHelper(cred);
                if (helper != null)
                {
                    var task = helper.GetFluxAsync();
                    task.Wait();
                    var response = task.Result;
                    if (Unsafe.AsPointer(ref flux) != null)
                    {
                        flux.Flux = response.Flux.Bytes;
                        flux.OnlineTime = (long)response.OnlineTime.TotalSeconds;
                        flux.Balance = (double)response.Balance;
                        return WriteString(response.Username, flux.Username, flux.UsernameLength);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_login")]
        public static int UseregLogin(in NetCredential cred)
        {
            try
            {
                IUsereg helper = GetUseregHelper(cred);
                var task = helper.LoginAsync();
                task.Wait();
                var response = task.Result;
                if (!response.Succeed)
                {
                    LastError = response.Message;
                    return -1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_logout")]
        public static int UseregLogout(in NetCredential cred)
        {
            try
            {
                IUsereg helper = GetUseregHelper(cred);
                var task = helper.LogoutAsync();
                task.Wait();
                var response = task.Result;
                if (!response.Succeed)
                {
                    LastError = response.Message;
                    return -1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_drop")]
        public static int UseregDrop(in NetCredential cred, long addr)
        {
            try
            {
                IUsereg helper = GetUseregHelper(cred);
                var task = helper.LogoutAsync(new IPAddress(addr));
                task.Wait();
                var response = task.Result;
                if (!response.Succeed)
                {
                    LastError = response.Message;
                    return -1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int UseregUsersCallback(in NetUser user, IntPtr data);

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_users")]
        public static int UseregUsers(in NetCredential cred, UseregUsersCallback callback, IntPtr data)
        {
            try
            {
                IUsereg helper = GetUseregHelper(cred);
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
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }

        public delegate int UseregDetailsCallback(in NetDetail detail, IntPtr data);

        [NativeCallable(CallingConvention = CallingConvention.Cdecl, EntryPoint = "tunet_usereg_details")]
        public static int UseregDetails(in NetCredential cred, Models.NetDetailOrder order, int descending, UseregDetailsCallback callback, IntPtr data)
        {
            try
            {
                IUsereg helper = GetUseregHelper(cred);
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
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
            return -1;
        }
    }
}
