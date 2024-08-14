using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RentApp.Web.Components.Core;

public abstract class ViewModelBase : IViewModelBase
{
    public required Action<string> NavigateTo { protected get; set; }
    public required Action<string> Notify { protected get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual void OnInitialized() {}

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void SetValue<T>(ref T backingFiled, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingFiled, value)) return;
        backingFiled = value;
        OnPropertyChanged(propertyName);
    }
}