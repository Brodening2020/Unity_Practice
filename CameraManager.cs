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
        // �R���|�[�l���g�����蓖�Ă��Ă��Ȃ��ꍇ�͎����I�Ɍ���
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

        // �X�N���v�g�Q�Ƃ̎擾
        if (firstPersonCamera != null)
            firstPersonCameraScript = firstPersonCamera.GetComponent<FirstPersonCamera>();

        if (thirdPersonCamera != null)
            thirdPersonCameraScript = thirdPersonCamera.GetComponent<ThirdPersonCamera>();

        if (player != null && playerController == null)
            playerController = player.GetComponent<Kirby_Controller>();

        // �f�t�H���g��3�l�̎��_����J�n
        SwitchToThirdPerson();
    }

    void Update()
    {
        // �J�����؂�ւ��L�[�������ꂽ�王�_��؂�ւ�
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

        // �J�����̗L��/������؂�ւ�
        if (firstPersonCamera != null)
            firstPersonCamera.enabled = true;
        if (thirdPersonCamera != null)
            thirdPersonCamera.enabled = false;

        // �X�N���v�g�̗L��/������؂�ւ�
        if (firstPersonCameraScript != null)
            firstPersonCameraScript.enabled = true;
        if (thirdPersonCameraScript != null)
            thirdPersonCameraScript.enabled = false;

        // �R���g���[���[�ɃJ���������X�V
        if (playerController != null)
            playerController.SetActiveCamera(firstPersonCamera.transform);

        Debug.Log("1�l�̎��_�ɐ؂�ւ��܂���");
    }

    void SwitchToThirdPerson()
    {
        isFirstPersonView = false;

        // �J�����̗L��/������؂�ւ�
        if (firstPersonCamera != null)
            firstPersonCamera.enabled = false;
        if (thirdPersonCamera != null)
            thirdPersonCamera.enabled = true;

        // �X�N���v�g�̗L��/������؂�ւ�
        if (firstPersonCameraScript != null)
            firstPersonCameraScript.enabled = false;
        if (thirdPersonCameraScript != null)
            thirdPersonCameraScript.enabled = true;

        // �R���g���[���[�ɃJ���������X�V
        if (playerController != null)
            playerController.SetActiveCamera(thirdPersonCamera.transform);

        Debug.Log("3�l�̎��_�ɐ؂�ւ��܂���");
    }
}