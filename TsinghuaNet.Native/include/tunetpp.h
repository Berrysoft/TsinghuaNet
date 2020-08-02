#ifndef TUNETPP_H
#define TUNETPP_H

#include "tunet.h"
#include <chrono>
#include <exception>
#include <memory>
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
        tunet_exception(const char* message) noexcept : message(message) {}

        const char* what() const noexcept override { return message.c_str(); }
    };

    struct string_deleter
    {
        void operator()(void* p) const noexcept
        {
            tunet_string_free((const char*)p);
        }
    };

    using unique_string = std::unique_ptr<char, string_deleter>;

    struct flux
    {
        unique_string username;
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
            unique_string message{ tunet_last_err() };
            throw tunet_exception(message.get());
        }

        template <typename F, typename... Args>
        void invoke(F&& f, Args&&... args) const
        {
            std::int32_t res = f(std::forward<Args>(args)...);
            if (res < 0)
                throw last_error();
        }

    public:
        void login() const { invoke(tunet_login, &cred); }

        void logout() const { invoke(tunet_logout, &cred); }

        flux status() const
        {
            tunet_flux f;
            invoke(tunet_status, &cred, &f);
            return flux{ unique_string{ f.username }, f.flux, std::chrono::seconds(f.online_time), f.balance };
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
        static int usereg_users_callback(const tunet_user* pu, void* data)
        {
            std::vector<user>* ph = (std::vector<user>*)data;
            tunet::user u = { pu->address, std::chrono::system_clock::time_point(std::chrono::seconds(pu->login_time)) };
            std::copy(std::begin(pu->mac_address), std::end(pu->mac_address), u.mac_address);
            ph->push_back(std::move(u));
            return 1;
        }

    public:
        std::vector<user> usereg_users() const
        {
            std::vector<user> users;
            invoke(tunet_usereg_users, &cred, &helper::usereg_users_callback, &users);
            return users;
        }

    private:
        static int usereg_details_callback(const tunet_detail* pd, void* data)
        {
            std::vector<detail>* ph = (std::vector<detail>*)data;
            ph->push_back({ std::chrono::system_clock::time_point(std::chrono::seconds(pd->login_time)), std::chrono::system_clock::time_point(std::chrono::seconds(pd->logout_time)), pd->flux });
            return 1;
        }

    public:
        std::vector<detail> usereg_details(tunet_detail_order order = tunet_detail_logout_time, bool descending = false) const
        {
            std::vector<detail> details;
            invoke(tunet_usereg_details, &cred, order, descending ? 1 : 0, &helper::usereg_details_callback, &details);
            return details;
        }
    };
} // namespace tunet

#endif // !TUNETPP_H
