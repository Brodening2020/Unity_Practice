using Unity.VisualScripting;
using UnityEngine;

public class TestController : MonoBehaviour
{
    // キャラクター制御のパラメータ
    [Header("Movement Settings")]
    public float walkSpeed = 4.0f;
    public float runSpeed = 8.0f;
    public float jumpForce = 8.0f;
    public float rotationSpeed = 10.0f;
    public float gravity = 20.0f;

    // アニメーション制御
    [Header("Animation Settings")]
    public Animator animator;
    private readonly int walkParamID = Animator.StringToHash("IsWalking");
    private readonly int runParamID = Animator.StringToHash("IsRunning");
    private readonly int jumpParamID = Animator.StringToHash("IsJumping");

    // コンポーネント参照
    CharacterController characterController;
    private Transform cameraTransform;

    // 移動関連の変数
    private Vector3 moveDirection = Vector3.zero;
    private bool kirby_grounded;

    private void Start()
    {
        // 必要なコンポーネントの取得
        characterController = GetComponent<CharacterController>();

        if (Camera.main != null)
            cameraTransform = Camera.main.transform;

        // アニメーターが設定されていない場合は自動的に取得
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // 接地判定 - プロパティとして正しく参照
        kirby_grounded = characterController.isGrounded;
        print(kirby_grounded);

        // 接地しているとき
        if (kirby_grounded)
        {
            print("CharacterController is grounded");

            // 入力を取得
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 Velocity = new Vector3(horizontal, 0, vertical);
            moveDirection = Velocity.normalized;

            /* カメラの向きに合わせた移動方向を計算
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Y軸の値を0にして水平方向のみの移動ベクトルを取得
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            moveDirection = (forward * vertical + right * horizontal).normalized;

            // 移動入力がある場合はキャラクターの向きを移動方向に回転
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            */
            // 走る入力（Shiftキー）を検出
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // 速度を適用
            moveDirection *= currentSpeed*Time.deltaTime;

            /* ジャンプ入力を検出
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
                animator.SetBool(jumpParamID, true);
            }
            else
            {
                animator.SetBool(jumpParamID, false);
            }*/

            /* アニメーションパラメータ更新
            animator.SetBool(walkParamID, moveDirection.magnitude > 0.1f && !isRunning);
            animator.SetBool(runParamID, moveDirection.magnitude > 0.1f && isRunning);
            */
        }

        // 重力の適用
        if (!kirby_grounded)
        {
            print("CharacterController is not grounded");
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //移動先を計算　transform.positionは現在位置
        Vector3 destination = transform.position + moveDirection;

        //移動先に向けて回転
        transform.LookAt(destination);
        //移動先の座標を設定
        transform.position = destination;
    }
}