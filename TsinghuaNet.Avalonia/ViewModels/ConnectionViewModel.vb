Public Class ConnectionViewModel
    Inherits Helper.ConnectionViewModel

    Public Sub New()
        RefreshNetUsers()
    End Sub

    Public Event Drop As EventHandler
    Public Sub DropSelection()
        RaiseEvent Drop(Me, EventArgs.Empty)
    End Sub
End Class
