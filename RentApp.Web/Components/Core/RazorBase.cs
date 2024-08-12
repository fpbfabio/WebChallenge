using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace RentApp.Web.Components.Core;

public class RazorBase<T> : ComponentBase, IDisposable where T : IViewModelBase
{
    [Inject] public required NavigationManager NavigationManager { init; get; }
    [Inject] public required T ViewModel { init; get; }

    protected override async Task OnInitializedAsync()
    {
        ViewModel.PropertyChanged += async (sender, e) =>
        {
            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        };
        await base.OnInitializedAsync();
        ViewModel.NavigateTo = NavigateTo; 
        ViewModel.OnInitialized();
    }

    async void OnPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
    {
        await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }

    private void NavigateTo(string s)
    {
        NavigationManager.NavigateTo(s);
    }

    public void Dispose()
    {
        ViewModel.PropertyChanged -= OnPropertyChangedHandler;
    }
}
