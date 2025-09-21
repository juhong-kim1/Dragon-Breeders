using System.Collections.Generic;
using UnityEngine;

public class StringTable : DataTable
{
    public static readonly string Unknown = "키 없음";

    public class Data
    {
        public string ID { get; set; }
        public string STRING { get; set; }
    }

    private readonly Dictionary<string, string> dictionary = new Dictionary<string, string>();

    public override void Load(string filename)
    {
        dictionary.Clear();

        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"StringTable.Load 실패: {filename} 파일을 찾을 수 없음!");
            return;
        }
        var list = LoadCSV<Data>(textAsset.text);
        foreach (var item in list)
        {
            if (!dictionary.ContainsKey(item.ID))
            {
                dictionary.Add(item.ID, item.STRING);
            }
            else
            {
                Debug.LogError($"키 중복: {item.ID}");
            }
        }
    }

    public string Get(string key)
    {
        if (!dictionary.ContainsKey(key))
        {
            return Unknown;
        }
        return dictionary[key];
    }
}