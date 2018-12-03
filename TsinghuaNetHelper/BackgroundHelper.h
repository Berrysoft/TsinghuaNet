#pragma once
#include "BackgroundHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct BackgroundHelper
    {
        BackgroundHelper() = delete;

        static Windows::Foundation::IAsyncOperation<bool> RequestAccessAsync();
        static void UnregisterLiveTile();
        static void UnregisterLogin();
        static void RegisterLiveTile();
        static void RegisterLogin();
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct BackgroundHelper : BackgroundHelperT<BackgroundHelper, implementation::BackgroundHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation
