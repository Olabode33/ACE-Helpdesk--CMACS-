using System.Threading.Tasks;
using Kad.PMSDemo.Views;
using Xamarin.Forms;

namespace Kad.PMSDemo.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
