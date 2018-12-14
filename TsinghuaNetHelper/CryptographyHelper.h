#pragma once

namespace winrt::TsinghuaNetHelper
{
    hstring GetMD5(hstring const& input);
    hstring GetSHA1(hstring const& input);
    std::string XEncode(std::string const& str, std::string const& key);
    std::string Base64Encode(std::string const& t);
    hstring GetHMACMD5(std::string const& key);
} // namespace winrt::TsinghuaNetHelper
