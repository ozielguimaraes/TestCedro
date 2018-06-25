using System.Threading.Tasks;

namespace TestCedro.Infra.CrossCutting.Helpers.Abstractions.Interfaces
{
    public interface IMessengerService
    {
        bool Send(string from, string to, string subject, string body, bool isBodyHtml);
        Task<bool> SendAsync(string from, string to, string subject, string body, bool isBodyHtml);
    }
}