using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //�C���X�y�N�^��Target�Ƃ��������쐬
    //�����Ƀv���C���[��Transform�R���|�[�l���g���h���O�h���b�v�őΉ�������
    [Header("Target Settings")]
    public Transform target;

    [Header("Position Settings")]
    public float distance = 5.0f;
    public float height = 2.0f;
    public float smoothSpeed = 10.0f;

    //�}�E�X���x����������ƃY�[���C�����Ă��܂��悤�Ȗ�肪����
    //�J�����̈ʒu�ړ��̍ۂɕ⊮���Ԃɍ����ĂȂ��̂����H
    [Header("Rotation Settings")]
    public float mouseSensitivity = 0.5f;
    public float minVerticalAngle = -60.0f;
    public float maxVerticalAngle = 60.0f;

    private float rotation_x = 0.0f;
    private float rotation_y = 0.0f;

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

        // ����]�̏����p�x��ݒ�@�v���C���[�̎����ƍ��킹��
        rotation_y = target.eulerAngles.y;
        
    }

    private void LateUpdate()
    {
        // �}�E�X���͂ɂ���] 0.1�����Ȃ��Ɗ��x����
        float mouseX = Input.GetAxis("Mouse X") * 0.1f * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * 0.1f * mouseSensitivity;

        // �c�̊p�x���X�V
        rotation_x -= mouseY;
        rotation_x = Mathf.Clamp(rotation_x, minVerticalAngle, maxVerticalAngle);

        // ���̊p�x���X�V
        rotation_y += mouseX;

        // �J�����̈ړ���|�W�V�������v�Z
        Vector3 targetPosition = Vector3.zero;
        targetPosition.x = target.position.x - (distance * Mathf.Cos(rotation_y));
        targetPosition.z = target.position.z - (distance * Mathf.Sin(rotation_y));
        targetPosition.y = target.position.y + height + (distance * Mathf.Sin(rotation_x));

        //�ړ� ���̎��X���[�W���O������ƃY�[���C������悤�ȕςȋ���������
        transform.position = targetPosition;

        //�����܂ł̓J�����̈ʒu�̒����@�����ł͂Ȃ�
        // �J�������^�[�Q�b�g�Ɍ�����
        transform.LookAt(target.position + Vector3.up * 1.0f); // �v���C���[�̑�����菭���������悤�ɒ���
    }
}