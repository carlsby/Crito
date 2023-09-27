using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using static Lucene.Net.Index.TermsEnum;

namespace Crito.Services;

public class EmailService : IDisposable
{
    private string _from;
    private string _smtp;
    private int _port;
    private string _username;
    private string _password;
    private MailKit.Net.Smtp.SmtpClient _client;

    public EmailService(string from, string smtp, int port, string username, string password)
    {
        _from = from;
        _smtp = smtp;
        _port = port;
        _username = username;
        _password = password;
        _client = new MailKit.Net.Smtp.SmtpClient();
    }

    public async Task SendAsync(string to, string subject, string body)
    {

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_from));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        await _client.ConnectAsync(_smtp, _port, SecureSocketOptions.StartTls);
        await _client.AuthenticateAsync(_username, _password);

        await _client.SendAsync(email);
        await _client.DisconnectAsync(true);

    }



    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _client.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}
