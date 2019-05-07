Imports System.Globalization

Public Module StringHelper
    Public Function GetFluxString(flux As Long) As String
        Dim f As Double = flux
        If f < 1000 Then
            Return $"{f} B"
        End If
        f /= 1000
        If f < 1000 Then
            Return $"{f:F2} KB"
        End If
        f /= 1000
        If f < 1000 Then
            Return $"{f:F2} MB"
        End If
        f /= 1000
        Return $"{f:F2} GB"
    End Function

    Public Function GetCurrencyString(currency As Decimal) As String
        Static zhCulture As New CultureInfo("zh-CN")
        Return currency.ToString("C2", zhCulture)
    End Function

    Public Function GetNetStateString(state As NetState) As String
        Select Case state
            Case NetState.Net
                Return "Net"
            Case NetState.Auth4
                Return "Auth4"
            Case NetState.Auth6
                Return "Auth6"
            Case Else
                Return "不登录"
        End Select
    End Function
End Module
