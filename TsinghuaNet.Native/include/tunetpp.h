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
        tunet_credential cred;

    public:
        helper(const std::string& username = {}, const std::string& password = {}, tunet_state state = tunet_unknown)
            : username(username), password(password)
        {
            cred = { username.c_str(), password.c_str(), state };
        }

    private:
        tunet_exception last_error() const
        {
            char message[256];
            std::int32_t len = tunet_last_err(message, sizeof(message));
            return tunet_exception(message, len);
        }

        template <typename F, typename... Args>
        void invoke(F&& f, Args&&... args) const
        {
            std::int32_t len = f(&cred, std::forward<Args>(args)...);
            if (len < 0)
                throw last_error();
        }

    public:
        void login() const { invoke(tunet_login); }

        void logout() const { invoke(tunet_logout); }

        flux status() const
        {
            tunet_flux f;
            std::int32_t len = tunet_status(&cred, &f);
            return { std::string(f.username, len), f.flux, std::chrono::seconds(f.online_time), f.balance };
        }

        void usereg_login() const { invoke(tunet_usereg_login); }

        void usereg_logout() const { invoke(tunet_usereg_logout); }

        void usereg_drop(std::int64_t addr) const { invoke(tunet_usereg_drop, addr); }

#ifdef USE_INET_ADDR

        void usereg_drop(const char* addr) const
        {
            usereg_drop(::inet_addr(addr));
        }

#endif // USE_INET_ADDR
    };
} // namespace tunet

#endif // !TUNETPP_H
