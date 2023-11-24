namespace IOT_API.Helper;

public static class Generate
{
    public static string OTP_6()
    {
        Random random = new();
        string otp = random.Next(100000, 999999).ToString();
        return otp;
    }

    public static string OTP_8()
    {
        Random random = new();
        string otp = random.Next(10000000, 99999999).ToString();
        return otp;
    }
}