using UnityEngine;
using static Unity.VisualScripting.Icons;

public enum Languages
{
    Korean,
    English,
}

public static class DataTableIds
{
    public static readonly string[] StringTableIds =
    {
        "StringTableKr",
        "StringTableEn",
    };
    public static string String => StringTableIds[(int)Variables.Language];

    public static readonly string DragonStat = "DragonStatTable";
}


public static class Variables
{
    public static Languages Language = Languages.Korean;
}
