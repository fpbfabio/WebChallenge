using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace RentApp.Web.Components.Core;

public class RazorBase<T> : ComponentBase, IDisposable where T : INotifyPropertyChanged
{
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
    }

    async void OnPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
    {
        await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        ViewModel.PropertyChanged -= OnPropertyChangedHandler;
    }
}
