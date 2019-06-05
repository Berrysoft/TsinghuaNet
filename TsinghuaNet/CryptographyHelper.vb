Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text

Module CryptographyHelper
    Private Function GetHexString(data() As Byte) As String
        Dim builder As New StringBuilder(data.Length * 2)
        For Each d In data
            builder.Append(d.ToString("x2"))
        Next
        Return builder.ToString()
    End Function

    Public Function GetMD5(input As String) As String
        If String.IsNullOrEmpty(input) Then
            Return String.Empty
        End If
        Using md5Hash = MD5.Create()
            Dim data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input))
            Return GetHexString(data)
        End Using
    End Function

    Public Function GetSHA1(input As String) As String
        If String.IsNullOrEmpty(input) Then
            Return String.Empty
        End If
        Using sha1Hash = SHA1.Create()
            Dim data = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(input))
            Return GetHexString(data)
        End Using
    End Function

    Private Function S(a() As Byte, b As Boolean) As UInteger()
        Dim c As Integer = a.Length
        Dim n = (c + 3) \ 4
        Dim v() As UInteger
        If b Then
            ReDim v(n)
            v(n) = c
        Else
            ReDim v(Math.Max(n, 4) - 1)
        End If
        Unsafe.CopyBlock(Unsafe.As(Of UInteger, Byte)(v(0)), a(0), c)
        Return v
    End Function

    Private Function L(a() As UInteger, b As Boolean) As Byte()
        Dim d As Integer = a.Length
        Dim c As UInteger = CUInt(d - 1) << 2
        If b Then
            Dim m As UInteger = a(d - 1)
            If m < c - 3 OrElse m > c Then
                Return Array.Empty(Of Byte)()
            End If
            c = m
        End If
        Dim n As UInteger = If(b, c, d << 2)
        Dim aa(n - 1) As Byte
        Unsafe.CopyBlock(aa(0), Unsafe.As(Of UInteger, Byte)(a(0)), n)
        Return aa
    End Function

    Public Function XEncode(str As String, key As String) As Byte()
        If String.IsNullOrEmpty(str) Then
            Return Array.Empty(Of Byte)()
        End If
        Dim v = S(Encoding.UTF8.GetBytes(str), True)
        Dim k = S(Encoding.UTF8.GetBytes(key), False)
        Dim n As Integer = v.Length - 1
        Dim z As UInteger = v(n)
        Dim y As UInteger
        Dim q As Integer = 6 + 52 \ (n + 1)
        Dim d As UInteger = 0
        Do While q > 0
            q -= 1
            d += &H9E3779B9
            Dim e As UInteger = (d >> 2) And 3
            For p = 0 To n
                y = v((p + 1) Mod (n + 1))
                Dim m As UInteger = (z >> 5) Xor (y << 2)
                m += (y >> 3) Xor (z << 4) Xor d Xor y
                m += k((p And 3) Xor CInt(e)) Xor z
                v(p) += m
                z = v(p)
            Next
        Loop
        Return L(v, False)
    End Function

    Private Const Base64N = "LVoJPiCN2R8G90yg+hmFHuacZ1OWMnrsSTXkYpUq/3dlbfKwv6xztjI7DeBE45QA"

    Public Function Base64Encode(t() As Byte) As String
        Dim a = t.Length
        Dim len = ((a + 2) \ 3) * 4
        Dim u As New StringBuilder(len)
        Const r = "="c
        For o = 0 To a - 1 Step 3
            Dim h = (CInt(t(o)) << 16) +
                    If(o + 1 < a, CInt(t(o + 1)) << 8, 0) +
                    If(o + 2 < a, t(o + 2), 0)
            For i = 0 To 3
                If o * 8 + i * 6 > a * 8 Then
                    u.Append(r)
                Else
                    u.Append(Base64N((h >> (6 * (3 - i))) And &H3F))
                End If
            Next
        Next
        Return u.ToString()
    End Function

    Public Function GetHMACMD5(key As String) As String
        If key Is Nothing Then
            key = String.Empty
        End If
        Using hash As New HMACMD5(Encoding.UTF8.GetBytes(key))
            Return GetHexString(hash.ComputeHash(Array.Empty(Of Byte)()))
        End Using
    End Function
End Module
