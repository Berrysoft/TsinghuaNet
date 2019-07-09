using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using TsinghuaNet.Models;

namespace TsinghuaNet.ViewModels
{
    public class ConnectionViewModel : NetViewModelBase
    {
        public ConnectionViewModel()
        {
            NetUsers = new ObservableCollection<NetUser>();
            RefreshCommand = new Command(this, RefreshNetUsers);
            RefreshNetUsers();
        }

        public ObservableCollection<NetUser> NetUsers { get; }

        public ICommand RefreshCommand { get; }

        public async void RefreshNetUsers()
        {
            await RefreshNetUsersAsync();
        }

        public async Task RefreshNetUsersAsync()
        {
            try
            {
                var helper = Credential.GetUseregHelper();
                await helper.LoginAsync();
                await RefreshNetUsersAsync(helper);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task RefreshNetUsersAsync(IUsereg helper)
        {
            var users = await helper.GetUsersAsync().ToListAsync();
            var usersmodel = NetUsers;
            int i = 0;
            while (i < usersmodel.Count)
            {
                NetUser olduser = usersmodel[i];
                // 循环判断旧元素是否存在于新集合中
                for (var j = 0; j <= users.Count - 1; j++)
                {
                    NetUser user = users[j];
                    // 如果存在则移除新元素
                    if (olduser == user)
                    {
                        users.RemoveAt(j);
                        i += 1;
                        goto continue_while;
                    }
                }
                // 反之移除旧元素
                usersmodel.RemoveAt(i);
                continue_while:;
            }
            // 最后添加新增元素
            // 判断大小以防止索引错误
            foreach (var user in users)
                usersmodel.Add(user);
        }

        public Task DropAsync(params IPAddress[] ips) => DropAsync(ips.AsEnumerable());

        public async Task DropAsync(IEnumerable<IPAddress> ips)
        {
            try
            {
                var helper = Credential.GetUseregHelper();
                await helper.LoginAsync();
                foreach (var ip in ips)
                    await helper.LogoutAsync(ip);
                await RefreshNetUsersAsync(helper);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
