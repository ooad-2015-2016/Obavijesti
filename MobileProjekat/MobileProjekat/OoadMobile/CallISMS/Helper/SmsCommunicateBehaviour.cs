using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Devices.Sms;

namespace MobileProjekat.OoadMobile.CallISMS.Helper
{
    class SmsCommunicateBehaviour : ICommunicateBehaviour
    {
        private SmsDevice2 device;
        String textPoruke = "Trolololo";

        public async void Communicate(Contact kontakt)
        {
            // If this is the first request, get the default SMS device. If this
            // is the first SMS device access, the user will be prompted to grant
            // access permission for this application.
            if (device == null)
            {
                try
                {
                    device = SmsDevice2.GetDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            if (device != null)
            {
                string msgStr;
                try
                {
                    // Create a text message - set the entered destination number and message text.
                    SmsTextMessage2 msg = new SmsTextMessage2();
                    string telBroj = kontakt.Phones.FirstOrDefault<ContactPhone>().Number;
                    msg.To = telBroj;
                    msg.Body = textPoruke;
                    SmsSendMessageResult result = await device.SendMessageAndGetResultAsync(msg);

                    if (result.IsSuccessful)
                    {
                        msgStr = "";
                        msgStr += "Text message sent, cellularClass: " + result.CellularClass.ToString();
                        IReadOnlyList<Int32> messageReferenceNumbers = result.MessageReferenceNumbers;

                        for (int i = 0; i < messageReferenceNumbers.Count; i++)
                        {
                            msgStr += "\n\t\tMessageReferenceNumber[" + i.ToString() + "]: " + messageReferenceNumbers[i].ToString();
                        }
                    }
                    else
                    {
                        msgStr = "";
                        msgStr += "ModemErrorCode: " + result.ModemErrorCode.ToString();
                        msgStr += "\nIsErrorTransient: " + result.IsErrorTransient.ToString();
                        if (result.ModemErrorCode == SmsModemErrorCode.MessagingNetworkError)
                        {
                            msgStr += "\n\tNetworkCauseCode: " + result.NetworkCauseCode.ToString();

                            if (result.CellularClass == CellularClass.Cdma)
                            {
                                msgStr += "\n\tTransportFailureCause: " + result.TransportFailureCause.ToString();
                            }
                            throw new Exception(msgStr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("Nema SMS device");
            }
        }

        public string dajMetodKomunikacije()
        {
            return "Sms";
        }
    }
}
