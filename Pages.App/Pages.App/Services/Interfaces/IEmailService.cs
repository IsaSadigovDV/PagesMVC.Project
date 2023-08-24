namespace Pages.App.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendMail(string from, string to, string subject, string text, string link, string name);
    }
}
