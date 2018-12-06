#pragma once
#include "BackgroundHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct BackgroundHelper
    {
        BackgroundHelper() = delete;

        static Windows::Foundation::IAsyncOperation<bool> RequestAccessAsync();
        static void RegisterLiveTile(bool reg);
        static void RegisterLogin(bool reg);
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct BackgroundHelper : BackgroundHelperT<BackgroundHelper, implementation::BackgroundHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation
