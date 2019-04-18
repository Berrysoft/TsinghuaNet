Imports TsinghuaNetUWP.Helper

Public NotInheritable Class SsidSuggestion
    Inherits StackPanel

    Public Shared ReadOnly SsidProperty As DependencyProperty = DependencyProperty.Register(NameOf(Ssid), GetType(String), GetType(SsidSuggestion), New PropertyMetadata(Nothing))
    Public Property Ssid As String
        Get
            Return GetValue(SsidProperty)
        End Get
        Set(value As String)
            SetValue(SsidProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register(NameOf(Value), GetType(String), GetType(SsidSuggestion), New PropertyMetadata(NetState.Unknown))
    Public Property Value As NetState
        Get
            Return GetValue(ValueProperty)
        End Get
        Set(value As NetState)
            SetValue(ValueProperty, value)
        End Set
    End Property

    Public Shared ReadOnly SsidStyleProperty As DependencyProperty = DependencyProperty.Register(NameOf(SsidStyle), GetType(Style), GetType(SsidSuggestion), New PropertyMetadata(Nothing))
    Public Property SsidStyle As Style
        Get
            Return GetValue(SsidStyleProperty)
        End Get
        Set(value As Style)
            SetValue(SsidStyleProperty, value)
        End Set
    End Property
End Class
