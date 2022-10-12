namespace ToyParser.Models;

public class Region
{
    public string Name { get; }
    public string Cookie { get; }

    private Region(string name, string cookie)
    {
        Name = name;
        Cookie = cookie;
    }

    public static Region SaintPetersburg = new Region("Санкт-Петербург", "tmr_detect=0|1665574787237; BITRIX_SM_city=78000000000;");
    public static Region RostovOnDon = new Region("Ростов-на-Дону", "tmr_detect=0|1665574977917; BITRIX_SM_city=61000001000;");
}