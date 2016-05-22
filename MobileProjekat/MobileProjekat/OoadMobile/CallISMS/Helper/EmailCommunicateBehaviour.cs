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
    //posalje korisniku Email (tj pozove api za to ovisno koji mail client korisnik iskoristi)
    public class EmailCommunicateBehaviour : ICommunicateBehaviour
    {
        //template poruke, nemam ideja
        String textPoruke = "Trolololo";

        public async void Communicate(Contact kontakt)
        {
            //Email Message Objekat
            var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
            //tekst poruke
            emailMessage.Body = textPoruke;
            StorageFile attachmentFile = null;
            //ako neko hoce da doda attachment na poruku moze ucitati file i proslijediti ga ovako
            //ovo se nikad nece pozvati, samo kao primjer kako se attachment doda
            if (attachmentFile != null)
            {
                var stream = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromFile(attachmentFile);
                var attachment = new Windows.ApplicationModel.Email.EmailAttachment(
                    attachmentFile.Name,
                    stream);
                emailMessage.Attachments.Add(attachment);
            }
            //prvi mail koji se nadje
            var email = kontakt.Emails.FirstOrDefault<ContactEmail>();
            if (email != null)
            {
                //postaviti kao recipient
                var emailRecipient = new Windows.ApplicationModel.Email.EmailRecipient(email.Address);
                //lista, moze se dodati vise kontakata
                emailMessage.To.Add(emailRecipient);
            }
            //poziva napokon api sa spremnom template porukom. Korisnik je naknadno moze izmjeniti koristeci mail client
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        public string dajMetodKomunikacije()
        {
            return "Email";
        }
    }
}
