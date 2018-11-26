#pragma once
#include "NotificationHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NotificationHelper : NotificationHelperT<NotificationHelper>
    {
        NotificationHelper() = default;

        void UpdateTile(TsinghuaNetHelper::FluxUser const& user);
        void SendToast(TsinghuaNetHelper::FluxUser const& user);

        static Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::NotificationHelper> LoadAsync();

    private:
        std::wstring tile_t;
        std::wstring toast_t;
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NotificationHelper : NotificationHelperT<NotificationHelper, implementation::NotificationHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation
