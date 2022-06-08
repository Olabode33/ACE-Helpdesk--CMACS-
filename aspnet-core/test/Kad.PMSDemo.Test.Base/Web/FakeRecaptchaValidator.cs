using System.Threading.Tasks;
using Kad.PMSDemo.Security.Recaptcha;

namespace Kad.PMSDemo.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
