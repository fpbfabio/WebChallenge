using RentApp.Web.Components.Features.Interfaces;

namespace RentApp.Web.Components.Features.Common;

public class EnforceDriverProfile
{
    private const string NO_PROFILE_REDIRECT_PAGE = "/register";
    private const string ERRO_REDIRECT_PAGE = "/error";
    public required IDriverProfileGateway Gateway { init; private get; }

    public void EnsureProfileExists(string id, Action handleExistingProfile, Action<string> handleRedirect)
    {
        Gateway.ProfileExists(id, (exists) =>
        {
            if (exists)
            {
                handleExistingProfile();
            }
            else
            {
                handleRedirect(NO_PROFILE_REDIRECT_PAGE);
            }
        }, (s) =>
        {
            Console.WriteLine("Error at EnforceUserHasProfile.HasProfile : " + s);
            handleRedirect(ERRO_REDIRECT_PAGE);
        });
    }
}