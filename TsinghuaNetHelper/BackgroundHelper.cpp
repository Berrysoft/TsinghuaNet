#include "pch.h"

#include "BackgroundHelper.h"
#include <winrt/Windows.ApplicationModel.Background.h>
#include <winrt/Windows.UI.Xaml.Interop.h>

using namespace winrt;
using namespace Windows::ApplicationModel::Background;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;

namespace winrt::TsinghuaNetHelper::implementation
{
    IAsyncOperation<bool> BackgroundHelper::RequestAccessAsync()
    {
        auto status = co_await BackgroundExecutionManager::RequestAccessAsync();
        if (status == BackgroundAccessStatus::Unspecified ||
            status == BackgroundAccessStatus::DeniedByUser ||
            status == BackgroundAccessStatus::DeniedBySystemPolicy)
        {
            co_return false;
        }
        co_return true;
    }

    void UnregisterTask(hstring const& name)
    {
        for (auto t : BackgroundTaskRegistration::AllTasks())
        {
            auto registration = t.Value();
            if (registration.Name() == name)
            {
                registration.Unregister(true);
            }
        }
    }

    BackgroundTaskBuilder NewTask(hstring const& name, TypeName const& type)
    {
        BackgroundTaskBuilder task;
        task.Name(name);
        task.TaskEntryPoint(type.Name);
        return task;
    }

    constexpr wchar_t LIVETILETASK[] = L"LIVETILETASK";
    constexpr wchar_t LOGINTASK[] = L"LOGINTASK";

    void BackgroundHelper::RegisterLiveTile(bool reg)
    {
        UnregisterTask(LIVETILETASK);
        if (reg)
        {
            auto task = NewTask(LIVETILETASK, xaml_typename<LiveTileTask>());
            task.SetTrigger(TimeTrigger(15, false));
            task.AddCondition(SystemCondition(SystemConditionType::InternetAvailable));
            task.Register();
        }
    }

    void BackgroundHelper::RegisterLogin(bool reg)
    {
        UnregisterTask(LOGINTASK);
        if (reg)
        {
            auto task = NewTask(LOGINTASK, xaml_typename<LoginTask>());
            task.SetTrigger(SystemTrigger(SystemTriggerType::NetworkStateChange, false));
            task.AddCondition(SystemCondition(SystemConditionType::InternetNotAvailable));
            task.Register();
        }
    }
} // namespace winrt::TsinghuaNetHelper::implementation
