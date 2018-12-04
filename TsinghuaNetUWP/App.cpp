#include "pch.h"

#include "App.h"

#include "MainPage.h"

using namespace winrt;
using namespace Windows::ApplicationModel;
using namespace Windows::ApplicationModel::Activation;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Navigation;
using namespace TsinghuaNetUWP;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{

    /// <summary>
    /// 初始化单一实例应用程序对象。
    /// 这是执行的创作代码的第一行，
    /// 逻辑上等同于 main() 或 WinMain()。
    /// </summary>
    App::App()
    {
        InitializeComponent();
        Suspending({ this, &App::OnSuspending });

#if defined _DEBUG && !defined DISABLE_XAML_GENERATED_BREAK_ON_UNHANDLED_EXCEPTION
        UnhandledException([this](IInspectable const&, UnhandledExceptionEventArgs const& e) {
            if (IsDebuggerPresent())
            {
                auto errorMessage = e.Message();
                __debugbreak();
            }
        });
#endif
    }

    /// <summary>
    /// 在应用程序由最终用户正常启动时进行调用。
    /// 其它入口点将在启动应用程序以打开特定文件等情况下使用。
    /// </summary>
    /// <param name="e">有关启动请求和过程的详细信息。</param>
    void App::OnLaunched(LaunchActivatedEventArgs const& e)
    {
        Frame rootFrame{ nullptr };
        auto content = Window::Current().Content();
        if (content)
        {
            rootFrame = content.try_as<Frame>();
        }

        // 不要在窗口已包含内容时重复应用程序初始化，
        // 只需确保窗口处于活动状态
        if (!rootFrame)
        {
            // 创建要充当导航上下文的框架，并导航到第一页
            rootFrame = Frame();

            rootFrame.NavigationFailed({ this, &App::OnNavigationFailed });

            if (e.PreviousExecutionState() == ApplicationExecutionState::Terminated)
            {
                //TODO: 从之前挂起的应用程序加载状态
            }

            // 将框架放在当前窗口中
            Window::Current().Content(rootFrame);
        }

        if (!e.PrelaunchActivated())
        {
            if (!rootFrame.Content())
            {
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置参数
                rootFrame.Navigate(xaml_typename<TsinghuaNetUWP::MainPage>(), box_value(e.Arguments()));
            }
            // 确保当前窗口处于活动状态
            Window::Current().Activate();
        }
    }

    /// <summary>
    /// 在应用程序通过正常启动之外的其他方法激活时调用。
    /// </summary>
    /// <param name="e">事件的事件数据。</param>
    void App::OnActivated(IActivatedEventArgs const& e)
    {
        Frame rootFrame = Window::Current().Content().try_as<Frame>();
        if (!rootFrame)
        {
            rootFrame = Frame();
            Window::Current().Content(rootFrame);
        }

        if (!rootFrame.Content())
        {
            rootFrame.Navigate(xaml_typename<TsinghuaNetUWP::MainPage>(), box_value(e.Kind()));
        }
        TsinghuaNetUWP::MainPage mainPage = rootFrame.Content().try_as<TsinghuaNetUWP::MainPage>();

        MainPage* pmain = get_self<MainPage>(mainPage);
        if (e.Kind() == ActivationKind::ToastNotification)
        {
            pmain->ToastLogined(true);
        }

        Window::Current().Activate();
    }

    /// <summary>
    /// 在将要挂起应用程序执行时调用。
    /// 保存应用状态，无需知道应用程序会被终止还是会恢复，
    /// 并让内存内容保持不变。
    /// </summary>
    /// <param name="sender">挂起的请求的源。</param>
    /// <param name="e">有关挂起请求的详细信息。</param>
    void App::OnSuspending(IInspectable const&, SuspendingEventArgs const& e)
    {
        auto deferral = e.SuspendingOperation().GetDeferral();
        // TODO: 保存应用程序状态并停止任何后台活动
        deferral.Complete();
    }

    /// <summary>
    /// 导航到特定页失败时调用
    /// </summary>
    /// <param name="sender">导航失败的框架</param>
    /// <param name="e">有关导航失败的详细信息</param>
    void App::OnNavigationFailed(IInspectable const&, NavigationFailedEventArgs const& e)
    {
        throw hresult_error(E_FAIL, hstring(L"Failed to load Page ") + e.SourcePageType().Name);
    }
} // namespace winrt::TsinghuaNetUWP::implementation
