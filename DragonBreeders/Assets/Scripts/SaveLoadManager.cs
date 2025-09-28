using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using SaveDataVC = SaveDataV1;

public class SaveLoadManager
{
    public static int SaveDataVersion { get; } = 1;

    public static SaveDataVC Data { get; set; } = new SaveDataVC();

    private static readonly string[] SaveFileName =
    {
        "SaveAuto.json",
    };

    public static string SaveDirectory => $"{Application.persistentDataPath}/Save";

    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
    };

    public static bool Save(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length)
            return false;

        try
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }

            var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
            var json = JsonConvert.SerializeObject(Data, settings);
            File.WriteAllText(path, json);
            return true;
        }
        catch
        {
            Debug.LogError("Save 예외 발생");
            return false;
        }
    }

    public static bool Load(int slot = 0)
    {
        if (slot < 0 || slot >= SaveFileName.Length)
            return false;
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        if (!File.Exists(path))
            return false;
        try
        {
            var json = File.ReadAllText(path);
            Debug.Log("JSON 읽기 성공");
            Debug.Log($"JSON 내용: {json}");

            var dataSave = JsonConvert.DeserializeObject<SaveDataVC>(json, settings);
            Debug.Log($"역직렬화 결과: {dataSave}");

            if (dataSave == null)
            {
                Debug.LogError("dataSave가 null입니다!");
                return false;
            }

            while (dataSave.Version < SaveDataVersion)
            {
                dataSave = dataSave.VersionUp() as SaveDataVC;
            }
            Data = dataSave;
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load 예외 발생: {e.Message}");
            Debug.LogError($"스택 트레이스: {e.StackTrace}");
            return false;
        }
    }
}

