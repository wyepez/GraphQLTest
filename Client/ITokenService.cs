using System.Threading.Tasks;

namespace Client
{
    public interface ITokenService
    {
        Task<string> GetToken();
    }
}
