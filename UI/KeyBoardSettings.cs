using UnityEngine;

public class KeyBoardSettings : MonoBehaviour
{
    public GameObject keyBoardSettings;
    public ShowGameMenu showGameMenu;
    public GameObject ShowGameMenuObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyBoardSettings.SetActive(false);
    }

    public void ReturnGameMenu()
    {
        keyBoardSettings.SetActive(false);
        ShowGameMenuObject.SetActive(true);
        showGameMenu.menu.SetActive(true);
    }
}
