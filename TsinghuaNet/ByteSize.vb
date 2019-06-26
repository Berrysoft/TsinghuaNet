Imports System.Runtime.InteropServices

''' <summary>
''' 表示流量，以字节计算，并记1k=1000
''' </summary>
Public Structure ByteSize
    Implements IComparable
    Implements IComparable(Of ByteSize)
    Implements IEquatable(Of ByteSize)
    Implements IFormattable

    Private Const KILA As Long = 1000
    Private Const MEGA As Long = 1000_000
    Private Const GIGA As Long = 1000_000_000
    Private Const TERA As Long = 1000_000_000_000

    ''' <summary>
    ''' 字节数
    ''' </summary>
    ''' <returns>字节数</returns>
    Public Property Bytes As Long

    ''' <summary>
    ''' 千字节数
    ''' </summary>
    ''' <returns>KB</returns>
    Public Property KilaBytes As Double
        Get
            Return Bytes / KILA
        End Get
        Set(value As Double)
            Bytes = value * KILA
        End Set
    End Property

    ''' <summary>
    ''' 兆字节数
    ''' </summary>
    ''' <returns>MB</returns>
    Public Property MegaBytes As Double
        Get
            Return Bytes / MEGA
        End Get
        Set(value As Double)
            Bytes = value * MEGA
        End Set
    End Property

    ''' <summary>
    ''' 吉字节数
    ''' </summary>
    ''' <returns>GB</returns>
    Public Property GigaBytes As Double
        Get
            Return Bytes / GIGA
        End Get
        Set(value As Double)
            Bytes = value * GIGA
        End Set
    End Property

    ''' <summary>
    ''' 太字节数
    ''' </summary>
    ''' <returns>TB</returns>
    Public Property TeraBytes As Double
        Get
            Return Bytes / TERA
        End Get
        Set(value As Double)
            Bytes = value * TERA
        End Set
    End Property

    ''' <summary>
    ''' 使用字节数初始化<see cref="ByteSize"/>
    ''' </summary>
    ''' <param name="bytes">字节数</param>
    Public Sub New(bytes As Long)
        Me.Bytes = bytes
    End Sub

    ''' <summary>
    ''' 使用字节数初始化<see cref="ByteSize"/>
    ''' </summary>
    ''' <param name="bytes">字节数</param>
    Public Shared Function FromBytes(bytes As Long) As ByteSize
        Return New ByteSize(bytes)
    End Function

    ''' <summary>
    ''' 使用千字节数初始化<see cref="ByteSize"/>
    ''' </summary>
    ''' <param name="kb">KB</param>
    Public Shared Function FromKilaBytes(kb As Double) As ByteSize
        Return New ByteSize(kb * KILA)
    End Function

    ''' <summary>
    ''' 使用兆字节数初始化<see cref="ByteSize"/>
    ''' </summary>
    ''' <param name="mb">MB</param>
    Public Shared Function FromMegaBytes(mb As Double) As ByteSize
        Return New ByteSize(mb * MEGA)
    End Function

    ''' <summary>
    ''' 使用吉字节数初始化<see cref="ByteSize"/>
    ''' </summary>
    ''' <param name="gb">GB</param>
    Public Shared Function FromGigaBytes(gb As Double) As ByteSize
        Return New ByteSize(gb * GIGA)
    End Function

    ''' <summary>
    ''' 使用太字节数初始化<see cref="ByteSize"/>
    ''' </summary>
    ''' <param name="tb">TB</param>
    Public Shared Function FromTeraBytes(tb As Double) As ByteSize
        Return New ByteSize(tb * TERA)
    End Function

    ''' <inheritdoc/>
    Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Return CompareTo(CType(obj, ByteSize))
    End Function

    ''' <inheritdoc/>
    Public Function CompareTo(other As ByteSize) As Integer Implements IComparable(Of ByteSize).CompareTo
        Return Bytes.CompareTo(other.Bytes)
    End Function

    ''' <inheritdoc/>
    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is ByteSize AndAlso Equals(CType(obj, ByteSize))
    End Function

    ''' <inheritdoc/>
    Public Overloads Function Equals(other As ByteSize) As Boolean Implements IEquatable(Of ByteSize).Equals
        Return Bytes = other.Bytes
    End Function

    ''' <inheritdoc/>
    Public Overrides Function GetHashCode() As Integer
        Return Bytes.GetHashCode()
    End Function

    ''' <inheritdoc/>
    Public Overrides Function ToString() As String
        Return ToString(Nothing, Nothing)
    End Function

    Public Overloads Function ToString(format As String) As String
        Return ToString(format, Nothing)
    End Function

    Public Overloads Function ToString(formatProvider As IFormatProvider) As String
        Return ToString(Nothing, formatProvider)
    End Function

    ''' <inheritdoc/>
    Public Overloads Function ToString(format As String, formatProvider As IFormatProvider) As String Implements IFormattable.ToString
        If format Is Nothing Then
            format = "F2"
        End If
        Dim b = Math.Abs(Bytes)
        If b < KILA Then
            Return $"{Bytes.ToString(format, formatProvider)} B"
        ElseIf b < MEGA Then
            Return $"{KilaBytes.ToString(format, formatProvider)} KB"
        ElseIf b < GIGA Then
            Return $"{MegaBytes.ToString(format, formatProvider)} MB"
        ElseIf b < TERA Then
            Return $"{GigaBytes.ToString(format, formatProvider)} GB"
        Else
            Return $"{TeraBytes.ToString(format, formatProvider)} TB"
        End If
    End Function

    ''' <summary>
    ''' 尝试将字符串转换为<see cref="ByteSize"/>实例
    ''' </summary>
    ''' <param name="str">字符串</param>
    ''' <param name="s">返回实例</param>
    ''' <returns>指示转换是否成功</returns>
    Public Shared Function TryParse(str As String, <Out> ByRef s As ByteSize) As Boolean
        If str Is Nothing Then Return False
        Dim index As Integer = 0
        Do While index < str.Length
            If Not (Char.IsDigit(str(index)) OrElse str(index) = "."c) Then
                Exit Do
            End If
            index += 1
        Loop
        Dim f = Double.Parse(str.Substring(0, index))
        Dim id = str.Substring(index).Trim().ToUpper()
        Select Case id
            Case "B"
                s = New ByteSize(f)
            Case "K", "KB"
                s = FromKilaBytes(f)
            Case "M", "MB"
                s = FromMegaBytes(f)
            Case "G", "GB"
                s = FromGigaBytes(f)
            Case "T", "TB"
                s = FromTeraBytes(f)
            Case Else
                Return False
        End Select
        Return True
    End Function

    ''' <summary>
    ''' 将字符串转换为<see cref="ByteSize"/>实例
    ''' </summary>
    ''' <param name="str">字符串</param>
    ''' <returns>实例</returns>
    ''' <exception cref="ArgumentNullException">当<paramref name="str"/>为<see langword="Nothing"/></exception>
    ''' <exception cref="FormatException">不支持的单位</exception>
    Public Shared Function Parse(str As String) As ByteSize
        If str Is Nothing Then Throw New ArgumentNullException(NameOf(str))
        Dim result As ByteSize
        If TryParse(str, result) Then
            Return result
        Else
            Throw New FormatException("不支持的单位。")
        End If
    End Function

    Public Shared Operator =(s1 As ByteSize, s2 As ByteSize) As Boolean
        Return s1.Equals(s2)
    End Operator

    Public Shared Operator <>(s1 As ByteSize, s2 As ByteSize) As Boolean
        Return Not s1 = s2
    End Operator

    Public Shared Operator <(s1 As ByteSize, s2 As ByteSize) As Boolean
        Return s1.Bytes < s2.Bytes
    End Operator

    Public Shared Operator >(s1 As ByteSize, s2 As ByteSize) As Boolean
        Return s2 < s1
    End Operator

    Public Shared Operator <=(s1 As ByteSize, s2 As ByteSize) As Boolean
        Return Not s2 < s1
    End Operator

    Public Shared Operator >=(s1 As ByteSize, s2 As ByteSize) As Boolean
        Return Not s1 < s2
    End Operator

    Public Shared Operator +(s As ByteSize) As ByteSize
        Return s
    End Operator

    Public Shared Operator -(s As ByteSize) As ByteSize
        Return New ByteSize(-s.Bytes)
    End Operator

    Public Shared Operator +(s1 As ByteSize, s2 As ByteSize) As ByteSize
        Return New ByteSize(s1.Bytes + s2.Bytes)
    End Operator

    Public Shared Operator -(s1 As ByteSize, s2 As ByteSize) As ByteSize
        Return New ByteSize(s1.Bytes - s2.Bytes)
    End Operator

    Public Shared Operator /(s As ByteSize, r As Double) As ByteSize
        Return New ByteSize(s.Bytes / r)
    End Operator

    Public Shared Operator /(s1 As ByteSize, s2 As ByteSize) As Double
        Return s1.Bytes / s2.Bytes
    End Operator
End Structure
