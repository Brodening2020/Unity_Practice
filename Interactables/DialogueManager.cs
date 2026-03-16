using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    public DialogueInteractJudge dialogueInteractJudge;

    public KeyCode toggleKey = KeyCode.L;

    public string[] sentences;

    int index = 0;
    public bool isTalking = false;
    void Start()
    {
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (isTalking && Input.GetKeyDown(toggleKey))
        {
            NextSentence();
        }
    }

    public void StartDialogue()
    {
        dialogueUI.SetActive(true);
        dialogueInteractJudge.enabled = false; // 対話中はInteract判定を無効にする
        index = 0;
        isTalking = true;

        dialogueText.text = sentences[index];
    }

    void NextSentence()
    {
        index++;

        if (index >= sentences.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = sentences[index];
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
        isTalking = false;
        dialogueInteractJudge.enabled = true; // 対話終了後はInteract判定を有効にする
    }
}
