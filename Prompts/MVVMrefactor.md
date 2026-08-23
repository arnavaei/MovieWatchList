# Refactor This Blazor Project to MVVM

You are a senior .NET / C# / Blazor architect.

I have an existing Blazor project. I want you to refactor it into a clean, maintainable **MVVM (Model–View–ViewModel)** architecture without breaking existing functionality.

## 1. First: Analyze, Do NOT Modify

Before changing any code:

1. Inspect the entire project structure.
2. Identify:

   * Blazor components/pages
   * Models
   * Services
   * Business logic
   * API calls
   * State management
   * Dependency injection
   * Event handlers
   * Forms and validation
   * Navigation
   * Component parameters
   * JavaScript interop
   * Existing ViewModels, if any
3. Determine where business/application logic currently lives.
4. Identify code that is incorrectly placed inside `.razor` files.
5. Identify duplicated logic.
6. Identify components that contain too much responsibility.
7. Identify dependencies between components.
8. Determine whether the project is Blazor WebAssembly, Blazor Server, or Blazor Web App and respect the existing hosting model.

Do NOT immediately start refactoring.

First give me:

* Current architecture
* Problems you found
* Recommended MVVM architecture
* Proposed folder structure
* Mapping of existing files → new locations/types
* Potential risks
* Migration plan

Wait for my approval before performing a large-scale refactor.

---

# 2. Target Architecture

The target architecture should follow this general structure:

```text
Project/
│
├── Components/
│   ├── Pages/
│   │   └── ...
│   │
│   └── Shared/
│       └── ...
│
├── Models/
│   ├── ...
│
├── ViewModels/
│   ├── Base/
│   │   └── ViewModelBase.cs
│   │
│   ├── Pages/
│   │   └── ...
│   │
│   └── Components/
│       └── ...
│
├── Services/
│   ├── Interfaces/
│   │   └── ...
│   └── Implementations/
│       └── ...
│
├── Repositories/
│   ├── Interfaces/
│   │   └── ...
│   └── Implementations/
│       └── ...
│
├── DTOs/
│   └── ...
│
├── Mappers/
│   └── ...
│
├── Validators/
│   └── ...
│
├── Extensions/
│   └── ...
│
└── Program.cs
```

Adapt this structure to the actual project.

Do NOT create unnecessary layers just for the sake of following a textbook architecture.

The architecture should remain pragmatic for a Blazor application.

---

# 3. MVVM Responsibilities

Follow these responsibilities strictly.

## Model

Models represent application/domain data.

Models should NOT:

* Manipulate the UI
* Access Blazor components
* Perform navigation
* Contain UI state
* Directly depend on Razor components

Examples:

```csharp
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
```

Keep models focused on data/domain representation.

---

# 4. View

The View is the `.razor` component.

The View should primarily be responsible for:

* Rendering UI
* Binding UI elements
* Displaying ViewModel state
* Calling ViewModel commands/actions
* Handling component-specific UI concerns

Avoid putting business logic inside `.razor` files.

Bad:

```razor
@code {
    private async Task DeleteMovie(Movie movie)
    {
        var response = await Http.GetAsync(...);

        if (response.IsSuccessStatusCode)
        {
            Movies.Remove(movie);
        }
    }
}
```

Prefer:

```razor
<button @onclick="() => ViewModel.DeleteMovieAsync(movie)">
    Delete
</button>
```

The View should not know how the operation is implemented.

---

# 5. ViewModel

ViewModels are the primary presentation/application logic layer.

A ViewModel should:

* Expose data required by the View
* Expose UI state
* Execute commands/actions
* Call services
* Handle loading states
* Handle errors
* Handle validation
* Coordinate application operations
* Transform service results into presentation-friendly data
* Notify the View when state changes when necessary

Example:

```csharp
public class MovieListViewModel : ViewModelBase
{
    private readonly IMovieService _movieService;

    public IReadOnlyList<Movie> Movies { get; private set; } = [];

    public bool IsLoading { get; private set; }

    public string? ErrorMessage { get; private set; }

    public MovieListViewModel(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            Movies = await _movieService.GetMoviesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }

        OnPropertyChanged(nameof(Movies));
        OnPropertyChanged(nameof(IsLoading));
        OnPropertyChanged(nameof(ErrorMessage));
    }
}
```

Do not copy this blindly. Adapt the implementation to the project.

---

# 6. ViewModelBase

Create a reusable base class where appropriate.

Prefer a lightweight implementation such as:

```csharp
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
```

If the project already uses an MVVM framework or library, evaluate whether it should be retained rather than introducing another dependency.

Do NOT add CommunityToolkit.Mvvm or another MVVM framework automatically.

Only introduce a library if there is a clear architectural benefit.

---

# 7. Services

Services should contain reusable application/business operations.

For example:

```csharp
public interface IMovieService
{
    Task<IReadOnlyList<Movie>> GetMoviesAsync();
    Task<Movie?> GetMovieAsync(int id);
    Task AddMovieAsync(Movie movie);
    Task DeleteMovieAsync(int id);
}
```

