#ifndef TUNET_H
#define TUNET_H

#ifndef TUNET_API
#ifdef _MSC_VER
#define TUNET_API __cdecl
#else
#define TUNET_API
#endif
#endif // !TUNET_API

#ifndef TUNET_RESTRICT
#if defined(_MSC_VER)
#define TUNET_RESTRICT __restrict
#elif defined(__GNUC__)
#define TUNET_RESTRICT __restrict__
#else
#define TUNET_RESTRICT
#endif
#endif // !TUNET_RESTRICT

#ifdef __cplusplus
extern "C"
{
#endif // __cplusplus

#include <stdint.h>

    enum tunet_state
    {
        tunet_unknown,
        tunet_net,
        tunet_auth4,
        tunet_auth6
    };

    typedef struct tunet_credential
    {
        const char* username;
        const char* password;
        tunet_state state;
        int32_t use_proxy;
    } tunet_credential;

    typedef struct tunet_flux
    {
        char* username;
        int32_t username_length;
        int64_t flux;
        int64_t online_time;
        double balance;
    } tunet_flux;

    typedef struct tunet_user
    {
        int64_t address;
        int64_t login_time;
        char* client;
        int32_t client_length;
    } tunet_user;

    typedef struct tunet_detail
    {
        int64_t login_time;
        int64_t logout_time;
        int64_t flux;
    } tunet_detail;

    enum tunet_detail_order
    {
        tunet_detail_login_time,
        tunet_detail_logout_time,
        tunet_detail_flux
    };

    int32_t TUNET_API tunet_last_err(char* message, int32_t len);

    int32_t TUNET_API tunet_login(const tunet_credential* cred);
    int32_t TUNET_API tunet_logout(const tunet_credential* cred);
    int32_t TUNET_API tunet_status(const tunet_credential* TUNET_RESTRICT cred, tunet_flux* TUNET_RESTRICT flux);

    int32_t TUNET_API tunet_usereg_login(const tunet_credential* cred);
    int32_t TUNET_API tunet_usereg_logout(const tunet_credential* cred);
    int32_t TUNET_API tunet_usereg_drop(const tunet_credential* cred, int64_t addr);

    int32_t TUNET_API tunet_usereg_users(const tunet_credential* cred);
    int32_t TUNET_API tunet_usereg_users_destory(void);
    int32_t TUNET_API tunet_usereg_users_fetch(int32_t index, tunet_user* user);

    int32_t TUNET_API tunet_usereg_details(const tunet_credential* cred, tunet_detail_order order, int descending);
    int32_t TUNET_API tunet_usereg_details_destory(void);
    int32_t TUNET_API tunet_usereg_details_fetch(int32_t index, tunet_detail* detail);

#ifdef __cplusplus
}
#endif // __cplusplus

#endif // !TUNET_H
