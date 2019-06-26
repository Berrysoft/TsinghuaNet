Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports TsinghuaNet.Helper
Imports TsinghuaNet.Model

Module VerbHelper
    Public Status As New NetPingStatus

    <Extension>
    Public Async Function GetHelperAsync(opts As NetVerbBase) As Task(Of IConnect)
        If opts.Host = OptionNetState.Auto Then
            opts.Host = Await Status.SuggestAsync()
        End If
        Dim cred = Credential
        cred.State = opts.Host
        Return cred.GetHelper()
    End Function

    <Extension>
    Public Function GetUseregHelper(opts As VerbBase) As UseregHelper
        Dim cred = Credential
        Return cred.GetUseregHelper()
    End Function

    Private Function ReadPassword() As String
        Dim builder As New StringBuilder
        Do
            Dim c = Console.ReadKey(True)
            Select Case c.Key
                Case ConsoleKey.Enter
                    Exit Do
                Case ConsoleKey.Backspace
                    builder.Remove(builder.Length - 1, 1)
                Case Else
                    builder.Append(c.KeyChar)
            End Select
        Loop
        Console.WriteLine()
        Return builder.ToString()
    End Function

    Public Function ReadCredential() As NetCredential
        Console.Write("请输入用户名：")
        Dim u = Console.ReadLine()
        Console.Write("请输入密码：")
        Dim p = ReadPassword()
        Return New NetCredential() With {.Username = u, .Password = p}
    End Function

    Public Property Credential As NetCredential
        Get
            If File.Exists(SettingsPath) Then
                Dim cred As New NetCredential()
                Using stream As New StreamReader(SettingsPath)
                    Using reader As New JsonTextReader(stream)
                        Dim json = JObject.Load(reader)
                        cred.Username = GetSettings(json, "username", String.Empty)
                        cred.Password = Encoding.UTF8.GetString(Convert.FromBase64String(GetSettings(json, "password", String.Empty)))
                    End Using
                End Using
                Return cred
            Else
                Return ReadCredential()
            End If
        End Get
        Set(value As NetCredential)
            Dim json As New JObject
            json("username") = If(value.Username, String.Empty)
            json("password") = Convert.ToBase64String(Encoding.UTF8.GetBytes(If(value.Password, String.Empty)))
            CreateSettingsFolder()
            Using stream As New StreamWriter(SettingsPath)
                Using writer As New JsonTextWriter(stream)
                    json.WriteTo(writer)
                End Using
            End Using
        End Set
    End Property
End Module

Module SettingsHelper
    Private Const settingsFilename As String = "settings.json"
    Public ReadOnly Property SettingsPath As String
        Get
            Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "TsinghuaNet.CLI", settingsFilename)
        End Get
    End Property

    Public Function GetSettings(json As JObject, key As String, def As JToken) As JToken
        If json.ContainsKey(key) Then
            Return json(key)
        Else
            Return def
        End If
    End Function

    Public Sub CreateSettingsFolder()
        Dim home As New DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
        home.CreateSubdirectory(Path.Combine(".config", "TsinghuaNet.CLI"))
    End Sub

    Public Sub DeleteSettings()
        Dim p = SettingsPath
        If File.Exists(p) Then
            Console.Write("是否要删除设置文件？[y/N]")
            Dim de = Console.ReadLine()
            If String.Equals(de, "y", StringComparison.OrdinalIgnoreCase) Then
                File.Delete(p)
                Console.WriteLine("已删除")
            End If
        End If
    End Sub
End Module
