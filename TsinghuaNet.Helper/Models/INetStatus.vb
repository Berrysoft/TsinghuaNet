Imports System.ComponentModel
Imports System.Net.NetworkInformation
Imports MvvmHelpers

Public Enum NetStatus
    Unknown
    Wwan
    Wlan
    Lan
End Enum

Public Interface INetStatus
    Inherits INotifyPropertyChanged
    Property Status As NetStatus
    Property Ssid As String
    Function RefreshAsync() As Task
    Function SuggestAsync() As Task(Of NetState)
End Interface

Public Class NetPingStatus
    Inherits ObservableObject
    Implements INetStatus

    Public Property Status As NetStatus Implements INetStatus.Status
        Get
            Return NetStatus.Unknown
        End Get
        Set(value As NetStatus)

        End Set
    End Property

    Public Property Ssid As String Implements INetStatus.Ssid
        Get
            Return Nothing
        End Get
        Set(value As String)

        End Set
    End Property

    Private Shared Async Function CanConnectTo(uri As String) As Task(Of Boolean)
        Try
            Dim p As New Ping
            Dim reply = Await p.SendPingAsync(uri)
            Return reply.Status = IPStatus.Success
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Shared Async Function GetSuggestion() As Task(Of NetState)
        If Await CanConnectTo("auth4.tsinghua.edu.cn") Then
            Return NetState.Auth4
        ElseIf Await CanConnectTo("net.tsinghua.edu.cn") Then
            Return NetState.Net
        ElseIf Await CanConnectTo("auth6.tsinghua.edu.cn") Then
            Return NetState.Auth6
        Else
            Return NetState.Unknown
        End If
    End Function

    Public Function RefreshAsync() As Task Implements INetStatus.RefreshAsync
        Return Task.CompletedTask
    End Function

    Public Function SuggestAsync() As Task(Of NetState) Implements INetStatus.SuggestAsync
        Return GetSuggestion()
    End Function
End Class

Public MustInherit Class NetMapStatus
    Inherits ObservableObject
    Implements INetStatus

    Private _Status As NetStatus
    Public Property Status As NetStatus Implements INetStatus.Status
        Get
            Return _Status
        End Get
        Set(value As NetStatus)
            SetProperty(_Status, value)
        End Set
    End Property

    Private _Ssid As String
    Public Property Ssid As String Implements INetStatus.Ssid
        Get
            Return _Ssid
        End Get
        Set(value As String)
            SetProperty(_Ssid, value)
        End Set
    End Property

    Public MustOverride Function RefreshAsync() As Task Implements INetStatus.RefreshAsync

    Private Shared SsidStateMap As New Dictionary(Of String, NetState) From
    {
        {"Tsinghua", NetState.Net},
        {"Tsinghua-5G", NetState.Net},
        {"Tsinghua-IPv4", NetState.Auth4},
        {"Tsinghua-IPv6", NetState.Auth6},
        {"Wifi.郑裕彤讲堂", NetState.Net}
    }

    Public Function SuggestAsync() As Task(Of NetState) Implements INetStatus.SuggestAsync
        Return Task.Run(
            Function()
                Select Case Status
                    Case NetStatus.Lan
                        Return NetState.Auth4
                    Case NetStatus.Wlan
                        If SsidStateMap.ContainsKey(Ssid) Then
                            Return SsidStateMap(Ssid)
                        Else
                            Return NetState.Unknown
                        End If
                    Case Else
                        Return NetState.Unknown
                End Select
            End Function)
    End Function
End Class
