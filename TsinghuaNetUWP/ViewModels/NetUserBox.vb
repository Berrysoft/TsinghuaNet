Imports System.Net
Imports Berrysoft.Tsinghua.Net

Public Class NetUserBox
    Inherits DependencyObject
    Implements IEquatable(Of NetUser)

    Public Sub New(Optional user As NetUser = Nothing)
        If user IsNot Nothing Then
            Address = user.Address
            LoginTime = user.LoginTime
            Client = user.Client
        End If
    End Sub

    Public Shared ReadOnly AddressProperty As DependencyProperty = DependencyProperty.Register(NameOf(Address), GetType(IPAddress), GetType(NetUserBox), New PropertyMetadata(Nothing))
    Public Property Address As IPAddress
        Get
            Return GetValue(AddressProperty)
        End Get
        Set(value As IPAddress)
            SetValue(AddressProperty, value)
        End Set
    End Property

    Public Shared ReadOnly LoginTimeProperty As DependencyProperty = DependencyProperty.Register(NameOf(LoginTime), GetType(Date), GetType(NetUserBox), New PropertyMetadata(New Date()))
    Public Property LoginTime As Date
        Get
            Return GetValue(LoginTimeProperty)
        End Get
        Set(value As Date)
            SetValue(LoginTimeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ClientProperty As DependencyProperty = DependencyProperty.Register(NameOf(Client), GetType(String), GetType(NetUserBox), New PropertyMetadata(String.Empty))
    Public Property Client As String
        Get
            Return GetValue(ClientProperty)
        End Get
        Set(value As String)
            SetValue(ClientProperty, value)
        End Set
    End Property

    Public Sub Drop()
        RaiseEvent DropUser(Me, Address)
    End Sub

    Public Overloads Function Equals(other As NetUser) As Boolean Implements IEquatable(Of NetUser).Equals
        Return Address.Equals(other) AndAlso LoginTime = other.LoginTime AndAlso Client = other.Client
    End Function

    Public Event DropUser As EventHandler(Of IPAddress)
End Class
