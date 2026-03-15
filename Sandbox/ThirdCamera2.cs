using UnityEngine;

public class ThirdCamera2 : MonoBehaviour
{
    //インスペクタにTargetという項目を作成
    //ここにプレイヤーのTransformコンポーネントをドラッグドロップで対応させる
    [Header("Target Settings")]
    public Transform target;

    [Header("Position Settings")]
    public float distance = 5.0f;
    public float height = 2.0f;
    public float smoothSpeed = 10.0f;

    [Header("Rotation Settings")]
    public float mouseSensitivity = 3.0f;
    public float minVerticalAngle = -60.0f;
    public float maxVerticalAngle = 60.0f;

    private float rotation_x = 0.0f;
    private float rotation_y = 0.0f;
    private Vector3 smoothVelocity = Vector3.zero;

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

        // 初期角度を設定
        rotation_y = target.eulerAngles.y;
    }

    private void LateUpdate()
    {
        // マウス入力による回転
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 縦の角度を制限
        rotation_x -= mouseY; // 反転して直感的な動きに
        rotation_x = Mathf.Clamp(rotation_x, minVerticalAngle, maxVerticalAngle);

        // 横の角度を更新
        rotation_y += mouseX;

        // 回転を適用
        Quaternion rotation = Quaternion.Euler(rotation_x, rotation_y, 0);

        // カメラの位置を計算
        Vector3 targetPosition = target.position - (rotation * Vector3.forward * distance);

        // スムーズに移動
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 1 / smoothSpeed);

        // カメラの回転を直接設定（LookAtを使わない）
        transform.rotation = rotation;
    }
}