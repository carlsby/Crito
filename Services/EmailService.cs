using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using static Lucene.Net.Index.TermsEnum;
using Crito.Models;
using Crito.Contexts;

namespace Crito.Services;

public class EmailService : IDisposable
{
    private string _from;
    private string _smtp;
    private int _port;
    private string _username;
    private string _password;
    private MailKit.Net.Smtp.SmtpClient _client;
    private readonly DataContext _context;

    public EmailService(string from, string smtp, int port, string username, string password, DataContext context)
    {
        _from = from;
        _smtp = smtp;
        _port = port;
        _username = username;
        _password = password;
        _client = new MailKit.Net.Smtp.SmtpClient();
        _context = context;
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

    public async Task AddEmailAsync(ContactForm form)
    {
        _context.Contact.Add(new ContactEntity { Email = form.Email, Name = form.Name, Message = form.Message, Id = form.Id });
        await _context.SaveChangesAsync();
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
