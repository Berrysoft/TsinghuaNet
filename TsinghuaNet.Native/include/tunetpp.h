#ifndef TUNETPP_H
#define TUNETPP_H

#include "tunet.h"
#include <chrono>
#include <exception>
#include <string>

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

    class helper
    {
    private:
        std::string username;
        std::string password;
        net_credential cred;

    public:
        helper(const std::string& username = {}, const std::string& password = {}, net_state state = net_unknown)
            : username(username), password(password)
        {
            cred = { username.c_str(), password.c_str(), state };
        }

    private:
        tunet_exception last_error() const
        {
            char message[128];
            std::int32_t len = tunet_last_err(message, sizeof(message) - 1);
            return tunet_exception(message, len);
        }

        template <typename F, typename... Args>
        std::string invoke(F&& f, Args&&... args) const
        {
            char message[128];
            std::int32_t len = f(&cred, std::forward<Args>(args)..., message, sizeof(message) - 1);
            if (len < 0)
                throw last_error();
            return std::string(message, len);
        }

    public:
        std::string login() const { return invoke(tunet_login); }

        std::string logout() const { return invoke(tunet_logout); }

        flux status() const
        {
            net_flux f;
            std::int32_t len = tunet_status(&cred, &f);
            return { std::string(f.username, len), f.flux, std::chrono::seconds(f.online_time), f.balance };
        }

        std::string usereg_login() const { return invoke(tunet_usereg_login); }

        std::string usereg_logout() const { return invoke(tunet_usereg_logout); }

        std::string usereg_drop(std::int64_t addr) const { return invoke(tunet_usereg_drop, addr); }

#ifdef USE_INET_ADDR

        std::string usereg_drop(const char* addr) const
        {
            return usereg_drop(::inet_addr(addr));
        }

#endif // USE_INET_ADDR
    };
} // namespace tunet

#endif // !TUNETPP_H
