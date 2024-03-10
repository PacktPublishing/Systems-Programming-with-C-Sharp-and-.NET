using System.Net;
using System.Net.Mail;

// Create the mail message
var mail = new MailMessage();
mail.From = new MailAddress("dennis@vroegop.org");
mail.To.Add("dearreader@thisbook.com");
mail.Subject = "Hi there System Programmer!";
mail.Body =
    "This is a test email from the System Programming book.";


// Create a multipart message

var multipartMail = new MailMessage();
multipartMail.From = new MailAddress("dennis@vroegop.org");
multipartMail.To.Add("dearreader@thisbook.com");
multipartMail.Subject = "Hi there System Programmer!";

var htmlBody = "<html><body><h1>Hi there System Programmer!</h1></body></html>";

var htmlView =
    AlternateView.CreateAlternateViewFromString(
        htmlBody,
        null,
        "text/html");

var plainView =
    AlternateView.CreateAlternateViewFromString(
        "This is a test email from the System Programming book.",
        null,
        "text/plain");

multipartMail.AlternateViews.Add(plainView);
multipartMail.AlternateViews.Add(htmlView);


// Set up the connection to the SMTP server
// And no, this is NOT a valid SMTP server. Use your own :-)
var client =
    new SmtpClient("smtp.vroegop.org");
client.Port = 587;
client.EnableSsl = true;
client.Credentials =
    new NetworkCredential(
        "dennis@vroegop.org",
        "MySuperSecretPassword");

// Send the email!
client.Send(mail);
client.Send(multipartMail);