Imports System.Globalization
Imports Eto.Forms

Public Class StringFormatConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If parameter Is Nothing OrElse String.IsNullOrEmpty(parameter) Then
            Return value.ToString()
        Else
            Return String.Format(parameter, value)
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
