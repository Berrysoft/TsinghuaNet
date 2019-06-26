Imports Newtonsoft.Json.Linq

''' <summary>
''' 网页的回复
''' </summary>
Public Structure LogResponse
    ''' <summary>
    ''' 是否成功
    ''' </summary>
    Public ReadOnly Succeed As Boolean
    ''' <summary>
    ''' 回复消息
    ''' </summary>
    Public ReadOnly Message As String

    ''' <summary>
    ''' 使用相应信息初始化<see cref="LogResponse"/>
    ''' </summary>
    ''' <param name="succeed">是否成功</param>
    ''' <param name="message">回复消息</param>
    Public Sub New(succeed As Boolean, message As String)
        Me.Succeed = succeed
        Me.Message = message
    End Sub

    Public Shared Operator =(r1 As LogResponse, r2 As LogResponse) As Boolean
        Return r1.Succeed = r2.Succeed AndAlso r1.Message = r2.Message
    End Operator

    Public Shared Operator <>(r1 As LogResponse, r2 As LogResponse) As Boolean
        Return Not r1 = r2
    End Operator

    ''' <inheritdoc/>
    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is LogResponse AndAlso Me = CType(obj, LogResponse)
    End Function

    ''' <inheritdoc/>
    Public Overrides Function GetHashCode() As Integer
        Return Succeed.GetHashCode() Xor If(Message?.GetHashCode(), 0)
    End Function

    ''' <inheritdoc/>
    Public Overrides Function ToString() As String
        Return Message
    End Function

    ''' <summary>
    ''' 转换来自 net.tsinghua.edu.cn 的回复
    ''' </summary>
    ''' <param name="response">回复字符串</param>
    ''' <returns>实例</returns>
    Friend Shared Function ParseFromNet(response As String) As LogResponse
        Return New LogResponse(response = "Login is successful.", response)
    End Function

    ''' <summary>
    ''' 转换来自 auth.tsinghua.edu.cn 的回复
    ''' </summary>
    ''' <param name="response">回复字符串</param>
    ''' <returns>实例</returns>
    Friend Shared Function ParseFromAuth(response As String) As LogResponse
        Try
            Dim jsonstr = response.Substring(9, response.Length - 10)
            Dim json = JObject.Parse(jsonstr)
            Return New LogResponse(json("error") = "ok", $"error: {json("error")}; error_msg: {json("error_msg")}")
        Catch ex As Exception
            Return New LogResponse(False, response)
        End Try
    End Function

    ''' <summary>
    ''' 转换来自 usereg.tsinghua.edu.cn 的回复
    ''' </summary>
    ''' <param name="response">回复字符串</param>
    ''' <returns>实例</returns>
    Friend Shared Function ParseFromUsereg(response As String) As LogResponse
        Return New LogResponse(response = "ok", response)
    End Function
End Structure
