Imports Newtonsoft.Json.Linq

Public Structure LogResponse
    Public ReadOnly Succeed As Boolean
    Public ReadOnly Message As String

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

    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is LogResponse AndAlso Me = CType(obj, LogResponse)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Succeed.GetHashCode() Xor If(Message?.GetHashCode(), 0)
    End Function

    Public Overrides Function ToString() As String
        Return Message
    End Function

    Friend Shared Function ParseFromNet(response As String) As LogResponse
        Return New LogResponse(response = "Login is successful.", response)
    End Function

    Friend Shared Function ParseFromAuth(response As String) As LogResponse
        Try
            Dim jsonstr = response.Substring(9, response.Length - 10)
            Dim json = JObject.Parse(jsonstr)
            Return New LogResponse(json("error") = "ok", $"error: {json("error")}; error_msg: {json("error_msg")}")
        Catch ex As Exception
            Return New LogResponse(False, response)
        End Try
    End Function

    Friend Shared Function ParseFromUsereg(response As String) As LogResponse
        Return New LogResponse(response = "ok", response)
    End Function
End Structure
