Public Module FluxHelper
    Public ReadOnly BaseFlux As ByteSize = ByteSize.FromGigaBytes(25)

    Public Function GetMaxFlux(flux As ByteSize, balance As Decimal) As ByteSize
        Return New ByteSize(Math.Max(flux.Bytes, BaseFlux.Bytes)) + ByteSize.FromGigaBytes(balance / 2)
    End Function
End Module
