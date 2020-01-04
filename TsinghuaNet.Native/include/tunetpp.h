#ifndef TUNETPP_H
#define TUNETPP_H

#include "tunet.h"
#include <chrono>
#include <exception>
#include <string>
#include <vector>

#ifdef USE_INET_ADDR
#ifdef _WIN32
#include <WinSock2.h>
#else
#include <arpa/inet.h>
#endif // _WIN32
#endif // USE_INET_ADDR

namespace tunet
{
    struct tunet_exception : std::exception
    {
    private:
        std::string message;

    public:
        tunet_exception(const char* message, int32_t len) noexcept : message(message, len) {}

        const char* what() const noexcept override { return message.c_str(); }
    };

    struct flux
    {
        std::string username;
        std::int64_t bytes;
        std::chrono::seconds online_time;
        double balance;
    };

    struct user
    {
        std::int64_t address;
        std::chrono::system_clock::time_point login_time;
        std::uint8_t mac_address[6];
    };

    struct detail
    {
        std::chrono::system_clock::time_point login_time;
        std::chrono::system_clock::time_point logout_time;
        std::int64_t bytes;
    };

    class helper
    {
    private:
        std::string username;
        std::string password;
        tunet_credential cred;

    public:
        helper(const std::string& username = {}, const std::string& password = {}, tunet_state state = tunet_unknown, bool proxy = false)
            : username(username), password(password)
        {
            cred = { this->username.c_str(), this->password.c_str(), state, proxy ? 1 : 0 };
        }

    private:
        tunet_exception last_error() const
        {
            char message[256];
            std::int32_t len = tunet_last_err(message, sizeof(message));
            return tunet_exception(message, len);
        }

        template <typename F, typename... Args>
        std::int32_t invoke(F&& f, Args&&... args) const
        {
            std::int32_t len = f(std::forward<Args>(args)...);
            if (len < 0)
                throw last_error();
            return len;
        }

    public:
        void login() const { invoke(tunet_login, &cred); }

        void logout() const { invoke(tunet_logout, &cred); }

        flux status() const
        {
            tunet_flux f = {};
            char username[32];
            f.username = username;
            f.username_length = sizeof(username);
            std::int32_t len = invoke(tunet_status, &cred, &f);
            return { std::string(f.username, len), f.flux, std::chrono::seconds(f.online_time), f.balance };
        }

        void usereg_login() const { invoke(tunet_usereg_login, &cred); }

        void usereg_logout() const { invoke(tunet_usereg_logout, &cred); }

        void usereg_drop(std::int64_t addr) const { invoke(tunet_usereg_drop, &cred, addr); }

#ifdef USE_INET_ADDR

        void usereg_drop(const char* addr) const
        {
            usereg_drop(::inet_addr(addr));
        }

#endif // USE_INET_ADDR

    private:
        std::vector<user> users_cache;

        static int usereg_users_callback(const tunet_user* user, void* data)
        {
            helper* ph = (helper*)data;
            tunet::user u = { user->address, std::chrono::system_clock::time_point(std::chrono::seconds(user->login_time)) };
            std::copy(std::begin(user->mac_address), std::end(user->mac_address), u.mac_address);
            ph->users_cache.push_back(std::move(u));
            return 1;
        }

    public:
        std::vector<user> usereg_users() const
        {
            invoke(tunet_usereg_users, &cred, &helper::usereg_users_callback, this);
            return std::move(users_cache);
        }

    private:
        std::vector<detail> details_cache;

        static int usereg_details_callback(const tunet_detail* detail, void* data)
        {
            helper* ph = (helper*)data;
            ph->details_cache.push_back({ std::chrono::system_clock::time_point(std::chrono::seconds(detail->login_time)), std::chrono::system_clock::time_point(std::chrono::seconds(detail->logout_time)), detail->flux });
            return 1;
        }

    public:
        std::vector<detail> usereg_details(tunet_detail_order order = tunet_detail_logout_time, bool descending = false) const
        {
            invoke(tunet_usereg_details, &cred, order, descending ? 1 : 0, &helper::usereg_details_callback, this);
            return std::move(details_cache);
        }
    };
} // namespace tunet

#endif // !TUNETPP_H
