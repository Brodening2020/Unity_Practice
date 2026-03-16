using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public Animator animator;
    // （ぶつかる方の）ドアの当たり判定　ドアが開いているときは当たり判定を無効にする
    public Collider doorCollider;

    bool isOpen = false;

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        animator.SetBool("isOpen", isOpen);

        // 当たり判定
        doorCollider.enabled = !isOpen;
    }
}
