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

    int tunet_login(net_credential* cred, char* message, int len);
    int tunet_logout(net_credential* cred, char* message, int len);
    int tunet_status(net_credential* cred, net_flux* flux);
    int tunet_usereg_login(net_credential* cred, char* message, int len);
    int tunet_usereg_logout(net_credential* cred, char* message, int len);
    int tunet_usereg_drop(net_credential* cred, int32_t addr, char* message, int len);

#ifdef __cplusplus
}
#endif // __cplusplus

#endif // !TUNET_H
