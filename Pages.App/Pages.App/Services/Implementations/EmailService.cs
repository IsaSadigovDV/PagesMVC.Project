using Pages.App.Services.Interfaces;
using System.Net.Mail;

namespace Pages.App.Services.Implementations
{
    public class EmailService:IEmailService
    {
        private readonly IWebHostEnvironment _env;

        public EmailService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task SendMail(string from, string to, string subject, string text, string link, string name)
        {

            string body = string.Empty;
            string path = Path.Combine(_env.WebRootPath, "templates", "email.html");
            using (StreamReader SourceReader = System.IO.File.OpenText(path))
            {
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{{Link}}", link);
            body = body.Replace("{{Name}}", name);
            body = body.Replace("{{Text}}", text);
            MailMessage mm = new MailMessage();
            mm.To.Add(to);
            mm.Subject = subject;
            mm.Body = body;
            mm.IsBodyHtml = true;
            mm.From = new MailAddress(from);

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(from, "sadiqovisa11");

            await smtp.SendMailAsync(mm);
        }
    }
}
