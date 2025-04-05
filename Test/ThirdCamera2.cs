using UnityEngine;

public class ThirdCamera2 : MonoBehaviour
{
    //�C���X�y�N�^��Target�Ƃ������ڂ��쐬
    //�����Ƀv���C���[��Transform�R���|�[�l���g���h���b�O�h���b�v�őΉ�������
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
        // �J�[�\�������b�N
        Cursor.lockState = CursorLockMode.Locked;

        if (target == null)
        {
            Debug.LogError("Third Person Camera: �^�[�Q�b�g���ݒ肳��Ă��܂���I");
            enabled = false;
            return;
        }

        // �����p�x��ݒ�
        rotation_y = target.eulerAngles.y;
    }

    private void LateUpdate()
    {
        // �}�E�X���͂ɂ���]
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // �c�̊p�x�𐧌�
        rotation_x -= mouseY; // ���]���Ē����I�ȓ�����
        rotation_x = Mathf.Clamp(rotation_x, minVerticalAngle, maxVerticalAngle);

        // ���̊p�x���X�V
        rotation_y += mouseX;

        // ��]��K�p
        Quaternion rotation = Quaternion.Euler(rotation_x, rotation_y, 0);

        // �J�����̈ʒu���v�Z
        Vector3 targetPosition = target.position - (rotation * Vector3.forward * distance);

        // �X���[�Y�Ɉړ�
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, 1 / smoothSpeed);

        // �J�����̉�]�𒼐ڐݒ�iLookAt���g��Ȃ��j
        transform.rotation = rotation;
    }
}