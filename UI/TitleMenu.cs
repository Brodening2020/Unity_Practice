using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void NewGame(string sceneName)
    {
        SaveSystem.StartNew();
        SceneManager.LoadScene(sceneName);
    }

    public void Continue(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}