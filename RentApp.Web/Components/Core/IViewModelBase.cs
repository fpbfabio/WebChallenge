using System.ComponentModel;

namespace RentApp.Web.Components.Core;

public interface IViewModelBase : INotifyPropertyChanged
{
    public Action<string> NavigateTo { set; protected get; }
    void OnInitialized();
}