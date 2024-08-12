namespace RentApp.Web.Components.Core;

public interface IAuthViewModelBase : IViewModelBase
{
    Func<bool> IsUserAuthenticated { set; protected get; }
    Func<string> GetUserId { set; protected get; }
    void OnAuthInitialized();
}