# TsinghuaNetUWP
清华大学校园网客户端，UWP架构，使用C++/WinRT编写。

其内核与我的另一个使用WPF编写的[客户端](https://github.com/Berrysoft/Tsinghua_Auth4_Net)相似，不过使用的是WinRT的库。

在UWP架构下使用C++，速度不一定会有很大提升，因此这个项目仅仅是用来练习。

## 功能特点
* 自动连接，并针对不同的网络类型给出建议。
* 使用Windows凭据管理用户名与密码。

## 注意事项
* 请正常注销Auth连接，否则可能会有短时间难以登录的问题。
* Auth连接的流量显示要慢一点，可能在登录后需要刷新一下。
