#pragma once

namespace winrt::TsinghuaNetUWP
{
    winrt::hstring GetCredential(winrt::hstring const& username);
    void SaveCredential(winrt::hstring const& username, winrt::hstring const& password);
    void RemoveCredential(winrt::hstring const& username);
    winrt::hstring StoredUsername();
    void StoredUsername(winrt::hstring const& value);
} // namespace winrt::TsinghuaNetUWP
