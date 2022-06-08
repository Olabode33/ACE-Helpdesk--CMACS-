using System.Collections.Generic;
using MvvmHelpers;
using Kad.PMSDemo.Models.NavigationMenu;

namespace Kad.PMSDemo.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}