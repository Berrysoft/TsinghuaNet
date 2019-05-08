Public Module FluxHelper
    Public Const BaseFlux As Long = 25000000000

    Public Function GetMaxFlux(flux As Long, balance As Decimal) As Long
        Return Math.Max(flux, BaseFlux) + balance / 2 * 1000 * 1000 * 1000
    End Function
End Module
