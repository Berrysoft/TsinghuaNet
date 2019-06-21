Public Class EnumIndexConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        If [Enum].IsDefined(value.GetType(), value) Then
            Return value
        Else
            Return 0
        End If
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Return value
    End Function
End Class
