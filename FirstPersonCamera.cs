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
            Debug.LogError("First Person Camera: �^�[�Q�b�g���ݒ肳��Ă��܂���I");
            enabled = false;
            return;
        }

        // �J�[�\�������b�N
        Cursor.lockState = CursorLockMode.Locked;

        // ������]�p�x�̐ݒ�
        rotation_y = firstperson_target.eulerAngles.y;

        // �J�����ʒu�̏����ݒ�
        UpdateCameraPosition();
    }

    void Update()
    {
        if (firstperson_target == null)
            return;

        // �}�E�X���͂ɂ���]
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // �c�̊p�x���X�V
        rotation_x -= mouseY;
        rotation_x = Mathf.Clamp(rotation_x, minVerticalAngle, maxVerticalAngle);

        // ���̊p�x���X�V
        rotation_y += mouseX;

        // �J�����̉�]��ݒ�
        transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);

        // �J�����ʒu�̍X�V
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // 1�l�̎��_�Ȃ̂Ńv���C���[�̖ڂ̈ʒu�ɌŒ�
        transform.position = firstperson_target.position + new Vector3(0, height, 0);
    }
}