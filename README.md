# TsinghuaNet
清华大学校园网联网库与客户端。

## TsinghuaNet
联网库，和一些其他项目需要的辅助类。使用C#，支持 .NET Standard 2.0。

## TsinghuaNet.CLI
命令行版本，使用Visual Basic，支持 .NET Core 3.0，并有针对Windows x64与Linux x64的AOT版本。
### 登录/注销
``` bash
# 使用默认（net）方式登录
TsinghuaNet.CLI login -u 用户名 -p 密码
# 使用默认（net）方式注销，不需要用户名密码
TsinghuaNet.CLI logout
# 使用auth4方式登录
TsinghuaNet.CLI login -s auth4 -u 用户名 -p 密码
# 使用auth4方式注销，需要用户名密码
TsinghuaNet.CLI logout -s auth4 -u 用户名 -p 密码
```
### 在线状态
``` bash
# 使用默认（net）方式
TsinghuaNet.CLI status
# 使用auth4方式
TsinghuaNet.CLI status -s auth4
```
### 查询/强制下线在线IP
``` bash
# 查询
TsinghuaNet.CLI online -u 用户名 -p 密码
# 强制下线
TsinghuaNet.CLI drop -a IP地址 -u 用户名 -p 密码
```
### 流量明细
``` bash
# 使用默认排序（注销时间，升序）查询明细
TsinghuaNet.CLI detail -u 用户名 -p 密码
# 使用登录时间（升序）查询明细
TsinghuaNet.CLI detail -o login -u 用户名 -p 密码
# 使用流量降序查询明细
TsinghuaNet.CLI detail -o flux -d -u 用户名 -p 密码
# 使用流量降序查询明细，并按注销日期组合
TsinghuaNet.CLI detail -o flux -dg -u 用户名 -p 密码
```

## TsinghuaNet.UWP
UWP版本，3.0以前的版本使用C++/WinRT，之后的版本使用Visual Basic。

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

相关问答见[此处](./README.UWP.md)。

## TsinghuaNet.Avalonia
Avalonia版本，使用Visual Basic，支持 .NET Core 3.0，并有针对Windows x64的AOT版本。

仍在开发中，将会至少支持**TsinghuaNet.CLI**的所有功能，并至少再提供Linux x64的AOT。
