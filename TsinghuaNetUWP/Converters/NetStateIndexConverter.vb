Imports TsinghuaNetUWP.Helper

Public Class NetStateIndexConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Return CInt(CType(value, NetState))
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Return CType(CInt(value), NetState)
    End Function
End Class
