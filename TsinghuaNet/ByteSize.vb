Imports System.Runtime.InteropServices

Public Structure ByteSize
    Implements IComparable(Of ByteSize)
    Implements IEquatable(Of ByteSize)
    Implements IFormattable

    Private Const KILA As Long = 1000
    Private Const MEGA As Long = 1000_000
    Private Const GIGA As Long = 1000_000_000
    Private Const TERA As Long = 1000_000_000_000

    Public Property Bytes As Long

    Public Property KilaBytes As Double
        Get
            Return Bytes / KILA
        End Get
        Set(value As Double)
            Bytes = value * KILA
        End Set
    End Property

    Public Property MegaBytes As Double
        Get
            Return Bytes / MEGA
        End Get
        Set(value As Double)
            Bytes = value * MEGA
        End Set
    End Property

    Public Property GigaBytes As Double
        Get
            Return Bytes / GIGA
        End Get
        Set(value As Double)
            Bytes = value * GIGA
        End Set
    End Property

    Public Property TeraBytes As Double
        Get
            Return Bytes / TERA
        End Get
        Set(value As Double)
            Bytes = value * TERA
        End Set
    End Property

    Public Sub New(bytes As Long)
        Me.Bytes = bytes
    End Sub

    Public Shared Function FromBytes(bytes As Long) As ByteSize
        Return New ByteSize(bytes)
    End Function

    Public Shared Function FromKilaBytes(kb As Double) As ByteSize
        Return New ByteSize(kb * KILA)
    End Function

    Public Shared Function FromMegaBytes(mb As Double) As ByteSize
        Return New ByteSize(mb * MEGA)
    End Function

    Public Shared Function FromGigaBytes(gb As Double) As ByteSize
        Return New ByteSize(gb * GIGA)
    End Function

    Public Shared Function FromTeraBytes(tb As Double) As ByteSize
        Return New ByteSize(tb * TERA)
    End Function

    Public Function CompareTo(other As ByteSize) As Integer Implements IComparable(Of ByteSize).CompareTo
        Return Bytes.CompareTo(other.Bytes)
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is ByteSize AndAlso Equals(CType(obj, ByteSize))
    End Function

    Public Overloads Function Equals(other As ByteSize) As Boolean Implements IEquatable(Of ByteSize).Equals
        Return Bytes = other.Bytes
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return Bytes.GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return ToString(Nothing, Nothing)
    End Function

    Public Overloads Function ToString(format As String) As String
        Return ToString(format, Nothing)
    End Function

    Public Overloads Function ToString(formatProvider As IFormatProvider) As String
        Return ToString(Nothing, formatProvider)
    End Function

    Public Overloads Function ToString(format As String, formatProvider As IFormatProvider) As String Implements IFormattable.ToString
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

    Public Shared Function TryParse(str As String, <Out> ByRef s As ByteSize) As Boolean
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
                s = New ByteSize()
                Return False
        End Select
        Return True
    End Function

    Public Shared Function Parse(str As String) As ByteSize
        Dim result As ByteSize
        If TryParse(str, result) Then
            Return result
        Else
            Throw New ArgumentException(NameOf(str))
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
