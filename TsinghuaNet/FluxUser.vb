Public Structure FluxUser
    Public ReadOnly Property Username As String
    Public ReadOnly Property Flux As Long
    Public ReadOnly Property OnlineTime As TimeSpan
    Public ReadOnly Property Balance As Decimal

    Public Sub New(username As String, flux As Long, onlineTime As TimeSpan, balance As Decimal)
        Me.Username = username
        Me.Flux = flux
        Me.OnlineTime = onlineTime
        Me.Balance = balance
    End Sub

    Public Shared Operator =(u1 As FluxUser, u2 As FluxUser) As Boolean
        Return u1.Username = u2.Username AndAlso u1.Flux = u2.Flux AndAlso u1.OnlineTime = u2.OnlineTime AndAlso u1.Balance = u2.Balance
    End Operator

    Public Shared Operator <>(u1 As FluxUser, u2 As FluxUser) As Boolean
        Return Not u1 = u2
    End Operator

    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is FluxUser AndAlso Me = CType(obj, FluxUser)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return If(Username?.GetHashCode(), 0)
    End Function

    Friend Shared Function Parse(fluxstr As String) As FluxUser
        Dim r = fluxstr.Split(","c)
        If String.IsNullOrWhiteSpace(r(0)) Then
            Return Nothing
        Else
            Return New FluxUser(
                r(0),
                r(6),
                TimeSpan.FromSeconds(CLng(r(2)) - CLng(r(1))),
                r(11))
        End If
    End Function
End Structure
