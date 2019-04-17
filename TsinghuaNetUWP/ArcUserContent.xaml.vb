'https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

Imports TsinghuaNetUWP

Public NotInheritable Class ArcUserContent
    Inherits UserControl
    Implements IUserContent

    Public Property User As FluxUserBox Implements IUserContent.User

    Public Property OnlineTime As TimeSpan Implements IUserContent.OnlineTime

    Public Property FreeOffset As Double Implements IUserContent.FreeOffset

    Public Property FluxOffset As Double Implements IUserContent.FluxOffset

    Public Property IsProgressActive As Boolean Implements IUserContent.IsProgressActive

    Public Sub BeginAnimation() Implements IUserContent.BeginAnimation
        Throw New NotImplementedException()
    End Sub

    Public Function AddOneSecond() As Boolean Implements IUserContent.AddOneSecond
        Throw New NotImplementedException()
    End Function
End Class
