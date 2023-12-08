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

    public static List<string> ConvertStringToList(string input)
    {
        // Chia chuỗi thành mảng các chuỗi con sử dụng ", " làm dấu phân cách
        string[] stringArray = input.Split(new string[] { ", " }, StringSplitOptions.None);

        // Chuyển đổi mảng thành danh sách
        List<string> resultList = stringArray.ToList();

        return resultList;
    }
}