Implementation:

```csharp
public class MovieService : IMovieService
{
    // Implementation
}
```

ViewModels should depend on abstractions:

```csharp
private readonly IMovieService _movieService;
```

Avoid:

```csharp
private readonly MovieService _movieService;
```

unless there is a specific reason.

---

# 8. Dependency Injection

Register ViewModels and services using the existing DI system.

For example:

```csharp
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<MovieListViewModel>();
```

Choose `Scoped`, `Transient`, or `Singleton` based on the actual lifecycle requirements.

Do not blindly register everything as Singleton.

Pay particular attention to Blazor WebAssembly versus Blazor Server/Web App lifecycle behavior.

---

# 9. Razor Components

Refactor Razor components so that they become thin Views.

Prefer:

```razor
@page "/movies"
@inject MovieListViewModel ViewModel

<h1>Movies</h1>

@if (ViewModel.IsLoading)
{
    <p>Loading...</p>
}
else if (!string.IsNullOrEmpty(ViewModel.ErrorMessage))
{
    <p>@ViewModel.ErrorMessage</p>
}
else
{
    @foreach (var movie in ViewModel.Movies)
    {
        <MovieCard Movie="movie" />
    }
}

@code {
    protected override async Task OnInitializedAsync()
    {
        await ViewModel.LoadAsync();
    }
}
```

The `@code` section should remain minimal.

Do not move every single line from `.razor` into a ViewModel mechanically.

Blazor lifecycle-specific behavior can remain in the View when appropriate.

---

# 10. Commands / User Actions

For user actions such as:

* Add
* Edit
* Delete
* Search
* Filter
* Sort
* Submit
* Save
* Cancel
* Refresh

Prefer ViewModel methods.

Example:

```csharp
public async Task DeleteMovieAsync(int movieId)
{
    await _movieService.DeleteMovieAsync(movieId);
    await LoadAsync();
}
```

The View:

```razor
<button @onclick="() => ViewModel.DeleteMovieAsync(movie.Id)">
    Delete
</button>
```

Do not put business logic inside the event handler.

---

# 11. UI State

Move page-level UI state into the ViewModel when it represents presentation state.

Examples:

```csharp
public bool IsLoading { get; private set; }

public bool IsSaving { get; private set; }

public bool IsDeleteDialogOpen { get; private set; }

public string SearchTerm { get; set; } = string.Empty;

public string? ErrorMessage { get; private set; }

public string? SuccessMessage { get; private set; }
```

However, keep purely local visual concerns in the View when appropriate.

Do not turn every CSS class or UI detail into ViewModel state.

---

# 12. Forms and Validation

Inspect all forms.

Move business/application validation into appropriate validation mechanisms.

Do not duplicate validation between View and ViewModel unnecessarily.

Use:

* DataAnnotations when appropriate
* Custom validators when necessary
* Service/domain validation for business rules

The View should be responsible for displaying validation feedback.

---

# 13. API / HTTP Calls

If HTTP/API calls currently exist inside Razor components, move them out.

Bad:

```razor
@inject HttpClient HttpClient

@code {
    private async Task LoadMovies()
    {
        Movies = await HttpClient.GetFromJsonAsync<List<Movie>>("api/movies");
    }
}
```

Prefer:

```text
View
 ↓
ViewModel
 ↓
Service
 ↓
HttpClient/API
```

For example:

```csharp
MovieListViewModel
        ↓
IMovieService
        ↓
MovieService
        ↓
HttpClient
        ↓
API
```

---

# 14. Navigation

Do not put navigation logic into Models or Services unless navigation is genuinely part of an application workflow.

ViewModels may coordinate navigation when appropriate, but avoid tightly coupling business logic to Blazor's `NavigationManager`.

If necessary, introduce an abstraction such as:

```csharp
public interface INavigationService
{
    void NavigateTo(string uri);
}
```

Only introduce this abstraction if it provides a real architectural benefit.

Do not over-engineer.

---

# 15. Component Communication

Inspect:

* `[Parameter]`
* `[Parameter] EventCallback`
* Cascading parameters
* Shared state
* Parent/child communication

Do not force every component into MVVM.

Reusable presentational components can remain simple Razor components.

For example:

```text
MovieListPage
    ↓
MovieListViewModel
    ↓
MovieCard
```

`MovieCard` does not necessarily need its own ViewModel.

Use a ViewModel when the component has meaningful presentation/application logic.

---

# 16. Async Code

Follow modern C# async practices.

Prefer:

```csharp
Task
Task<T>
async/await
CancellationToken
```

where appropriate.

Avoid:

```csharp
async void
.Result
.Wait()
Task.Run()
```

unless there is a specific and justified reason.

Consider cancellation for long-running operations.

---

# 17. Error Handling

Do not silently swallow exceptions.

Bad:

```csharp
catch
{
}
```

Prefer:

