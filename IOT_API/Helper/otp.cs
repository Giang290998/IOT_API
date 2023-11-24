using OtpNet;

namespace IOT_API.Helper;

public static class OTP
{
    public static string CreateKey()
    {
        var key = KeyGeneration.GenerateRandomKey(20);
        var key_string = Base32Encoding.ToString(key);
        return key_string;
    }

    public static string Create(string key_string)
    {
        var base32Bytes = Base32Encoding.ToBytes(key_string);
        var totp = new Totp(base32Bytes);
        var otp = totp.ComputeTotp(DateTime.UtcNow);
        Console.WriteLine($"Time remain: {totp.RemainingSeconds()}");
        return otp;


        // var base32Bytes = Base32Encoding.ToBytes(key_string);
        // // var totp = new Totp(base32Bytes, step: 30);
        // var totp = new Totp(base32Bytes, timeCorrection: new TimeCorrection(DateTime.UtcNow.AddSeconds(40)));
        // var otp = totp.ComputeTotp();
        // return otp;
    }

    public static bool Verify(string key_string, string user_provide_otp)
    {
        var keyBytes = Base32Encoding.ToBytes(key_string);

        var totp = new Totp(keyBytes);

        if (totp.VerifyTotp(user_provide_otp, out long timeStepMatched))
        {
            Console.WriteLine($"Mã OTP hợp lệ. Time remain: {totp.RemainingSeconds()}");
            return true;
        }
        else
        {
            Console.WriteLine("Mã OTP không hợp lệ.");
            return false;
        }
    }
}