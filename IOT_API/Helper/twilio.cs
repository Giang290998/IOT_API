using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace IOT_API.Helper;

public static class TWILIO
{
    public static void SendSMS(string phone, string message_content)
    {
        // Find your Account SID and Auth Token at twilio.com/console
        // and set the environment variables. See http://twil.io/secure
        string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID") ?? "TWILIO_ACCOUNT_SID";
        string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN") ?? "TWILIO_AUTH_TOKEN";

        TwilioClient.Init(accountSid, authToken);

        var message = MessageResource.Create(
            body: message_content,
            from: new Twilio.Types.PhoneNumber(Environment.GetEnvironmentVariable("TWILIO_PHONE")),
            to: new Twilio.Types.PhoneNumber($"+84{phone[1..]}")
        );
    }
}
