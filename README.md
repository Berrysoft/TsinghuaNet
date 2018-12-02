# TsinghuaNetUWP
清华大学校园网客户端，UWP架构，使用C++/WinRT编写。

其内核与我的另一个使用WPF编写的[客户端](https://github.com/Berrysoft/Tsinghua_Auth4_Net)相似，不过使用的是WinRT的库。
为了满足认证要求，采用组合而非继承的架构（因为JavaScript不支持继承）。

## 功能特点
* 针对不同的网络类型给出建议。
* 使用Windows凭据管理用户名与密码。
* 在网络状态改变时会自动判断，后台连接。
* 后台刷新流量。

## 注意事项
* 请正常注销Auth连接，否则可能会有短时间难以登录的问题。
* Auth连接的流量显示要慢一点，可能在登录后需要刷新一下。
