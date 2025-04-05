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

    //�n�ʂ�ǃI�u�W�F�N�g�����uCameraBlockable�v�Ƃ������C���[�}�X�N�ɓo�^���Ă���
    //����Ƃ�������Raycast�ŃJ�������܂蔻��𒲂ׂĂ����
    [Header("BlockageCheck Settings")]
    public LayerMask CameraBlockable_LayerMask;

    [Header("Zoom Settings")]
    public float zoomSensitivity = 0.3f;

    private float rotation_x = 0.0f;
    private float rotation_y = 0.0f;

    private float zoom_level = 0.0f;
    private float zoomed_distance;

    RaycastHit hit;

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

        //�g��k����������
        zoomed_distance = distance;
        
    }

    private void LateUpdate()
    {
        Zoom();

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
        targetPosition.x = target.position.x - (zoomed_distance * Mathf.Cos(rotation_y));
        targetPosition.z = target.position.z - (zoomed_distance * Mathf.Sin(rotation_y));
        targetPosition.y = target.position.y + height + (zoomed_distance * Mathf.Sin(rotation_x));
        
        
        
        //�ړ� ���̎��X���[�W���O������ƃY�[���C������悤�ȕςȋ���������
        transform.position = targetPosition;

        //�J�������n�ʂɖ��܂邱�Ƃ�h���@�n�ʂƂԂ������炻�̏�ɃJ����������悤��
        if(Physics.Linecast(transform.position, target.position, out hit, CameraBlockable_LayerMask))
        {
            transform.position = hit.point+ Vector3.up * 0.1f;
        }



        //�����܂ł̓J�����̈ʒu�̒����@�����ł͂Ȃ�
        // �J�������^�[�Q�b�g�Ɍ�����
        transform.LookAt(target.position + Vector3.up * 1.0f); // �v���C���[�̑�����菭���������悤�ɒ���

        //�g��k��
        void Zoom()
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            Debug.Log(zoom);
            zoom_level += zoom * zoomSensitivity;
            zoom_level = Mathf.Clamp(zoom_level, 1.0f, -1.0f);

            if (zoom_level > 0)
            {
                zoomed_distance = (1-distance) * zoom_level + 1.0f;
            }
            else if (zoom < 0)
            {
                zoomed_distance = -4.0f * distance * zoom_level + distance;
            }
        }


    }
}