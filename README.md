# Binding

## Setting things up and background reading
Objects that want to notify other parts of the system when their properties change must implement [`INotifyPropertyChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-7.0).
`INotifyPropertyChanged` look like:
```c#
public interface INotifyPropertyChanged
{
    // Occurs when a property value changes.
    event PropertyChangedEventHandler PropertyChanged;
}
```

Frameworks that support binding (e.g. Xamarin Forms) have a binding class that hooks into/provides the implementation of the event. So when you fulfill your side of the interface by triggering the event, what you're actually doing is running the binding class' method that (probably) updates relevant UI elements/views.

In the most raw form, you could create trigger the event at the appropriate time:
```c#
public class MyClassThatNotifiesOthers
{
    private event PropertyChangedEventHandler EventHandlerWhenNameChanges;
    private string _name;
    public string Name
    {
        get => this._name;
        set
        {
            this._name = value;
            this.EventHandlerWhenNameChanges?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }
    }
}
```

The most basic (and bad) way of binding a view to some object property is to do it in the view's code-behind.
So, say I am creating a `HomePage` component.
It will extend from [`Xamarin.Forms.ContentPage`](https://learn.microsoft.com/en-us/dotnet/api/xamarin.forms.contentpage?view=xamarin-forms).
All [Pages](https://learn.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/controls/pages) are subclasses of [`Xamarin.Forms.BindableObject`](https://learn.microsoft.com/en-us/dotnet/api/xamarin.forms.bindableobject?view=xamarin-forms), which implements `INotifyPropertyChanged`.
`BindableObject` implements `INotifyPropetyChanged` like so:
```c#
public class BindableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    //...
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

_That means we have an easy way for our components to logically implement `INotifyPropertyChanged`, via `BindableObject.OnPropertyChanged()`._

We make our viewmodels extend from that base class and call that method when its properties actually change.
Then when we bind views to the viewmodel's properties, Xamarin can handle the visual updating.
```c#
public class MyViewModel : BindableObject
{
    private string displayString;
    public string DisplayString
    {
        get => this.displayString;
        set
        {
            if (this.displayString != value)
            {
                this.displayString = value;
                this.OnPropertyChanged(nameof(this.DisplayString));
            }
        }
    }
}
```
<br />

### Ok Xamarin provides an implementation of `INotifyProperyChanged`, so why do we use Prism and/or ReactiveUI?
[Prism](https://prismlibrary.com/docs), [ReactiveUI](https://github.com/reactiveui/ReactiveUI), and [MVVMHelpers](https://github.com/jamesmontemagno/mvvm-helpers) all offer their own base classes that implement `INotifyProperyChanged`.
Each also comes with more OOTB features, but they are very similar in how they go about `INotifyProperyChanged` implementation.

|Library|class to extend|extended class method to call in your VM's setter to trigger the `INotifyProperyChanged.PropertyChanged` event|
|-|-|-|
|Xamarin.Forms|`BindableObject`|`OnPropertyChanged()`|
|ReactiveUI|`ReactiveObject`|`RaiseAndSetIfChanged()`|
|Prism|`BindableBase`|`SetProperty()`|
<br />

### Alright, but what's this `[Reactive]` attribute on all the bound-to properties?
This is a [Fody](https://github.com/Fody/Home) thing.
Specifically the ["Reactive"](https://github.com/Fody/Home/blob/master/pages/addins.md#addins-list) add-in for Fody.
Fody is an extensible tool for weaving .NET assemblies, and they'll inject INotifyPropertyChanged code into properties at compile time for you.
It reduces rendundant boilerplate code, simplifying this...
```c#
private string name;
public string Name 
{
    get => name;
    set => this.RaiseAndSetIfChanged(ref name, value);
}
```

...into this

```c#
[Reactive]
public string Name { get; set; }
```
<br />

## Vanilla Xamarin.Forms binding
* [Microsoft Learn chapters about binding](https://learn.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/data-binding/)

<br />

## View-to-View Binding with Vanilla Xamarin
* You can bind the properties of a view to the properties of another view in the same page
    * Microsoft's example is a slider with a label. The text on the label is bound to the value of the slider: if the slider goes up, the text automatically updates to show the value
* This is usefu/necessary in a `ListView`
    * The binding context of each item in the list is set to the appropriate element in `ListView.ItemsSource`
    * If you want each repeated item to reference the same binding context as the `ListView`'s (which is probably the viewmodel of that page), do it like so:
    * `{Binding Source={x:Reference MyListView}, Path=BindingContext}` is like saying "this binding comes from the 'BindingContext' property of the 'MyListView' view/element"
    * See [Microsoft's section on this](https://stackoverflow.com/a/40913726/21644376) as well as [this SO answer](https://stackoverflow.com/a/40913726/21644376)
```xml
<ListView x:Name="MyListView"
    ItemsSource="{Binding MyBaseballTeams}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <Label Text="{Binding TeamName}" />
                <Button BindingContext="{Binding Source={x:Reference MyListView}, Path=BindingContext}"
                    Command="{Binding ViewTeamCommand}"
                    Text="View Info" />
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```
<br />

### Hmm okay, but zoom out a little and I don't see where the `ListView`'s binding context is set to anything...
* This is where [`ReactiveUI.ContentView<VM>`](https://www.reactiveui.net/api/reactiveui.xamforms/reactivecontentview_1/#reactivecontentview%3Ctviewmodel%3E-class) comes in
* I _think_ this automagically wires up the view to use the specified viewmodel parameter

<br />

## Binding with `ReactiveUI`
All of our viewmodels extend from `ReactiveUI.ReactiveObject`.
The [intro](https://www.reactiveui.net/docs/handbook/data-binding/) and [Xamarin Forms](https://www.reactiveui.net/docs/handbook/data-binding/xamarin-forms) docpages have more info.
```c#
this.OneWayBind(ViewModel,
    viewModel => viewModel.Person.Name,
    view => view.Name.Text);
// OR
this.WhenAnyValue(x => x.ViewModel.Person.Name)
    .BindTo(this, view => view.Name.Text);
```
TODO - make note on `.Bind()` (two-way)

<br />

<br />



# Navigation

## Prism Navigation
We use [Prism](https://prismlibrary.com/docs/xamarin-forms/navigation/navigation-basics.html) for navigation.
There's a three-step (plus some setup) process for getting started:

1. Make `App` extend `Prism.DryIoc.PrismApplication`
    * ctors
    * `OnInitialized()` should call `InitializeComponent()` and navigate to first page (maybe revisit this part after setting everything else up)
```c#
public partial class App : Prism.DryIoc.PrismApplication
{
    public App() : this(null) { }

    public App(IPlatformInitializer initializer) : base(initializer) { }

    protected override void OnInitialized()
    {
        InitializeComponent();
        var result = this.NavigationService.NavigateAsync("SplashPage");
    }
}
```

2. Register pages that should be navigable
    * do this in `App`'s RegisterForNavigation
    * the overload of `RegisterForNavigation<TPage, TViewModel>()` shown below handles setting the binding context of the specified page!
```c#
protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    containerRegistry.RegisterForNavigation<SplashPage, SplashPageViewModel>();
    containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
}
```

3. Inject an `INavigationService` into whichever viewmodels will be doing navigation:
```c#
public class SplashPageViewModel : ReactiveObject
{
    private readonly INavigationService _navigationService;

    public SplashPageViewModel(INavigationService navigationService)
    {
        this._navigationService = navigationService;
    }
}
```

4. Perform navigation with either relative or absolute paths:
    * kind of important to handle failed navigation - otherwise it fails silently...
```c#
private async void NavigateToHome()
{
    var result = await this._navigationService.NavigateAsync("HomePage");
    if (!result.Success)
    {
        Console.WriteLine(result.Exception);
    }
}
```
<br />

## Notes on Prism navigation
* When registering pages for navigation, you _can_ provide a different key/identifier, but using the page/class name seems better
* There are two types of navigation paths
    * __Relative__: subsequent navs will create a stack (e.g. `NavigateAsync("SomeView")`)
    * __Absolute__: prefixing a nav with backslash will reset the stack (e.g. `NavigateAsync("/SomeView")`)
* That's it!

<br />
<br />



# Observables

## History Lesson
Microsoft came out with Reactive Extensions ("ReactiveX") and its initial implementation on .NET. Today, ReactiveX has implementations in most major languages.

Similar to `INotifyPropertyChanged` is [`INotifyCollectionChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.inotifycollectionchanged). It defines an event that should be raised "when the underlying collection changes".
The provided implementation is [`ObservableCollection<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1) which "Represents a dynamic data collection that provides notifications _when items get added or removed, or when the whole list is refreshed_".

The expected behavior might be that if you change a property on an object _in_ the collection, the event would be raised. After all, you are modifying the collection, right? Nope - only collection-related actions (add, remove, clear) trigger the event.

## Enter DynamicData
[DynamicData](https://dynamic-data.org/) ([Github](https://github.com/reactivemarbles/DynamicData)) does two things:
1. Raises the `CollectionChanged` event when an item in the collection is changed
2. "Expose changes to the collection via an observable change set. The resulting observable change sets can be manipulated and transformed using Dynamic Data’s robust and powerful array of change set operators". In other words, a LINQ-like API to take action on observables

Using DynamicData is also a multi-step process:
1. Define a change set cache (optionally with a unique key selector)
```c#
private SourceCache<Tweet, Guid> _tweetCache = new SourceCache<Tweet, Guid>(x => x.UniqueId);
```

2. Define an ObservableCollection
```c#
private readonly ReadOnlyObservableCollection<Tweet> _tweets;
```

3. Expose the collection with a property
```c#
public ReadOnlyObservableCollection<Tweet> Tweets => this._tweets;
```

4. In the constructor, set up the cache and connect the observable
```c#
public TwitterFeedViewModel()
{
    IObservable<IChangeSet<Tweet, Guid>> changeSet = this._tweetCache
        .Connect()
        .RefCount();
    
    changeSet
        .Bind(out this._tweets)     // step 2
        .DisposeMany()
        .Subscribe()
        .DisposeWith(this.Disposables);
}
```

5. Bind a view to the exposed property (either XAML or code-behind):
```xml
<ListView
    x:Name="TweetListView"
    ItemsSource="{Binding Tweets}">
</ListView>
```
or
```c#
public MyTweetView()
{
    this.WhenAnyValue(view => view.ViewModel.Tweets)
        .BindTo(this, view => view.TweetListView.ItemsSource)
        .DisposeWith(this.Disposables);
}
```