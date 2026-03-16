using UnityEngine;
using System.IO;

public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/save.json";

    public static void Save(Transform player)
    {
        SaveData data = new SaveData();
        data.playerX = player.position.x;
        data.playerY = player.position.y;
        data.playerZ = player.position.z;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public static SaveData Load()
    {
        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }
}