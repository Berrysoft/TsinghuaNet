Imports System.Globalization
Imports Eto.Forms

Public Class FluxLimitConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim bytes As ByteSize = value
        Return CInt(bytes.GigaBytes)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return ByteSize.FromGigaBytes(value)
    End Function
End Class
