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
End Module
