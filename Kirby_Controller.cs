using UnityEngine;

public class Kirby_Controller : MonoBehaviour
{
    // キャラクター制御のパラメータ Headerと書くことでコンポーネントの欄に表示できてパラメータ調整可能
    [Header("Movement Settings")]
    public float walkSpeed = 4.0f;
    public float runSpeed = 8.0f;
    public float jumpForce = 8.0f;
    public float rotationSpeed = 10.0f;
    public float gravity = 20.0f;

    // CharacterController設定
    [Header("Character Controller Settings")]
    public float skinWidth = 0.08f;
    public float stepOffset = 0.3f;
    public float minMoveDistance = 0.001f;

    // アニメーション制御
    [Header("Animation Settings")]
    public Animator animator;
    private readonly int walkParamID = Animator.StringToHash("IsWalking");
    private readonly int runParamID = Animator.StringToHash("IsRunning");
    private readonly int jumpParamID = Animator.StringToHash("IsJumping");

    // コンポーネント参照
    private CharacterController characterController;
    private Transform cameraTransform;

    // 移動関連の変数
    private Vector3 moveDirection = Vector3.zero;
    private bool isGrounded;

    // デバッグ用
    [Header("Debug")]
    public bool debugMode = true;
    public bool forceGrounded = false;

    private void Start()
    {
        // 必要なコンポーネントの取得
        characterController = GetComponent<CharacterController>();

        // CharacterControllerの設定を調整
        if (characterController != null)
        {
            // 皮膚の厚みを設定（値が大きすぎると地面を検出できない）
            characterController.skinWidth = skinWidth;

            // 段差を乗り越える高さ
            characterController.stepOffset = stepOffset;

            // 最小移動距離
            characterController.minMoveDistance = minMoveDistance;

            // 中心を下方向に調整（地面に接触するように）
            characterController.center = new Vector3(0, 0, 0);

            // レイヤー設定の確認
            if (gameObject.layer == 0) // Default layer
            {
                Debug.Log("キャラクターが Default レイヤーにあります。もし問題が続くなら専用レイヤーの使用を検討してください。");
            }
        }
        else
        {
            Debug.LogError("CharacterController が見つかりません。コンポーネントを追加してください。");
            return;
        }

        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
        else
            Debug.LogError("Main Camera が見つかりません。");

        // アニメーターが設定されていない場合は自動的に取得
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // 初期位置を少し上に設定して確実に落下するようにする
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    private void Update()
    {
        if (characterController == null || cameraTransform == null)
            return;

        // 接地判定
        isGrounded = characterController.isGrounded;

        // デバッグモードの場合、強制的に接地
        if (debugMode && forceGrounded)
        {
            isGrounded = true;
        }

        // デバッグ表示
        if (debugMode)
        {
            Debug.Log($"isGrounded: {isGrounded}, Y Position: {transform.position.y}, Y Velocity: {moveDirection.y}");
        }

        // 接地しているとき
        if (isGrounded)
        {
            // 入力を取得
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // カメラの向きに合わせた移動方向を計算
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

            // 走る入力（Shiftキー）を検出
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // 速度を適用
            moveDirection *= currentSpeed;

            // ジャンプ入力を検出
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
                if (animator != null)
                    animator.SetBool(jumpParamID, true);
            }
            else
            {
                moveDirection.y = -0.5f; // 接地しているときは少し下向きの力を加える（地面への密着を維持）
                if (animator != null)
                    animator.SetBool(jumpParamID, false);
            }

            // アニメーションパラメータ更新
            if (animator != null)
            {
                animator.SetBool(walkParamID, moveDirection.magnitude > 0.1f && !isRunning);
                animator.SetBool(runParamID, moveDirection.magnitude > 0.1f && isRunning);
            }
        }
        else
        {
            // 空中での水平移動の保持（現在の水平速度を維持）
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                // カメラの向きに合わせた移動方向を計算
                Vector3 forward = cameraTransform.forward;
                Vector3 right = cameraTransform.right;

                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();

                Vector3 airMove = (forward * verticalInput + right * horizontalInput).normalized;

                // 空中での操作性を下げるために係数を小さくする
                float airControl = 0.3f;
                moveDirection.x = Mathf.Lerp(moveDirection.x, airMove.x * walkSpeed, Time.deltaTime * airControl);
                moveDirection.z = Mathf.Lerp(moveDirection.z, airMove.z * walkSpeed, Time.deltaTime * airControl);
            }

            // 重力の適用
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Y速度に下限を設定して極端な落下速度を防ぐ
        moveDirection.y = Mathf.Max(moveDirection.y, -50f);

        // 下方向のレイキャストで地面までの距離を確認
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
        {
            if (debugMode)
            {
                Debug.Log($"Ground distance: {hit.distance}, Ground layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
        }
        else if (debugMode)
        {
            Debug.Log("地面が検出されませんでした。");
        }

        // キャラクターを移動
        CollisionFlags collisionFlags = characterController.Move(moveDirection * Time.deltaTime);

        // 接地を強制する（床が検出されない問題の一時的な対策）
        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            isGrounded = true;
            moveDirection.y = 0;
        }
    }
}
