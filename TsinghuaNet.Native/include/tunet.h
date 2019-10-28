#ifndef TUNET_H
#define TUNET_H

#ifdef __cplusplus
extern "C"
{
#endif // __cplusplus

#include <stdint.h>

    enum net_state
    {
        net_unknown,
        net_net,
        net_auth4,
        net_auth6
    };

    typedef struct net_credential
    {
        const char* username;
        const char* password;
        net_state state;
    } net_credential;

    typedef struct net_flux
    {
        char* username;
        int32_t username_length;
        int64_t flux;
        int64_t online_time;
        double balance;
    } net_flux;

    int32_t tunet_last_err(char* message, int32_t len);
    int32_t tunet_login(net_credential* cred, char* message, int32_t len);
    int32_t tunet_logout(net_credential* cred, char* message, int32_t len);
    int32_t tunet_status(net_credential* cred, net_flux* flux);
    int32_t tunet_usereg_login(net_credential* cred, char* message, int32_t len);
    int32_t tunet_usereg_logout(net_credential* cred, char* message, int32_t len);
    int32_t tunet_usereg_drop(net_credential* cred, int32_t addr, char* message, int32_t len);

#ifdef __cplusplus
}
#endif // __cplusplus

#ifdef __cplusplus

#include <exception>
#include <string>

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

    class helper
    {
    private:
        std::string username;
        std::string password;
        net_state state;

    public:
        helper(std::string username = {}, std::string password = {}, net_state state = net_unknown)
            : username(username), password(password), state(state)
        {
        }

    private:
        void throw_last_error() const
        {
            char message[128];
            std::int32_t len = tunet_last_err(message, sizeof(message) - 1);
            throw tunet_exception(message, len);
        }

    public:
        std::string login() const
        {
            net_credential cred = { username.c_str(), password.c_str(), state };
            char message[128];
            std::int32_t len = tunet_login(&cred, message, sizeof(message) - 1);
            if (len < 0)
                throw_last_error();
            return std::string(message, len);
        }

        std::string logout() const
        {
            net_credential cred = { username.c_str(), password.c_str(), state };
            char message[128];
            std::int32_t len = tunet_logout(&cred, message, sizeof(message) - 1);
            if (len < 0)
                throw_last_error();
            return std::string(message, len);
        }

        std::string usereg_login() const
        {
            net_credential cred = { username.c_str(), password.c_str(), state };
            char message[128];
            std::int32_t len = tunet_usereg_login(&cred, message, sizeof(message) - 1);
            if (len < 0)
                throw_last_error();
            return std::string(message, len);
        }

        std::string usereg_logout() const
        {
            net_credential cred = { username.c_str(), password.c_str(), state };
            char message[128];
            std::int32_t len = tunet_usereg_logout(&cred, message, sizeof(message) - 1);
            if (len < 0)
                throw_last_error();
            return std::string(message, len);
        }
    };
} // namespace tunet

#endif // __cplusplus

#endif // !TUNET_H
