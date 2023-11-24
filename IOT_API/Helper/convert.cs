namespace IOT_API.Helper;

public static class ConvertCustom
{
    public static string ListStringToString(List<string> list)
    {
        string data = "[";
        for (int i = 0; i < list?.Count; i++)
        {
            if (i == list?.Count - 1)
            {
                data += $"'{list?[i]}'";
                break;
            }
            data += $"'{list?[i]}', ";
        }
        data += "]";
        return data;
    }
}