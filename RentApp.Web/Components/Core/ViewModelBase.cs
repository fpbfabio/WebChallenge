using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RentApp.Web.Components.Core;

public abstract class ViewModelBase : IViewModelBase
{
    private bool isBusy = false;

    public bool IsBusy
    {
        get => isBusy;
        set
        {
            SetValue(ref isBusy, value);
        }
    }

    public required Action<string> NavigateTo { get; set; }

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