#include "pch.h"

#include "ConnectHelper.h"

using namespace winrt;

namespace winrt::TsinghuaNetHelper::implementation
{
    IConnect ConnectHelper::GetHelper(NetState const& state)
    {
        switch (state)
        {
        case NetState::Net:
            return NetHelper();
        case NetState::Auth4:
            return Auth4Helper();
        case NetState::Auth6:
            return Auth6Helper();
        case NetState::Auth4_25:
            return Auth4Helper25();
        case NetState::Auth6_25:
            return Auth6Helper25();
        default:
            return nullptr;
        }
    }

    template <typename T>
    inline T MakeHelper(hstring const& username, hstring const& password)
    {
        T result;
        result.Username(username);
        result.Password(password);
        return result;
    }
    IConnect ConnectHelper::GetHelper(NetState const& state, hstring const& username, hstring const& password)
    {
        switch (state)
        {
        case NetState::Net:
            return MakeHelper<NetHelper>(username, password);
        case NetState::Auth4:
            return MakeHelper<Auth4Helper>(username, password);
        case NetState::Auth6:
            return MakeHelper<Auth6Helper>(username, password);
        case NetState::Auth4_25:
            return MakeHelper<Auth4Helper25>(username, password);
        case NetState::Auth6_25:
            return MakeHelper<Auth6Helper25>(username, password);
        default:
            return nullptr;
        }
    }
} // namespace winrt::TsinghuaNetHelper::implementation
