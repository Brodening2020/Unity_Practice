using UnityEngine;

/// <summary>
/// 別個で設置できるカメラ
/// </summary>

public class RemoteCameraSystem : MonoBehaviour
{
    [Header("Camera Settings")]
    public KeyCode placeCameraKey = KeyCode.C;
    public float cameraPlaceDistance = 10f;
    public GameObject remoteCameraPrefab;

    [Header("Preview Settings")]
    public GameObject cameraPlacementPreview;
    public Color validPlacementColor = Color.green;
    public Color invalidPlacementColor = Color.red;
    public float previewUpdateRate = 0.1f;

    [Header("Picture-in-Picture Settings")]
    public Vector2 pipPosition = new Vector2(0.7f, 0.7f);  // Screen position (0-1)
    public Vector2 pipSize = new Vector2(0.25f, 0.25f);    // Screen size (0-1)

    // References
    private Transform playerCamera;
    private GameObject currentRemoteCamera;
    private RenderTexture remoteCameraTexture;
    private Material pipMaterial;
    private float lastPreviewUpdateTime;
    private bool canPlaceCamera = false;

    // Remote camera position
    private Vector3 remoteCameraPosition;
    private Quaternion remoteCameraRotation;

    void Start()
    {
        // Find player camera
        if (Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
        else
        {
            Debug.LogError("メインカメラが見つかりません。");
            enabled = false;
            return;
        }

        // Setup PiP rendering
        SetupPiPRendering();

        // Initialize preview
        if (cameraPlacementPreview == null)
        {
            cameraPlacementPreview = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cameraPlacementPreview.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Destroy(cameraPlacementPreview.GetComponent<Collider>());
        }

        // Set preview initially inactive
        cameraPlacementPreview.SetActive(false);
    }

    void Update()
    {
        if (playerCamera == null)
            return;

        // Update placement preview
        if (Time.time >= lastPreviewUpdateTime + previewUpdateRate)
        {
            UpdatePlacementPreview();
            lastPreviewUpdateTime = Time.time;
        }

        // Place camera on key press
        if (Input.GetKeyDown(placeCameraKey) && canPlaceCamera)
        {
            PlaceRemoteCamera();
        }
    }

    void UpdatePlacementPreview()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Calculate preview position
        if (Physics.Raycast(ray, out hit, cameraPlaceDistance))
        {
            // Surface detected, place on surface
            remoteCameraPosition = hit.point + hit.normal * 0.2f; // Slight offset from surface
            remoteCameraRotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
            canPlaceCamera = true;
            cameraPlacementPreview.GetComponent<Renderer>().material.color = validPlacementColor;
        }
        else
        {
            // No surface, place at maximum distance
            remoteCameraPosition = playerCamera.position + playerCamera.forward * cameraPlaceDistance;
            remoteCameraRotation = Quaternion.LookRotation(-playerCamera.forward, Vector3.up);
            canPlaceCamera = true;
            cameraPlacementPreview.GetComponent<Renderer>().material.color = validPlacementColor;
        }

        // Update preview position and activate it
        cameraPlacementPreview.transform.position = remoteCameraPosition;
        cameraPlacementPreview.transform.rotation = remoteCameraRotation;
        cameraPlacementPreview.SetActive(true);
    }

    void PlaceRemoteCamera()
    {
        // Remove previous camera if exists
        if (currentRemoteCamera != null)
        {
            Destroy(currentRemoteCamera);
        }

        // Create new camera
        if (remoteCameraPrefab != null)
        {
            currentRemoteCamera = Instantiate(remoteCameraPrefab, remoteCameraPosition, remoteCameraRotation);
        }
        else
        {
            // Create a basic camera if no prefab is provided
            currentRemoteCamera = new GameObject("RemoteCamera");
            currentRemoteCamera.transform.position = remoteCameraPosition;
            currentRemoteCamera.transform.rotation = remoteCameraRotation;

            // Add camera component
            Camera remoteCam = currentRemoteCamera.AddComponent<Camera>();
            remoteCam.clearFlags = CameraClearFlags.SolidColor;
            remoteCam.backgroundColor = new Color(0.05f, 0.05f, 0.05f, 1f);
            remoteCam.fieldOfView = 60f;
            remoteCam.nearClipPlane = 0.1f;
            remoteCam.farClipPlane = 100f;
            remoteCam.depth = -1; // Render before main camera
            remoteCam.targetTexture = remoteCameraTexture;

            // Add visual representation of the camera
            GameObject cameraVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cameraVisual.transform.SetParent(currentRemoteCamera.transform);
            cameraVisual.transform.localPosition = new Vector3(0, 0, 0.1f);
            cameraVisual.transform.localScale = new Vector3(0.2f, 0.2f, 0.4f);
            cameraVisual.GetComponent<Renderer>().material.color = Color.black;
        }

        // Connect the camera to the render texture
        Camera remoteCameraComponent = currentRemoteCamera.GetComponent<Camera>();
        if (remoteCameraComponent != null)
        {
            remoteCameraComponent.targetTexture = remoteCameraTexture;
        }

        Debug.Log("リモートカメラを設置しました: " + remoteCameraPosition);
    }

    void SetupPiPRendering()
    {
        // Create render texture for remote camera
        remoteCameraTexture = new RenderTexture(512, 512, 24);
        remoteCameraTexture.name = "RemoteCameraTexture";

        // Create material for PiP rendering
        pipMaterial = new Material(Shader.Find("Unlit/Texture"));
        pipMaterial.mainTexture = remoteCameraTexture;
    }

    void OnGUI()
    {
        // Only draw PiP if we have a remote camera
        if (currentRemoteCamera != null && pipMaterial != null)
        {
            // Calculate rect based on screen dimensions
            int width = (int)(Screen.width * pipSize.x);
            int height = (int)(Screen.height * pipSize.y);
            int x = (int)(Screen.width * pipPosition.x) - width;
            int y = (int)(Screen.height * pipPosition.y) - height;

            // Draw PiP in corner
            GUI.DrawTexture(new Rect(x, y, width, height), remoteCameraTexture, ScaleMode.StretchToFill);

            // Optional: Draw border around PiP
            GUI.Box(new Rect(x - 2, y - 2, width + 4, height + 4), "");
        }
    }

    void OnDestroy()
    {
        // Clean up resources
        if (remoteCameraTexture != null)
        {
            remoteCameraTexture.Release();
            Destroy(remoteCameraTexture);
        }

        if (pipMaterial != null)
        {
            Destroy(pipMaterial);
        }
    }
}