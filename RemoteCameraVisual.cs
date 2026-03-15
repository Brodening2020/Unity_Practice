using UnityEngine;

public class RemoteCameraVisual : MonoBehaviour
{
    [Header("Visual Settings")]
    public Color cameraColor = Color.red;
    public float rotationSpeed = 1.0f;
    public bool pulsate = true;
    public float pulsateSpeed = 1.0f;
    public float pulsateAmount = 0.2f;

    private Transform cameraBody;
    private Light indicatorLight;
    private float initialIntensity;

    void Start()
    {
        // Create visual representation
        CreateCameraVisuals();
    }

    void Update()
    {
        // Animate light intensity if enabled
        if (pulsate && indicatorLight != null)
        {
            float pulse = Mathf.Sin(Time.time * pulsateSpeed) * pulsateAmount + 1.0f;
            indicatorLight.intensity = initialIntensity * pulse;
        }
    }

    void CreateCameraVisuals()
    {
        // Create camera body
        cameraBody = new GameObject("CameraBody").transform;
        cameraBody.SetParent(transform);
        cameraBody.localPosition = Vector3.zero;

        // Main camera body
        GameObject mainBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mainBody.transform.SetParent(cameraBody);
        mainBody.transform.localPosition = new Vector3(0, 0, 0);
        mainBody.transform.localScale = new Vector3(0.3f, 0.3f, 0.5f);
        mainBody.GetComponent<Renderer>().material.color = cameraColor;

        // Lens
        GameObject lens = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        lens.transform.SetParent(cameraBody);
        lens.transform.localPosition = new Vector3(0, 0, 0.25f);
        lens.transform.localRotation = Quaternion.Euler(90, 0, 0);
        lens.transform.localScale = new Vector3(0.2f, 0.1f, 0.2f);
        lens.GetComponent<Renderer>().material.color = Color.black;

        // Small indicator light
        GameObject lightObj = new GameObject("IndicatorLight");
        lightObj.transform.SetParent(cameraBody);
        lightObj.transform.localPosition = new Vector3(0, 0.2f, 0);

        indicatorLight = lightObj.AddComponent<Light>();
        indicatorLight.type = LightType.Point;
        indicatorLight.color = cameraColor;
        indicatorLight.intensity = 0.5f;
        indicatorLight.range = 0.5f;
        initialIntensity = indicatorLight.intensity;

        // Remove colliders to prevent physics interference
        foreach (Transform child in cameraBody)
        {
            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }
        }
    }
}