#include "pch.h"

#include "ArcUserContent.h"
#include "GraphUserContent.h"
#include "LineUserContent.h"
#include "MainViewModel.h"
#include "WaterUserContent.h"

using namespace std;
using namespace sf;
using namespace winrt;
using namespace Windows::ApplicationModel;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    MainViewModel::MainViewModel()
    {
        m_NetUsers = single_threaded_observable_vector<IInspectable>();
    }

    hstring MainViewModel::GetVersionString(PackageVersion const& ver) const
    {
        return hstring(wsprint(L"{}.{}.{}.{}", ver.Major, ver.Minor, ver.Build, ver.Revision));
    }

    void MainViewModel::OnAutoLoginPropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto model{ d.try_as<class_type>() })
        {
            MainViewModel* pm{ get_self<MainViewModel>(model) };
            pm->m_AutoLoginChangedEvent(model, unbox_value<bool>(e.NewValue()));
        }
    }

    void MainViewModel::OnBackgroundAutoLoginPropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto model{ d.try_as<class_type>() })
        {
            MainViewModel* pm{ get_self<MainViewModel>(model) };
            pm->m_BackgroundAutoLoginChangedEvent(model, unbox_value<bool>(e.NewValue()));
        }
    }

    void MainViewModel::OnBackgroundLiveTilePropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto model{ d.try_as<class_type>() })
        {
            MainViewModel* pm{ get_self<MainViewModel>(model) };
            pm->m_BackgroundLiveTileChangedEvent(model, unbox_value<bool>(e.NewValue()));
        }
    }

    void MainViewModel::OnSettingsThemePropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto model{ d.try_as<class_type>() })
        {
            auto theme{ unbox_value<UserTheme>(e.NewValue()) };
            ElementTheme actheme{ (int)theme };
            if (theme == UserTheme::Auto)
            {
                time_t time = clock::to_time_t(clock::now());
                tm lt;
                localtime_s(&lt, &time);
                if (lt.tm_hour <= 6 || lt.tm_hour >= 18)
                {
                    actheme = ElementTheme::Dark;
                }
                else
                {
                    actheme = ElementTheme::Light;
                }
            }
            model.Theme(actheme);
        }
    }

    void MainViewModel::OnContentTypePropertyChanged(DependencyObject const& d, DependencyPropertyChangedEventArgs const& e)
    {
        if (auto model{ d.try_as<class_type>() })
        {
            auto oldc{ model.UserContent().try_as<IUserContent>() };
            IUserContent newc;
            switch (unbox_value<UserContentType>(e.NewValue()))
            {
            case UserContentType::Line:
                newc = make<LineUserContent>();
                break;
            case UserContentType::Ring:
                newc = make<ArcUserContent>();
                break;
            case UserContentType::Water:
                newc = make<WaterUserContent>();
                break;
            case UserContentType::Graph:
                newc = make<GraphUserContent>();
                break;
            }
            if (oldc)
            {
                newc.User(oldc.User());
            }
            model.UserContent(newc.try_as<UIElement>());

            MainViewModel* pm(get_self<MainViewModel>(model));
            pm->m_ContentTypeChangedEvent(model, unbox_value<UserContentType>(e.NewValue()));
        }
    }

    DEPENDENCY_PROPERTY_INIT(UserContent, UIElement, MainViewModel, nullptr)

    DEPENDENCY_PROPERTY_INIT(Response, LogResponse, MainViewModel, box_value(LogResponse{}))

    DEPENDENCY_PROPERTY_INIT(Username, hstring, MainViewModel, box_value(hstring{}))
    DEPENDENCY_PROPERTY_INIT(Password, hstring, MainViewModel, box_value(hstring{}))

    DEPENDENCY_PROPERTY_INIT(State, NetState, MainViewModel, box_value(NetState::Unknown))

    DEPENDENCY_PROPERTY_INIT(AutoLogin, bool, MainViewModel, box_value(true), &MainViewModel::OnAutoLoginPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(BackgroundAutoLogin, bool, MainViewModel, box_value(true), &MainViewModel::OnBackgroundAutoLoginPropertyChanged)
    DEPENDENCY_PROPERTY_INIT(BackgroundLiveTile, bool, MainViewModel, box_value(true), &MainViewModel::OnBackgroundLiveTilePropertyChanged)

    DEPENDENCY_PROPERTY_INIT(NetStatus, InternetStatus, MainViewModel, box_value(InternetStatus::Unknown))
    DEPENDENCY_PROPERTY_INIT(Ssid, hstring, MainViewModel, box_value(hstring{}))
    DEPENDENCY_PROPERTY_INIT(SuggestState, NetState, MainViewModel, box_value(NetState::Unknown))

    DEPENDENCY_PROPERTY_INIT(SettingsTheme, UserTheme, MainViewModel, box_value(UserTheme::Default), &MainViewModel::OnSettingsThemePropertyChanged)
    DEPENDENCY_PROPERTY_INIT(Theme, ElementTheme, MainViewModel, box_value(ElementTheme::Default))
    DEPENDENCY_PROPERTY_INIT(ContentType, UserContentType, MainViewModel, box_value(UserContentType::Ring), &MainViewModel::OnContentTypePropertyChanged)

    DEPENDENCY_PROPERTY_INIT(Version, PackageVersion, MainViewModel, box_value(Package::Current().Id().Version()))
} // namespace winrt::TsinghuaNetUWP::implementation
