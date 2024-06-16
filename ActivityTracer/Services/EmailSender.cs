using Castle.Core.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace ActivityTracer.Services
{
	public class EmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
	{
		private readonly IConfiguration _configuration;
		private readonly ILogger<EmailSender> _logger;

		public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
		{
			_configuration = configuration;
			_logger = logger;
		}


		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
			{
				Port = int.Parse(_configuration["Smtp:Port"]),
				Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
				EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"])
			};

			{
				MailMessage message = new MailMessage()
				{
					From = new MailAddress("ilyesbalazs02@gmail.com"),
					Subject = subject,
					IsBodyHtml = true,
					Body = htmlMessage,
					BodyEncoding = System.Text.Encoding.UTF8,
					SubjectEncoding = System.Text.Encoding.UTF8,
				};
				message.To.Add(email);

				try
				{
					await smtpClient.SendMailAsync(message);
					_logger.LogInformation("Email sent to {Email}", email);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error sending email to {Email}", email);
					throw;
				}
			}
		}
	}
}
