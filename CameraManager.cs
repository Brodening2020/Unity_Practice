using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera References")]
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public Transform player;

    [Header("Components")]
    public FirstPersonCamera firstPersonCameraScript;
    public ThirdPersonCamera thirdPersonCameraScript;
    public Kirby_Controller playerController;

    [Header("Settings")]
    public KeyCode switchCameraKey = KeyCode.V;

    private bool isFirstPersonView = false;

    void Start()
    {
        // コンポーネントが割り当てられていない場合は自動的に検索
        if (firstPersonCamera == null || thirdPersonCamera == null)
        {
            Camera[] cameras = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);
            foreach (Camera cam in cameras)
            {
                if (cam.GetComponent<FirstPersonCamera>() != null)
                    firstPersonCamera = cam;
                else if (cam.GetComponent<ThirdPersonCamera>() != null)
                    thirdPersonCamera = cam;
            }
        }

        // スクリプト参照の取得
        if (firstPersonCamera != null)
            firstPersonCameraScript = firstPersonCamera.GetComponent<FirstPersonCamera>();

        if (thirdPersonCamera != null)
            thirdPersonCameraScript = thirdPersonCamera.GetComponent<ThirdPersonCamera>();

        if (player != null && playerController == null)
            playerController = player.GetComponent<Kirby_Controller>();

        // デフォルトは3人称視点から開始
        SwitchToThirdPerson();
    }

    void Update()
    {
        // カメラ切り替えキーが押されたら視点を切り替え
        if (Input.GetKeyDown(switchCameraKey))
        {
            if (isFirstPersonView)
                SwitchToThirdPerson();
            else
                SwitchToFirstPerson();
        }
    }

    void SwitchToFirstPerson()
    {
        isFirstPersonView = true;

        // カメラの有効/無効を切り替え
        if (firstPersonCamera != null)
            firstPersonCamera.enabled = true;
        if (thirdPersonCamera != null)
            thirdPersonCamera.enabled = false;

        // スクリプトの有効/無効を切り替え
        if (firstPersonCameraScript != null)
            firstPersonCameraScript.enabled = true;
        if (thirdPersonCameraScript != null)
            thirdPersonCameraScript.enabled = false;

        // コントローラーにカメラ情報を更新
        if (playerController != null)
            playerController.SetActiveCamera(firstPersonCamera.transform);

        Debug.Log("1人称視点に切り替えました");
    }

    void SwitchToThirdPerson()
    {
        isFirstPersonView = false;

        // カメラの有効/無効を切り替え
        if (firstPersonCamera != null)
            firstPersonCamera.enabled = false;
        if (thirdPersonCamera != null)
            thirdPersonCamera.enabled = true;

        // スクリプトの有効/無効を切り替え
        if (firstPersonCameraScript != null)
            firstPersonCameraScript.enabled = false;
        if (thirdPersonCameraScript != null)
            thirdPersonCameraScript.enabled = true;

        // コントローラーにカメラ情報を更新
        if (playerController != null)
            playerController.SetActiveCamera(thirdPersonCamera.transform);

        Debug.Log("3人称視点に切り替えました");
    }
}