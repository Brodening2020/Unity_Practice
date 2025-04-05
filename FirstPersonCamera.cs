using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform firstperson_target;

    [Header("Position Settings")]
    public float offset = 0.5f;
    public float height = 2.0f;

    [Header("Rotation Settings")]
    public float mouseSensitivity = 2.0f;
    public float minVerticalAngle = -60.0f;
    public float maxVerticalAngle = 60.0f;

    private float rotation_x = 0.0f;
    private float rotation_y = 0.0f;

    void Start()
    {
        if (firstperson_target == null)
        {
            Debug.LogError("First Person Camera: ターゲットが設定されていません！");
            enabled = false;
            return;
        }

        // カーソルをロック
        Cursor.lockState = CursorLockMode.Locked;

        // 初期回転角度の設定
        rotation_y = firstperson_target.eulerAngles.y;

        // カメラ位置の初期設定
        UpdateCameraPosition();
    }

    void Update()
    {
        if (firstperson_target == null)
            return;

        // マウス入力による回転
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 縦の角度を更新
        rotation_x -= mouseY;
        rotation_x = Mathf.Clamp(rotation_x, minVerticalAngle, maxVerticalAngle);

        // 横の角度を更新
        rotation_y += mouseX;

        // カメラの回転を設定
        transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);

        // カメラ位置の更新
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // 1人称視点なのでプレイヤーの目の位置に固定
        transform.position = firstperson_target.position + new Vector3(0, height, 0);
    }
}