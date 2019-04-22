Imports TsinghuaNetUWP.Helper

Public NotInheritable Class NetDetailBox
    Public Sub New(day As Integer, flux As Double)
        Me.Day = day
        Me.Flux = flux
    End Sub

    Public Property Day As Integer
    Public Property Flux As Double
    Public ReadOnly Property ToolTipLabel As String
        Get
            Return $"{Day}：{UserHelper.GetFluxString(Flux * 1000000000)}"
        End Get
    End Property
End Class
