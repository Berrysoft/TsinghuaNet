Imports MvvmHelpers

Public Class NetObservableBase
    Inherits ObservableObject

    Private Shared ReadOnly _Credential As New NetCredential()

    Public ReadOnly Property Credential As NetCredential
        Get
            Return _Credential
        End Get
    End Property

    Private _IsBusy As Boolean
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(value As Boolean)
            SetProperty(_IsBusy, value)
        End Set
    End Property
End Class
