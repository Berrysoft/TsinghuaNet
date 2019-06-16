Imports System.Runtime.CompilerServices

Public Module EnumerableEx
    <Extension>
    Public Function OrderBy(Of T, TKey)(source As IEnumerable(Of T), selector As Func(Of T, TKey), descending As Boolean) As IOrderedEnumerable(Of T)
        If descending Then
            Return source.OrderByDescending(selector)
        Else
            Return source.OrderBy(selector)
        End If
    End Function

    <Extension>
    Public Function Sum(Of T)(source As IEnumerable(Of T), selector As Func(Of T, ByteSize)) As ByteSize
        Return New ByteSize(source.Sum(Function(v) selector(v).Bytes))
    End Function
End Module
