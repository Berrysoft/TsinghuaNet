﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TsinghuaNet.CrossPlatform.Models;
using Xamarin.Forms;

namespace TsinghuaNet.CrossPlatform.Views
{
    [DesignTimeVisible(true)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<MenuItemType, NavigationPage> MenuPages = new Dictionary<MenuItemType, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MenuPages.Add((int)MenuItemType.Info, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(MenuItemType id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case MenuItemType.Info:
                        MenuPages.Add(id, new NavigationPage(new InfoPage()));
                        break;
                    case MenuItemType.Browse:
                        MenuPages.Add(id, new NavigationPage(new ItemsPage()));
                        break;
                    case MenuItemType.Details:
                        MenuPages.Add(id, new NavigationPage(new DetailsPage()));
                        break;
                    case MenuItemType.Settings:
                        MenuPages.Add(id, new NavigationPage(new SettingsPage()));
                        break;
                    case MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}