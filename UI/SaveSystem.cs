using UnityEngine;
using System.IO;

public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/save.json";

    public static void StartNew()
    {
        SaveData data = new SaveData();
        data.playerX = 0f;
        data.playerY = 0f;
        data.playerZ = 0f;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

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
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }

        string json = File.ReadAllText(path);
        Debug.Log("Loading save data from " + path);

        return JsonUtility.FromJson<SaveData>(json);
    }
}