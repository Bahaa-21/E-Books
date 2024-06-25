using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Books.BusinessLogicLayer.Abstract;
using E_Books.ViewModel.FromView;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace E_Books.BusinessLogicLayer.Concrete
{
    public class EmailService : IEmailService
    {
        public void SendEmail(EmailDto emailDto)
        {
           MimeMessage email = new();
           email.From.Add(new MailboxAddress("Safa7at Administration" , "admin@sfa7at.com"));
           email.To.Add(MailboxAddress.Parse(emailDto.To));
           email.Subject = emailDto.Subject;
           email.Body = new TextPart(TextFormat.Html) {Text = emailDto.Body};

           using var client = new SmtpClient();
           client.Connect("smtp.ethereal.email" , 587 , MailKit.Security.SecureSocketOptions.StartTls);
           client.Authenticate("korbin84@ethereal.email" , "bpeNurnYhMH58gU3Qz");
           client.Send(email);
           client.Disconnect(true);
        }
    }
}