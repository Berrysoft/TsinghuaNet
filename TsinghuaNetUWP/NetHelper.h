#pragma once
#include <map>
#include <ppltasks.h>
#include <vector>

namespace winrt::TsinghuaNetUWP
{
    struct FluxUser
    {
        std::wstring username;
        std::uint64_t flux;
        Windows::Foundation::TimeSpan online_time;
        double balance;
    };

    FluxUser make_FluxUser(std::wstring_view const& fluxstr);

    struct IConnect
    {
        virtual ~IConnect() {}
        virtual concurrency::task<winrt::hstring> LoginAsync() const = 0;
        virtual concurrency::task<winrt::hstring> LogoutAsync() const = 0;
        virtual concurrency::task<FluxUser> FluxAsync() const = 0;
    };

    class NetHelper_base
    {
    private:
        Windows::Web::Http::HttpClient client;

    protected:
        winrt::hstring username;
        winrt::hstring password;

    public:
        virtual ~NetHelper_base() {}
        winrt::hstring Username() const { return username; }
        void Username(winrt::hstring const& s) { username = s; }
        winrt::hstring Password() const { return password; }
        void Password(winrt::hstring const& s) { password = s; }

    protected:
        concurrency::task<winrt::hstring> GetAsync(Windows::Foundation::Uri const& uri) const;
        concurrency::task<Windows::Storage::Streams::IBuffer> GetBytesAsync(Windows::Foundation::Uri const& uri) const;
        concurrency::task<winrt::hstring> PostAsync(Windows::Foundation::Uri const& uri) const;
        concurrency::task<winrt::hstring> PostAsync(Windows::Foundation::Uri const& uri, winrt::param::hstring const& data) const;
        concurrency::task<winrt::hstring> PostAsync(Windows::Foundation::Uri const& uri, std::map<winrt::hstring, winrt::hstring> const& data) const;
    };

    class NetHelper : public NetHelper_base, public IConnect
    {
    public:
        ~NetHelper() override {}

        concurrency::task<winrt::hstring> LoginAsync() const override;
        concurrency::task<winrt::hstring> LogoutAsync() const override;
        concurrency::task<FluxUser> FluxAsync() const override;

    private:
        static constexpr wchar_t LogUri[] = L"http://net.tsinghua.edu.cn/do_login.php";
        static constexpr wchar_t FluxUri[] = L"http://net.tsinghua.edu.cn/rad_user_info.php";
        static constexpr wchar_t LoginData[] = L"action=login&ac_id=1&username={}&password={{MD5_HEX}}{}";
        static constexpr wchar_t LogoutData[] = L"action=logout";
    };

    class AuthHelper : public NetHelper_base, public IConnect
    {
    public:
        virtual ~AuthHelper() override {}

        concurrency::task<winrt::hstring> LoginAsync() const override;
        concurrency::task<winrt::hstring> LogoutAsync() const override;
        concurrency::task<FluxUser> FluxAsync() const override;

    private:
        static constexpr wchar_t LogUriBase[] = L"https://auth{}.tsinghua.edu.cn/cgi-bin/srun_portal";
        static constexpr wchar_t FluxUriBase[] = L"https://auth{}.tsinghua.edu.cn/rad_user_info.php";
        static constexpr wchar_t ChallengeUriBase[] = L"https://auth{}.tsinghua.edu.cn/cgi-bin/get_challenge?username={{}}&double_stack=1&ip&callback=callback";
        static constexpr wchar_t LogoutData[] = L"action=logout";

        std::wstring const LogUri;
        std::wstring const FluxUri;
        std::wstring const ChallengeUri;

    protected:
        AuthHelper(int version);

    private:
        concurrency::task<std::string> ChallengeAsync() const;

        concurrency::task<std::map<winrt::hstring, winrt::hstring>> LoginDataAsync() const;
    };

    class Auth4Helper : public AuthHelper
    {
    public:
        Auth4Helper() : AuthHelper(4) {}
        ~Auth4Helper() override {}
    };

    class Auth6Helper : public AuthHelper
    {
    public:
        Auth6Helper() : AuthHelper(6) {}
        ~Auth6Helper() override {}
    };

    struct NetUser
    {
        std::wstring address;
        std::wstring login_time;
        std::wstring client;
    };

    class UseregHelper : public NetHelper_base
    {
    public:
        ~UseregHelper() override {}

        concurrency::task<winrt::hstring> LoginAsync() const;
        concurrency::task<winrt::hstring> LogoutAsync() const;
        concurrency::task<winrt::hstring> LogoutAsync(std::wstring const& ip) const;
        concurrency::task<std::vector<NetUser>> UsersAsync() const;

    private:
        static constexpr wchar_t LogUri[] = L"https://usereg.tsinghua.edu.cn/do.php";
        static constexpr wchar_t InfoUri[] = L"https://usereg.tsinghua.edu.cn/online_user_ipv4.php";
        static constexpr wchar_t LoginData[] = L"action=login&user_login_name={}&user_password={}";
        static constexpr wchar_t LogoutData[] = L"action=logout";
        static constexpr wchar_t DropData[] = L"action=drop&user_ip={}";
    };
} // namespace winrt::TsinghuaNetUWP
