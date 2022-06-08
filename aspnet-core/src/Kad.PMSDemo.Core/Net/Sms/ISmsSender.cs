using System.Threading.Tasks;

namespace Kad.PMSDemo.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}