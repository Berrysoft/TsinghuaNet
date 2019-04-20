Imports Berrysoft.Tsinghua.Net
Imports WinRTXamlToolkit.AwaitableUI
Imports MvvmHelpers

Public NotInheritable Class GraphUserContent
    Inherits UserControl
    Implements IUserContent

    Public Shared ReadOnly UserProperty As DependencyProperty = DependencyProperty.Register(NameOf(User), GetType(FluxUser), GetType(GraphUserContent), New PropertyMetadata(Nothing, AddressOf UserPropertyChanged))
    Public Property User As FluxUser Implements IUserContent.User
        Get
            Return GetValue(UserProperty)
        End Get
        Set(value As FluxUser)
            SetValue(UserProperty, value)
        End Set
    End Property
    Private Shared Sub UserPropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim content As GraphUserContent = d
        Dim flux As FluxUser = e.NewValue
        If flux IsNot Nothing Then
            content.OnlineTime = flux.OnlineTime
        End If
    End Sub

    Public Shared ReadOnly OnlineTimeProperty As DependencyProperty = DependencyProperty.Register(NameOf(OnlineTime), GetType(TimeSpan), GetType(GraphUserContent), New PropertyMetadata(TimeSpan.Zero))
    Public Property OnlineTime As TimeSpan Implements IUserContent.OnlineTime
        Get
            Return GetValue(OnlineTimeProperty)
        End Get
        Set(value As TimeSpan)
            SetValue(OnlineTimeProperty, value)
        End Set
    End Property

    Public Property IsProgressActive As Boolean Implements IUserContent.IsProgressActive
        Get
            Return Progress.IsActive
        End Get
        Set(value As Boolean)
            Progress.IsActive = value
        End Set
    End Property

    Public Sub BeginAnimation() Implements IUserContent.BeginAnimation
        ShowStoryboard.Begin()
    End Sub

    Public Function AddOneSecond() As Boolean Implements IUserContent.AddOneSecond
        If User Is Nothing OrElse String.IsNullOrEmpty(User.Username) Then
            Return False
        Else
            OnlineTime += TimeSpan.FromSeconds(1)
            Return True
        End If
    End Function

    Public ReadOnly Property Details As New ObservableRangeCollection(Of KeyValuePair(Of Integer, Double))

    Friend Async Function RefreshDetails(helper As UseregHelper) As Task
        Dim animationTask = HideStoryboard.BeginAsync()
        Dim ds = Await helper.GetDetailsAsync()
        Await animationTask
        Details.ReplaceRange(ds.GetDailyDetails())
    End Function
End Class

Module NetDetailExtensions
    <Extension>
    Public Iterator Function GetDailyDetails(ds As IEnumerable(Of NetDetail)) As IEnumerable(Of KeyValuePair(Of Integer, Double))
        Dim totalf As Double = 0
        Dim now As Date = Date.Now
        For Each b In From d In ds
                      Where d.OnlineDate.Month = now.Month
                      Group By d.OnlineDate.Day Into Flux = Sum(d.Flux)
            totalf += b.Flux / 1000000000
            Yield New KeyValuePair(Of Integer, Double)(b.Day, totalf)
        Next
    End Function
End Module
