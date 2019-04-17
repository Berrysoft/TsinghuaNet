Imports Berrysoft.Tsinghua.Net

Public Class FluxUserBox
    Inherits DependencyObject

    Public Sub New(Optional flux As FluxUser = Nothing)
        If flux IsNot Nothing Then
            Username = flux.Username
            Me.Flux = flux.Flux
            OnlineTime = flux.OnlineTime
            Balance = flux.Balance
        End If
    End Sub

    Public Shared ReadOnly UsernameProperty As DependencyProperty = DependencyProperty.Register(NameOf(Username), GetType(String), GetType(FluxUserBox), New PropertyMetadata(String.Empty))
    Public Property Username As String
        Get
            Return GetValue(UsernameProperty)
        End Get
        Set(value As String)
            SetValue(UsernameProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FluxProperty As DependencyProperty = DependencyProperty.Register(NameOf(Flux), GetType(Long), GetType(FluxUserBox), New PropertyMetadata(0L))
    Public Property Flux As Long
        Get
            Return GetValue(FluxProperty)
        End Get
        Set(value As Long)
            SetValue(FluxProperty, value)
        End Set
    End Property

    Public Shared ReadOnly OnlineTimeProperty As DependencyProperty = DependencyProperty.Register(NameOf(OnlineTime), GetType(TimeSpan), GetType(FluxUserBox), New PropertyMetadata(TimeSpan.Zero))
    Public Property OnlineTime As TimeSpan
        Get
            Return GetValue(OnlineTimeProperty)
        End Get
        Set(value As TimeSpan)
            SetValue(OnlineTimeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly BalanceProperty As DependencyProperty = DependencyProperty.Register(NameOf(Balance), GetType(Decimal), GetType(FluxUserBox), New PropertyMetadata(0D))
    Public Property Balance As Decimal
        Get
            Return GetValue(BalanceProperty)
        End Get
        Set(value As Decimal)
            SetValue(BalanceProperty, value)
        End Set
    End Property
End Class
