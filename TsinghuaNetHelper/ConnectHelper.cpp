#include "pch.h"

#include "ConnectHelper.h"

using namespace std;
using namespace winrt;

namespace winrt::TsinghuaNetHelper::implementation
{
    template <typename... Args>
    IConnect GetHelperT(NetState const& state, Args&&... args)
    {
        switch (state)
        {
        case NetState::Net:
            return NetHelper(forward<Args>(args)...);
        case NetState::Auth4:
            return Auth4Helper(forward<Args>(args)...);
        case NetState::Auth6:
            return Auth6Helper(forward<Args>(args)...);
        default:
            return nullptr;
        }
    }

    IConnect ConnectHelper::GetHelper(NetState const& state)
    {
        return GetHelperT(state);
    }

    IConnect ConnectHelper::GetHelper(NetState const& state, hstring const& username, hstring const& password)
    {
        return GetHelperT(state, username, password);
    }
} // namespace winrt::TsinghuaNetHelper::implementation
