
namespace ClearApplicationFoundation.Services
{
    public interface ILocalizationService
    {
        string Get(string key);

        string this[string key]
        {
            get;
        }
    }
}
