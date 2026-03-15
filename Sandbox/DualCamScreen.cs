using UnityEngine;

public class DualCamScreen : MonoBehaviour
{
    [Header("Camera References")]
    public Camera playerFirstPersonCamera;
    public Camera playerThirdPersonCamera;
    public Camera droneFirstPersonCamera;
    public Camera droneThirdPersonCamera;

    [Header("PiP Settings")]
    [Tooltip("Initial size of PiP window (0..1)")]
    public float pipWidth = 0.25f;
    public float pipHeight = 0.25f;
    [Tooltip("Minimum PiP size")]
    public float minPipSize = 0.1f;
    [Tooltip("Maximum PiP size")]
    public float maxPipSize = 0.5f;
    [Tooltip("Size adjustment step for + and - keys")]
    public float pipStep = 0.05f;

    private Camera activeMainCamera;
    private Camera activePipCamera;
    private Rect mainRect = new Rect(0f, 0f, 1f, 1f);
    private Vector2[] pipPositions = new Vector2[]
    {
        new Vector2(0.75f, 0.75f), // 右上
        new Vector2(0f, 0.75f),    // 左上
        new Vector2(0.75f, 0f),    // 右下
        new Vector2(0f, 0f)        // 左下
    };
    private int pipPositionIndex = 0;
    private bool isPipEnabled = false;

    private readonly float[] pipKeys = new float[] { 1, 2, 3, 4 };

    private void Start()
    {
        // 初期状態で PiP 無効、各カメラは画面全体か無効にします
        SetupInitialCameras();
    }

    private void Update()
    {
        HandleInput();
    }

    private void SetupInitialCameras()
    {
        // 1つのメインカメラを自動設定（優先順をつける）
        activeMainCamera = playerThirdPersonCamera ?? playerFirstPersonCamera ?? droneThirdPersonCamera ?? droneFirstPersonCamera;

        if (activeMainCamera == null)
        {
            Debug.LogWarning("DualCamScreen: メインカメラが1つも設定されていません。Inspectorでカメラをセットしてください。");
            return;
        }

        if (playerFirstPersonCamera != null) playerFirstPersonCamera.enabled = (playerFirstPersonCamera == activeMainCamera);
        if (playerThirdPersonCamera != null) playerThirdPersonCamera.enabled = (playerThirdPersonCamera == activeMainCamera);
        if (droneFirstPersonCamera != null) droneFirstPersonCamera.enabled = (droneFirstPersonCamera == activeMainCamera);
        if (droneThirdPersonCamera != null) droneThirdPersonCamera.enabled = (droneThirdPersonCamera == activeMainCamera);

        SetCameraRect(activeMainCamera, mainRect);

        // PiP は無効化
        DisablePip();

        if (activePipCamera != null)
        {
            SetCameraRect(activePipCamera, new Rect(0, 0, 0, 0));
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPipCamera(playerFirstPersonCamera);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPipCamera(playerThirdPersonCamera);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPipCamera(droneFirstPersonCamera);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetPipCamera(droneThirdPersonCamera);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            DisablePip();
        }

        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ResizePip( pipStep );
        }
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ResizePip( -pipStep );
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CyclePipPosition();
        }
    }

    private void SetPipCamera(Camera newPipCamera)
    {
        if (newPipCamera == null)
        {
            Debug.LogWarning("DualCamScreen: 選択したPiPカメラがInspectorで設定されていません。");
            return;
        }

        if (activeMainCamera == null)
        {
            Debug.LogWarning("DualCamScreen: メインカメラが設定されていません。PiPを設定できません。");
            return;
        }

        if (activePipCamera != null && activePipCamera != newPipCamera)
        {
            activePipCamera.enabled = false;
        }

        activePipCamera = newPipCamera;
        if (!activePipCamera.enabled)
        {
            activePipCamera.enabled = true;
        }

        // メインカメラの深度は低く、PiPの方が高くなるよう設定
        activeMainCamera.depth = 0;
        activePipCamera.depth = activeMainCamera.depth + 1;

        isPipEnabled = true;
        UpdatePipRect();
    }

    private void DisablePip()
    {
        isPipEnabled = false;
        if (activePipCamera != null)
        {
            activePipCamera.enabled = false;
        }
    }

    private void ResizePip(float delta)
    {
        if (!isPipEnabled || activePipCamera == null)
        {
            return;
        }

        pipWidth = Mathf.Clamp(pipWidth + delta, minPipSize, maxPipSize);
        pipHeight = Mathf.Clamp(pipHeight + delta, minPipSize, maxPipSize);

        UpdatePipRect();
    }

    private void CyclePipPosition()
    {
        if (!isPipEnabled || activePipCamera == null)
        {
            return;
        }

        pipPositionIndex = (pipPositionIndex + 1) % pipPositions.Length;
        UpdatePipRect();
    }

    private void UpdatePipRect()
    {
        if (activePipCamera == null || !isPipEnabled)
        {
            return;
        }

        var pos = pipPositions[pipPositionIndex];
        var x = pos.x;
        var y = pos.y;

        // 右下/左下の場合 y=0, 上向き = 1-pipHeight? Unity's viewport origin bottom-left, so left/top values depend on y.
        // y values are pre-computed as bottom-left corner.
        Rect pipRect = new Rect(x, y, pipWidth, pipHeight);

        SetCameraRect(activePipCamera, pipRect);
    }

    private void SetCameraRect(Camera cam, Rect rect)
    {
        if (cam == null)
            return;

        cam.rect = rect;
    }
}

