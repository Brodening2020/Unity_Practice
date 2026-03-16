using UnityEngine;

// ドアのInteract判定を行うクラス　このスクリプトをドアのオブジェクトにアタッチする
// ドアにはColliderを2つ作っておく　直接つけるのはInteract判定のColliderで Is Trigger=True
// Interact判定の方はプレイヤーのものよりかなり大きくする必要
// もう1つは1つは子に空オブジェクトを作りドア閉まってる時のの当たり判定とする Is Trigger=False

public class DoorInteractJudge : MonoBehaviour
{

    public DoorManager doorManager;

    private bool canInteract = false;

    public KeyCode toggleKey = KeyCode.L;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(toggleKey))
        {
            doorManager.ToggleDoor();

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