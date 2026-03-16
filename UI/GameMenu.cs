using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public Transform player;

    public void SaveGame()
    {
        SaveSystem.Save(player);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.Load();

        if (data != null)
        {
            player.position = new Vector3(
                data.playerX,
                data.playerY,
                data.playerZ
            );
        }
    }

    public void ReturnTitle(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
