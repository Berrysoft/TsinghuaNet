#include "pch.h"

#include "SettingsHelper.h"

using namespace winrt;
using namespace Windows::Storage;

namespace winrt::TsinghuaNetHelper::implementation
{
    template <typename T>
    T GetValue(hstring const& key)
    {
        auto settings = ApplicationData::Current().LocalSettings();
        auto values = settings.Values();
        auto value = values.TryLookup(key);
        if (value != nullptr)
        {
            return unbox_value<T>(value);
        }
        return {};
    }

    template <typename T>
    void SetValue(hstring const& key, T value)
    {
        auto settings = ApplicationData::Current().LocalSettings();
        auto values = settings.Values();
        values.Insert(key, box_value(value));
    }

    constexpr wchar_t StoredUsernameKey[] = L"Username";

    hstring SettingsHelper::StoredUsername()
    {
        return GetValue<hstring>(StoredUsernameKey);
    }

    void SettingsHelper::StoredUsername(hstring const& value)
    {
        SetValue(StoredUsernameKey, value);
    }

    constexpr wchar_t AutoLoginKey[] = L"AutoLogin";

    bool SettingsHelper::AutoLogin()
    {
        return GetValue<bool>(AutoLoginKey);
    }

    void SettingsHelper::AutoLogin(bool value)
    {
        SetValue(AutoLoginKey, value);
    }
} // namespace winrt::TsinghuaNetHelper::implementation
