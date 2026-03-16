using UnityEngine;

// 看板やドアなどのInteract判定を行うクラス　このスクリプトを看板やドアなどのオブジェクトにアタッチする
public class DialogueInteractJudge : MonoBehaviour
{

    public DialogueManager dialogueManager;

    private bool canInteract = false;

    public KeyCode toggleKey = KeyCode.L;

    void Update()
    {
        Debug.Log("canInteract: " + canInteract);
        if (canInteract && Input.GetKeyDown(toggleKey))
        {
            Debug.Log("Interacted with sign!");
            dialogueManager.StartDialogue();

            // プレイヤー操作停止
            // GetComponent<PlayerMovement>().enabled = false;
        }
    }

    //看板やドアのコライダーに入ったとき、canInteractをtrueにする　コライダーをプレイヤーのものよりかなり大きくする必要
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}