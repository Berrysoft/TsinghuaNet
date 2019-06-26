''' <summary>
''' 表示在线用户信息
''' </summary>
Public Structure FluxUser
    ''' <summary>
    ''' 用户名
    ''' </summary>
    Public ReadOnly Property Username As String
    ''' <summary>
    ''' 使用流量
    ''' </summary>
    Public ReadOnly Property Flux As ByteSize
    ''' <summary>
    ''' 在线时长
    ''' </summary>
    Public ReadOnly Property OnlineTime As TimeSpan
    ''' <summary>
    ''' 剩余流量
    ''' </summary>
    Public ReadOnly Property Balance As Decimal

    ''' <summary>
    ''' 使用相应信息初始化<see cref="FluxUser"/>
    ''' </summary>
    ''' <param name="username">用户名</param>
    ''' <param name="flux">使用流量</param>
    ''' <param name="onlineTime">在线时长</param>
    ''' <param name="balance">剩余流量</param>
    Public Sub New(username As String, flux As ByteSize, onlineTime As TimeSpan, balance As Decimal)
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

    ''' <inheritdoc/>
    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is FluxUser AndAlso Me = CType(obj, FluxUser)
    End Function

    ''' <inheritdoc/>
    Public Overrides Function GetHashCode() As Integer
        Return If(Username?.GetHashCode(), 0)
    End Function

    ''' <summary>
    ''' 转换字符串为<see cref="FluxUser"/>实例
    ''' </summary>
    ''' <param name="fluxstr">字符串</param>
    ''' <returns>实例</returns>
    Friend Shared Function Parse(fluxstr As String) As FluxUser
        Dim r = fluxstr.Split(","c)
        If String.IsNullOrWhiteSpace(r(0)) Then
            Return Nothing
        Else
            Return New FluxUser(
                r(0),
                ByteSize.FromBytes(r(6)),
                TimeSpan.FromSeconds(CLng(r(2)) - CLng(r(1))),
                r(11))
        End If
    End Function
End Structure
