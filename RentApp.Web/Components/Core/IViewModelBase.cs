using System.ComponentModel;

namespace RentApp.Web.Components.Core;

public interface IViewModelBase : INotifyPropertyChanged
{
    public Action<string> Notify { set; }
    public Action<string> NavigateTo { set; }
    void OnInitialized();
}