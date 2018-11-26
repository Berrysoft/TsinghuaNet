#include "pch.h"

#include "FluxUser.h"
#include <chrono>
#include <string_view>
#include <vector>

using namespace std;
using namespace std::chrono;
using namespace winrt;

namespace winrt::TsinghuaNetHelper::implementation
{
    vector<wstring_view> string_split(wstring_view const& s, wchar_t separator)
    {
        vector<wstring_view> result;
        size_t offset = 0, index = 0;
        while ((index = s.find(separator, offset)) != wstring_view::npos)
        {
            if (index > offset)
            {
                result.push_back(s.substr(offset, index - offset));
            }
            offset = index + 1;
        }
        if (offset + 1 < s.length())
        {
            result.push_back(s.substr(offset));
        }
        return result;
    }

    TsinghuaNetHelper::FluxUser FluxUser::Parse(hstring const& fluxstr)
    {
        auto r = string_split(fluxstr, L',');
        auto result = make<FluxUser>();
        if (!r.empty())
        {
            result.Username(hstring(r[0]));
            result.Flux(stoull(wstring(r[6])));
            result.OnlineTime(seconds(stoll(wstring(r[2])) - stoll(wstring(r[1]))));
            result.Balance(stod(wstring(r[10])));
        }
        return result;
    }
} // namespace winrt::TsinghuaNetHelper::implementation
