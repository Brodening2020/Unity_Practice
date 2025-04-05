using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //インスペクタにTargetという欄を作成
    //ここにプレイヤーのTransformコンポーネントをドラグドロップで対応させる
    [Header("Target Settings")]
    public Transform target;

    [Header("Position Settings")]
    public float distance = 5.0f;
    public float height = 2.0f;
    public float smoothSpeed = 10.0f;

    //マウス感度が高すぎるとズームインしてしまうような問題が発生
    //カメラの位置移動の際に補完が間に合ってないのかも？
    [Header("Rotation Settings")]
    public float mouseSensitivity = 0.5f;
    public float minVerticalAngle = -60.0f;
    public float maxVerticalAngle = 60.0f;

    //地面や壁オブジェクト等を「CameraBlockable」というレイヤーマスクに登録しておく
    //するとそこだけRaycastでカメラ埋まり判定を調べてくれる
    [Header("BlockageCheck Settings")]
    public LayerMask CameraBlockable_LayerMask;

    [Header("Zoom Settings")]
    public float zoomSensitivity = 0.3f;

    private float rotation_x = 0.0f;
    private float rotation_y = 0.0f;

    private float zoom_level = 0.0f;
    private float zoomed_distance;

    RaycastHit hit;

    private void Start()
    {

        // カーソルをロック
        Cursor.lockState = CursorLockMode.Locked;

        if (target == null)
        {
            Debug.LogError("Third Person Camera: ターゲットが設定されていません！");
            enabled = false;
            return;
        }

        // 横回転の初期角度を設定　プレイヤーの視線と合わせる
        rotation_y = target.eulerAngles.y;

        //拡大縮小を初期化
        zoomed_distance = distance;
        
    }

    private void LateUpdate()
    {
        Zoom();

        // マウス入力による回転 0.1かけないと感度高杉
        float mouseX = Input.GetAxis("Mouse X") * 0.1f * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * 0.1f * mouseSensitivity;

        // 縦の角度を更新
        rotation_x -= mouseY;
        rotation_x = Mathf.Clamp(rotation_x, minVerticalAngle, maxVerticalAngle);

        // 横の角度を更新
        rotation_y += mouseX;

        // カメラの移動先ポジションを計算
        Vector3 targetPosition = Vector3.zero;
        targetPosition.x = target.position.x - (zoomed_distance * Mathf.Cos(rotation_y));
        targetPosition.z = target.position.z - (zoomed_distance * Mathf.Sin(rotation_y));
        targetPosition.y = target.position.y + height + (zoomed_distance * Mathf.Sin(rotation_x));
        
        
        
        //移動 この時スムージングをするとズームインするような変な挙動が発生
        transform.position = targetPosition;

        //カメラが地面に埋まることを防ぐ　地面とぶつかったらその上にカメラが来るように
        if(Physics.Linecast(transform.position, target.position, out hit, CameraBlockable_LayerMask))
        {
            transform.position = hit.point+ Vector3.up * 0.1f;
        }



        //ここまではカメラの位置の調整　向きではない
        // カメラをターゲットに向ける
        transform.LookAt(target.position + Vector3.up * 1.0f); // プレイヤーの足元より少し上を見るように調整

        //拡大縮小
        void Zoom()
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            Debug.Log(zoom);
            zoom_level += zoom * zoomSensitivity;
            zoom_level = Mathf.Clamp(zoom_level, 1.0f, -1.0f);

            if (zoom_level > 0)
            {
                zoomed_distance = (1-distance) * zoom_level + 1.0f;
            }
            else if (zoom < 0)
            {
                zoomed_distance = -4.0f * distance * zoom_level + distance;
            }
        }


    }
}