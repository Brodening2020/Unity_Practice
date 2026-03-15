using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public void NewGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Continue(string sceneName)
    {
        //SaveSystem.Load();
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}