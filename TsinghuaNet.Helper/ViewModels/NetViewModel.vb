Imports TsinghuaNet

Public MustInherit Class NetViewModel
    Inherits NetModel

    Public Sub New()
        LoadSettings()
    End Sub

    Public MustOverride Sub LoadSettings()

    Public MustOverride Sub SaveSettings()

    Private _AutoLogin As Boolean
    Public Property AutoLogin As Boolean
        Get
            Return _AutoLogin
        End Get
        Set(value As Boolean)
            SetProperty(_AutoLogin, value)
        End Set
    End Property

    Private _EnableFluxLimit As Boolean
    Public Property EnableFluxLimit As Boolean
        Get
            Return _EnableFluxLimit
        End Get
        Set(value As Boolean)
            SetProperty(_EnableFluxLimit, value)
        End Set
    End Property

    Private _FluxLimit As Double
    Public Property FluxLimit As Double
        Get
            Return _FluxLimit
        End Get
        Set(value As Double)
            SetProperty(_FluxLimit, value)
        End Set
    End Property
End Class