```csharp
catch (Exception ex)
{
    ErrorMessage = "Unable to load movies.";
    // Log exception appropriately
}
```

Do not expose sensitive exception information directly to users.

Use the project's existing logging infrastructure where available.

---

# 18. State Notifications

If ViewModels use `INotifyPropertyChanged`, ensure state changes actually cause the Blazor View to update.

Do not assume that implementing `INotifyPropertyChanged` automatically makes Blazor rerender.

If necessary, establish a clean mechanism between the ViewModel and component lifecycle.

Keep this mechanism simple and avoid unnecessary event subscriptions or memory leaks.

---

# 19. Existing Functionality Must Be Preserved

This is extremely important.

The refactor must NOT change:

* Existing UI behavior
* Existing routes
* Existing navigation
* Existing API contracts
* Existing business rules
* Existing data structures unless necessary
* Existing user flows
* Existing validation behavior
* Existing CRUD functionality

This is an architectural refactor, not a redesign.

If something must change to support the architecture, explain why before changing it.

---

# 20. Do Not Over-Engineer

Avoid creating abstractions that have no practical purpose.

Do NOT automatically create:

```text
Repository
UnitOfWork
Factory
Mediator
CQRS
EventBus
GenericService
GenericRepository
BaseRepository<T>
BaseViewModel<T>
```

unless the existing application actually benefits from them.

The goal is:

**Clean MVVM, not maximum abstraction.**

---

# 21. Naming Conventions

Follow standard C# naming conventions.

Examples:

```text
MovieListViewModel
MovieDetailsViewModel
IMovieService
MovieService
Movie
MovieDto
```

Methods:

```text
LoadAsync()
SaveAsync()
DeleteAsync()
SearchAsync()
```

Properties:

```text
Movies
IsLoading
ErrorMessage
SelectedMovie
```

Avoid names such as:

```text
MovieVM
MovieVMClass
MovieManager
Helper
Utils
CommonStuff
```

unless they are genuinely appropriate.

---

# 22. Testing

After refactoring, identify which ViewModel and Service logic can be unit tested.

Prioritize testing:

* ViewModel commands
* State transitions
* Loading states
* Error handling
* CRUD operations
* Validation
* Search/filter/sort logic

Do not attempt to unit-test markup unnecessarily.

---

# 23. Migration Strategy

Refactor incrementally.

For each feature/page:

1. Identify the current View.
2. Identify its logic.
3. Identify required Models/DTOs.
4. Identify required Services.
5. Create the ViewModel.
6. Move appropriate logic into the ViewModel.
7. Inject dependencies.
8. Simplify the Razor component.
9. Build the project.
10. Fix compilation errors.
11. Verify behavior.
12. Move to the next feature.

Do not perform a giant blind rewrite of the entire application.

---

# 24. Build Verification

After every meaningful migration step:

* Build the project.
* Check compiler errors.
* Check nullable warnings.
* Check dependency injection registrations.
* Check component references.
* Check routes.
* Check runtime behavior where possible.

Do not tell me the refactor is complete if the project does not compile.

---

# 25. Final Architecture Report

When the refactor is complete, provide:

### Architecture

Explain the final architecture:

```text
View
 ↓
ViewModel
 ↓
Service
 ↓
Repository/API
 ↓
Data Source
```

### Files Changed

List:

* Created files
* Modified files
* Deleted files

### MVVM Mapping

For each major feature:

```text
View:
    Components/Pages/Movies.razor

ViewModel:
    ViewModels/Pages/MovieListViewModel.cs

Model:
    Models/Movie.cs

Service:
    Services/Interfaces/IMovieService.cs
    Services/Implementations/MovieService.cs
```

### Remaining Technical Debt

Clearly identify anything that could not be migrated cleanly.

### Verification

Report:

* Build status
* Errors
* Warnings
* Tests
* Known issues

---

# Critical Rules

1. **Analyze before modifying.**
2. **Do not blindly move code.**
3. **Do not break existing functionality.**
4. **Keep Razor Views thin.**
5. **Keep business/application logic out of Views.**
6. **ViewModels coordinate presentation/application behavior.**
7. **Services handle reusable application operations.**
8. **Models represent data/domain concepts.**
9. **Use dependency injection.**
10. **Prefer interfaces at architectural boundaries.**
11. **Do not over-engineer.**
12. **Do not introduce unnecessary third-party libraries.**
13. **Use modern C# and .NET practices.**
14. **Build after each meaningful migration.**
15. **If you encounter ambiguity, inspect the surrounding code before making assumptions.**
16. **If a proposed architectural change could alter behavior, stop and explain it before proceeding.**
17. **Never claim success without verifying that the project builds.**

## Start Now

Your first task is ONLY to analyze the existing project.

Do not modify files yet.

Return:

1. Current architecture
2. Current project structure
3. MVVM violations
4. Proposed architecture
5. Proposed folder structure
6. File-by-file migration plan
7. Risks and potential breaking changes
8. Recommended migration order

Then wait for approval before making changes.