using System.Threading.Tasks;

namespace TestCedro.Infra.CrossCutting.Helpers
{
    public class EmailFactory
    {
        public string FromAddress { get; set; } = Constants.Strings.Emails.Sender;
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; } = true;

        public async Task<EmailFactory> EmailForgotPassword(string toAddress, string resetToken, string resetUrl)
        {
            ToAddress = toAddress;
            Subject = $"{Constants.Strings.System.Name} - Solicitação de redefinição de senha";
            Body = string.Format("Use este token de redefinição de senha para redefinir sua senha. <br/>O token é: {0}<br/>Visite <a href='{1}'>{1}</a> para redefinir sua senha.<br/>",
                resetToken, resetUrl);
            return await Task.FromResult(this);
        }

        public async Task<EmailFactory> EmailSignup(string toAddress, string confirmationUrl)
        {
            ToAddress = toAddress;
            Subject = $"{Constants.Strings.System.Name} - Bem vindo - Ative sua conta";
            Body = $"Obrigado por se registrar, mas primeiro você precisa confirmar seu cadastro, clique <a href=\"{confirmationUrl}\">AQUI</a> para ativar sua conta.";
            return await Task.FromResult(this);
        }
    }
}