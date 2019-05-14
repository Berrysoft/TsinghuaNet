Public Class NetViewModel
    Inherits NetModel

    Private _Response As String
    Public Property Response As String
        Get
            Return _Response
        End Get
        Set(value As String)
            SetProperty(_Response, value)
        End Set
    End Property

    Private _AutoLogin As Boolean = True
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
