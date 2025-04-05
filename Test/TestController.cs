using Unity.VisualScripting;
using UnityEngine;

public class TestController : MonoBehaviour
{
    // �L�����N�^�[����̃p�����[�^
    [Header("Movement Settings")]
    public float walkSpeed = 4.0f;
    public float runSpeed = 8.0f;
    public float jumpForce = 8.0f;
    public float rotationSpeed = 10.0f;
    public float gravity = 20.0f;

    // �A�j���[�V��������
    [Header("Animation Settings")]
    public Animator animator;
    private readonly int walkParamID = Animator.StringToHash("IsWalking");
    private readonly int runParamID = Animator.StringToHash("IsRunning");
    private readonly int jumpParamID = Animator.StringToHash("IsJumping");

    // �R���|�[�l���g�Q��
    CharacterController characterController;
    private Transform cameraTransform;

    // �ړ��֘A�̕ϐ�
    private Vector3 moveDirection = Vector3.zero;
    private bool kirby_grounded;

    private void Start()
    {
        // �K�v�ȃR���|�[�l���g�̎擾
        characterController = GetComponent<CharacterController>();

        if (Camera.main != null)
            cameraTransform = Camera.main.transform;

        // �A�j���[�^�[���ݒ肳��Ă��Ȃ��ꍇ�͎����I�Ɏ擾
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // �ڒn���� - �v���p�e�B�Ƃ��Đ������Q��
        kirby_grounded = characterController.isGrounded;
        print(kirby_grounded);

        // �ڒn���Ă���Ƃ�
        if (kirby_grounded)
        {
            print("CharacterController is grounded");

            // ���͂��擾
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 Velocity = new Vector3(horizontal, 0, vertical);
            moveDirection = Velocity.normalized;

            /* �J�����̌����ɍ��킹���ړ��������v�Z
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Y���̒l��0�ɂ��Đ��������݂̂̈ړ��x�N�g�����擾
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            moveDirection = (forward * vertical + right * horizontal).normalized;

            // �ړ����͂�����ꍇ�̓L�����N�^�[�̌������ړ������ɉ�]
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            */
            // ������́iShift�L�[�j�����o
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // ���x��K�p
            moveDirection *= currentSpeed*Time.deltaTime;

            /* �W�����v���͂����o
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
                animator.SetBool(jumpParamID, true);
            }
            else
            {
                animator.SetBool(jumpParamID, false);
            }*/

            /* �A�j���[�V�����p�����[�^�X�V
            animator.SetBool(walkParamID, moveDirection.magnitude > 0.1f && !isRunning);
            animator.SetBool(runParamID, moveDirection.magnitude > 0.1f && isRunning);
            */
        }

        // �d�͂̓K�p
        if (!kirby_grounded)
        {
            print("CharacterController is not grounded");
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //�ړ�����v�Z�@transform.position�͌��݈ʒu
        Vector3 destination = transform.position + moveDirection;

        //�ړ���Ɍ����ĉ�]
        transform.LookAt(destination);
        //�ړ���̍��W��ݒ�
        transform.position = destination;
    }
}