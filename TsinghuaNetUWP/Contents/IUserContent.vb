﻿Public Interface IUserContent
    Property User As FluxUserBox

    Property OnlineTime As TimeSpan

    Property FreeOffset As Double

    Property FluxOffset As Double

    Property IsProgressActive As Boolean

    Sub BeginAnimation()

    Function AddOneSecond() As Boolean
End Interface

Module UserContentHelper
    Public Function Max(d1 As Double, d2 As Double) As Double
        Return Math.Max(d1, d2)
    End Function
End Module
