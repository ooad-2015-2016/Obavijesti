using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Contacts;
using Windows.Storage;

namespace MobileProjekat.OoadMobile.CallISMS.Helper
{
    public class EmailCommunicateBehaviour : ICommunicateBehaviour
    {

        String textPoruke = "Trolololo";

        public async void Communicate(Contact kontakt)
        {
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            emailMessage.Body = textPoruke;
            StorageFile attachmentFile = null;
            if (attachmentFile != null)
            {
                var stream = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromFile(attachmentFile);
                var attachment = new Windows.ApplicationModel.Email.EmailAttachment(
                    attachmentFile.Name,
                    stream);
                emailMessage.Attachments.Add(attachment);
            }
            var email = kontakt.Emails.FirstOrDefault<ContactEmail>();
            if (email != null)
            {
                var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(email.Address);
                emailMessage.To.Add(emailRecipient);
            }
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        public string dajMetodKomunikacije()
        {
            return "Email";
        }
    }
}
