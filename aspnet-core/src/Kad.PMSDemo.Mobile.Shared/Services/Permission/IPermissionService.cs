namespace Kad.PMSDemo.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}