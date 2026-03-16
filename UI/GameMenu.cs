using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.CompilerServices;


public class GameMenu : MonoBehaviour
{
    public CharacterController playerController;
    
    public ShowGameMenu showGameMenu;
    public GameObject ShowGameMenuObject;
    public GameObject KeyBoardSettings;

    public void Start()
    {
        string path = Application.persistentDataPath;
        Debug.Log(path);
        LoadGame();
    }

    // ゲームメニューから，キーボード設定画面に遷移する
    public void ShowKeyBoardSettings()
    {
        showGameMenu.menu.SetActive(false);
        ShowGameMenuObject.SetActive(false);
        KeyBoardSettings.SetActive(true);
    }

    public void SaveGame()
    {
        SaveSystem.Save(playerController.transform);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.Load();

        if (data != null)
        {
            Vector3 savedPosition = new Vector3(
                data.playerX,
                data.playerY + 1.0f, //ロード時にプレイヤーが埋まらないようにY座標を少し上げる
                data.playerZ
            );
            Vector3 moveVector = savedPosition - playerController.transform.position;
            playerController.Move(moveVector);
        }
    }

    public void ReturnTitle(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
