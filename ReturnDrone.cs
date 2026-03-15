using Unity.VisualScripting;
using UnityEngine;

// ドローンをプレイヤーの頭上にテレポートさせるスクリプト．Rボタンを押すとドローンがプレイヤーの頭上1mの位置に移動します．
//　ReturnDroneオブジェクトにコンポーネントとしてつける

public class ReturnDrone : MonoBehaviour
{

    [Header("Player and Drone Reference")]
    public PlayerController playerController;
    public DroneController droneController;

    [Header("Return Settings")]
    public float returnHeight = 2f;
    public KeyCode returnKey = KeyCode.R;

    private void Start()
    {

    }
    private void Update()
    {
        // RキーでDroneをPlayerに戻す
        if (Input.GetKeyDown(returnKey))
        {
            Debug.Log("ReturnDrone: Rキーが押されました。");
            if (playerController != null && droneController != null)
            {
                Debug.Log("ReturnDrone: PlayerController と DroneController が見つかりました。");
                // プレイヤーの頭上1mの位置を計算
                Vector3 returnPosition = playerController.transform.position + Vector3.up * returnHeight;
                Debug.Log("ReturnDrone: returnPosition:"+ returnPosition);
                // ドローンをその位置にテレポート
                if (droneController.isActive){
                    droneController.returnDestination = returnPosition;
                    droneController.returnDrone = true;
                }
                else
                {
                    droneController.transform.position = returnPosition;
                }
            }
        }
    }
}