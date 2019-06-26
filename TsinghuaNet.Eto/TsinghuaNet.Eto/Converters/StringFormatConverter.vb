Imports System.Globalization
Imports Eto.Forms

Public Class StringFormatConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If parameter Is Nothing Then
            Return value.ToString()
        Else
            Dim format = parameter.ToString()
            If String.IsNullOrEmpty(format) Then
                Return value.ToString()
            Else
                Return String.Format(format, value)
            End If
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
