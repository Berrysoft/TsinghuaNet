Imports TsinghuaNetUWP.Helper

Public Class UserContentTypeIndexConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Return CInt(CType(value, UserContentType))
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Return CType(CInt(value), UserContentType)
    End Function
End Class
