# Binding

## Absolute Vanilla .NET
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

## Ok Xamarin provides an implementation of `INotifyProperyChanged`, so why do we use Prism and/or ReactiveUI?
[Prism](https://prismlibrary.com/docs), [ReactiveUI](https://github.com/reactiveui/ReactiveUI), and [MVVMHelpers](https://github.com/jamesmontemagno/mvvm-helpers) all offer their own base classes that implement `INotifyProperyChanged`.
Each also comes with more OOTB features, but they are very similar in how they go about `INotifyProperyChanged` implementation.

|Library|class to extend|extended class method to call in your VM's setter to trigger the `INotifyProperyChanged.PropertyChanged` event|
|-|-|-|
|Xamarin.Forms|`BindableObject`|`OnPropertyChanged()`|
|ReactiveUI|`ReactiveObject`|`RaiseAndSetIfChanged()`|
|Prism|`BindableBase`|`SetProperty()`|
<br />

## Alright, but what's this `[Reactive]` attribute on all the bound-to properties?
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

## Actually binding
All of our viewmodels extend from `ReactiveUI.ReactiveObject`.
The [intro](https://www.reactiveui.net/docs/handbook/data-binding/) and [Xamarin Forms](https://www.reactiveui.net/docs/handbook/data-binding/xamarin-forms) docpages have more info.


# Navigation
We use [Prism](https://prismlibrary.com/docs/xamarin-forms/navigation/navigation-basics.html) for navigation.
There's a three-step (plus some setup) process for getting started:

1) Make `App` extend `Prism.DryIoc.PrismApplication`
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

2) Register pages that should be navigable
    * do this in `App`'s RegisterForNavigation
    * the overload of `RegisterForNavigation<TPage, TViewModel>()` shown below handles setting the binding context of the specified page!
```c#
protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    containerRegistry.RegisterForNavigation<SplashPage, SplashPageViewModel>();
    containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
}
```

3) Inject an `INavigationService` into whichever viewmodels will be doing navigation:
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

4) Perform navigation with either relative or absolute paths:
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

## Notes on Prism navigation
* When registering pages for navigation, you _can_ provide a different key/identifier, but using the page/class name seems better
* There are two types of navigation paths
    * __Relative__: subsequent navs will create a stack (e.g. `NavigateAsync("SomeView")`)
    * __Absolute__: prefixing a nav with backslash will reset the stack (e.g. `NavigateAsync("/SomeView")`)
