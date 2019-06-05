Public Module FluxHelper
    Public Const BaseFlux As Long = 25_000_000_000

    Public Function GetMaxFlux(flux As Long, balance As Decimal) As Long
        Return Math.Max(flux, BaseFlux) + balance / 2 * 1_000_000_000
    End Function
End Module
