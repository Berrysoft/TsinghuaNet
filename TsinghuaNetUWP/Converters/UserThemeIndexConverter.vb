Imports TsinghuaNetUWP.Helper

Public Class UserThemeIndexConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Return CInt(CType(value, UserTheme))
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Return CType(CInt(value), UserTheme)
    End Function
End Class
