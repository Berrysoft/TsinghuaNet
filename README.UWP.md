# TsinghuaNet.UWP

<table>
    <tbody>
        <tr>
            <th><img alt="主界面" src="./Screenshots/MainPage.png"/></th>
            <th><img alt="设置栏" src="./Screenshots/Settings.png"/></th>
        </tr>
        <tr>
            <th><img alt="适应主题" src="./Screenshots/Theme.png"/></th>
            <th><img alt="编辑建议" src="./Screenshots/Suggestions.png"/></th>
        </tr>
        <tr>
            <th><img alt="直线" src="./Screenshots/Line.png"/></th>
            <th><img alt="图标" src="./Screenshots/Circle.png"/></th>
        </tr>
    </tbody>
</table>

# Q&A
## 为什么需要一个客户端？
为了连接校园网，我们通常需要登录特定的网址，输入用户名和密码，并点击登录。这很麻烦，且由于各种技术原因容易出现误操作。而使用客户端，可以自动完成这些操作，节约时间。
## 本客户端有什么优势？
* 速度快。
* 针对不同的网络类型给出建议。
* 查看本月流量明细。
* 在网络状态改变时会自动判断，后台连接。
* 后台刷新流量，更新磁贴。
* 第一个支持Tsinghua-IPv4无线网。
## 如何确保我的信息安全？
* 用户名和密码使用[Windows凭据管理器](https://support.microsoft.com/zh-cn/help/4026814/windows-accessing-credential-manager)管理。
* 其他涉及隐私的信息，如在线用户、上网明细，即时从相应网站获得，不存储。
* 登录时使用了相应的加密算法。
* 所有代码开源，欢迎审核。
## 后台任务会不会拖慢系统速度？
不会。
## 从哪里下载最新版的程序？
在[Releases](https://github.com/Berrysoft/TsinghuaNetUWP/releases)页面。
## 我该如何从源代码编译这个程序？
为Visual Studio安装“通用 Windows 平台开发”，并需要Windows SDK 10.0.18362。

使用的[Berrysoft.Tsinghua.Net](https://github.com/Berrysoft/ClassLibrary/tree/master/src/Berrysoft.Tsinghua.Net)可能需要自行打包成NuGet包，因为那里的Releases不太及时。
## 我可以使用这里的源代码吗？
可以。这个项目使用[MIT许可证](./LICENSE)开源。
## 为什么要改变语言？
因为适用于C++/WinRT的第三方库太少了，而且这个架构使用起来太麻烦。

而用Visual Basic写起来则得心应手。得益于 .NET Native，得到的程序速度不亚于C++。
## 为什么无法登录Tsinghua-IPv6无线网？
这个技术问题还没有完全解决，欢迎你的帮助。
## 这个程序的颜色在我的电脑上和你的电脑上不同！
因为用的是主题色。
## 我对这个程序有一些建议……
欢迎[Issues](https://github.com/Berrysoft/TsinghuaNetUWP/issues)和[Pull requests](https://github.com/Berrysoft/TsinghuaNetUWP/pulls)。
## 这个程序能卸载干净吗？
卸载时，除了凭据，所有个人配置都会被删除。凭据仅在选中“保存密码”时保存。
## 大佬太厉害了！
反弹！您才是大佬！
