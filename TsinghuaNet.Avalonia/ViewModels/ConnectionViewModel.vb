Public Class ConnectionViewModel
    Inherits Helper.ConnectionViewModel

    Public Event Drop As EventHandler
    Public Sub DropSelection()
        RaiseEvent Drop(Me, EventArgs.Empty)
    End Sub
End Class
