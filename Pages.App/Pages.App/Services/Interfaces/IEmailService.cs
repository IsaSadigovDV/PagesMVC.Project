namespace Pages.App.Services.Interfaces
{
    public interface IEmailService
    {
        public Task Send(string from, string to, string link, string text, string subject);
    }
}
