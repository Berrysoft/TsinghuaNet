Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports Avalonia.ThemeManager
Imports MvvmHelpers
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class MainViewModel
    Inherits ObservableObject

    Private Const settingsFilename As String = "settings.json"

    Public Sub New()
        If File.Exists(settingsFilename) Then
            Try
                Using stream As New StreamReader(settingsFilename)
                    Using reader As New JsonTextReader(stream)
                        Dim json = JObject.Load(reader)
                        _Username = json("username")
                        _Password = Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(json("password")), Nothing, DataProtectionScope.CurrentUser))
                        _State = CInt(json("state"))
                    End Using
                End Using
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Sub SaveSettings()
        Dim json As New JObject
        json("username") = If(_Username, String.Empty)
        json("password") = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(If(_Password, String.Empty)), Nothing, DataProtectionScope.CurrentUser))
        json("state") = _State
        Using stream As New StreamWriter(settingsFilename)
            Using writer As New JsonTextWriter(stream)
                json.WriteTo(writer)
            End Using
        End Using
    End Sub

    Private _Username As String
    Public Property Username As String
        Get
            Return _Username
        End Get
        Set(value As String)
            SetProperty(_Username, value)
        End Set
    End Property

    Private _Password As String
    Public Property Password As String
        Get
            Return _Password
        End Get
        Set(value As String)
            SetProperty(_Password, value)
        End Set
    End Property

    Private _State As NetState
    Public Property State As NetState
        Get
            Return _State
        End Get
        Set(value As NetState)
            SetProperty(_State, value, onChanged:=AddressOf OnStateChanged)
        End Set
    End Property
    Private Sub OnStateChanged()
        If RefreshCommand.CanExecute(Nothing) Then
            RefreshCommand.Execute(Nothing)
        End If
    End Sub

    Public ReadOnly Property StateChangeCommand As New NetStateChangeCommand(Me)

    Private _OnlineUsername As String
    Public Property OnlineUsername As String
        Get
            Return _OnlineUsername
        End Get
        Set(value As String)
            SetProperty(_OnlineUsername, value)
        End Set
    End Property

    Private _OnlineFlux As Long
    Public Property OnlineFlux As Long
        Get
            Return _OnlineFlux
        End Get
        Set(value As Long)
            SetProperty(_OnlineFlux, value)
        End Set
    End Property

    Private _OnlineTime As TimeSpan
    Public Property OnlineTime As TimeSpan
        Get
            Return _OnlineTime
        End Get
        Set(value As TimeSpan)
            SetProperty(_OnlineTime, value)
        End Set
    End Property

    Private _OnlineBalance As Decimal
    Public Property OnlineBalance As Decimal
        Get
            Return _OnlineBalance
        End Get
        Set(value As Decimal)
            SetProperty(_OnlineBalance, value)
        End Set
    End Property

    Public ReadOnly Property LoginCommand As NetCommand = New LoginCommand(Me)
    Public ReadOnly Property LogoutCommand As NetCommand = New LogoutCommand(Me)
    Public ReadOnly Property RefreshCommand As NetCommand = New RefreshCommand(Me)

    Friend Function GetHelper() As IConnect
        Return ConnectHelper.GetHelper(State, Username, Password)
    End Function

    Friend Async Function RefreshAsync(helper As IConnect) As Task
        Dim flux As FluxUser = Nothing
        If helper IsNot Nothing Then
            flux = Await helper.GetFluxAsync()
        End If
        OnlineUsername = flux.Username
        OnlineFlux = flux.Flux
        OnlineTime = flux.OnlineTime
        OnlineBalance = flux.Balance
    End Function
End Class
