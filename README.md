# TsinghuaNetUWP
清华大学校园网客户端，UWP架构，使用C++/WinRT编写。

其内核与我的另一个使用WPF编写的[客户端](https://github.com/Berrysoft/Tsinghua_Auth4_Net)相似，不过使用了WinRT重写，而且支持的功能更多。

## 程序截图
|![主界面](./Screenshots/MainPage.png)|![设置栏](./Screenshots/Settings.png)|![适应主题](./Screenshots/Theme.png)|
|:-:|:-:|:-:|
|主界面|设置栏|适应主题|

## 功能特点
* 针对不同的网络类型给出建议。
* 使用Windows凭据管理用户名与密码。
* 在网络状态改变时会自动判断，后台连接。
* 后台刷新流量。
* 第一个支持Tsinghua-IPv4与Tsinghua-IPv6无线网。

## 注意事项
* 如果使用“管理连接”注销一个Auth连接，可能需要在主界面再注销一次才能再次登录。
* Auth连接的流量显示要慢一点，可能在登录后需要手动刷新一下。
* 上面的两个问题，详情请咨询[深澜软件](http://www.srun.com/)。

## 编译
源代码编译需要已经为Visual Studio安装“通用 Windows 平台开发”以及其中的“C++ 通用 Windows 平台工具”，并需要Windows SDK 10.0.17763。

此外，还需要我自己的格式化库[StreamFormat](https://github.com/Berrysoft/StreamFormat)。复制头文件即可。

最后，面对C++的编译速度，请保持耐心。
