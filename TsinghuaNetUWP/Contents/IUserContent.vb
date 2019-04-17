Public Interface IUserContent
    Property User As FluxUserBox

    Property OnlineTime As TimeSpan

    Property FreeOffset As Double

    Property FluxOffset As Double

    Property IsProgressActive As Boolean

    Sub BeginAnimation()

    Function AddOneSecond() As Boolean
End Interface
