using Xamarin.Forms.Internals;

namespace Kad.PMSDemo.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}