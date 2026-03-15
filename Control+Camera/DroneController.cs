using UnityEngine;

public class DroneController : MonoBehaviour
{
    // キャラクター制御のパラメータ
    [Header("Movement Settings")]
    public float walkSpeed = 4.0f;
    public float runSpeed = 8.0f;
    public float ascendSpeed = 3.0f;
    public float descendSpeed = 3.0f;
    public float rotationSpeed = 10.0f;

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

    // カメラ設定
    [Header("Camera Settings")]
    [SerializeField] private Transform activeCameraTransform;
    [SerializeField] private bool isFirstPersonView = false;

    // コンポーネント参照
    private CharacterController characterController;

    // 移動関連の変数
    private Vector3 moveDirection = Vector3.zero;

    // デバッグ用
    [Header("Debug")]
    public bool debugMode = true;
    // デフォルトでこのカメラを操作するかどうか
    public bool isActive = false;

    // ReturnDrone.csによるドローンの呼び戻しがあるかのフラグと位置
    public bool returnDrone = false;
    public Vector3 returnDestination = Vector3.zero;

    private void Start()
    {
        // 必要なコンポーネントの取得
        characterController = GetComponent<CharacterController>();

        // CharacterControllerの設定を調整
        if (characterController != null)
        {
            characterController.skinWidth = skinWidth;
            characterController.stepOffset = stepOffset;
            characterController.minMoveDistance = minMoveDistance;
            characterController.center = new Vector3(0, 0, 0);

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

        // デフォルトでメインカメラを使用
        if (activeCameraTransform == null && Camera.main != null)
            activeCameraTransform = Camera.main.transform;
        else if (activeCameraTransform == null)
            Debug.LogError("カメラが設定されていません。");

        // アニメーターが設定されていない場合は自動的に取得
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // プレイヤーの初期位置を少し上に設定
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    public void SetActiveCamera(Transform cameraTransform)
    {
        activeCameraTransform = cameraTransform;

        // 1人称カメラかどうかを判定
        isFirstPersonView = cameraTransform.GetComponent<FirstPersonCamera>() != null;

        if (isFirstPersonView)
            Debug.Log("1人称視点カメラに操作を切り替えました");
        else
            Debug.Log("3人称視点カメラに操作を切り替えました");
    }

    private void Update()
    {
        // 操作中のキャラクターのみ入力を処理する
        if (!isActive)
            return;

        if (characterController == null || activeCameraTransform == null)
            return;

        // デバッグ表示
        if (debugMode)
        {
            //Debug.Log($"Y Position: {transform.position.y}, Y Velocity: {moveDirection.y}");
        }

        //ReturnDroneの呼び戻し処理がある場合はそっち優先
        if (returnDrone)
        {
            returnDrone = false;
            Vector3 returnVector = returnDestination - characterController.transform.position;
            characterController.Move(returnVector);
            return;
        }

        // 入力を取得
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // カメラの向きに合わせた水平移動方向を計算
        Vector3 forward = activeCameraTransform.forward;
        Vector3 right = activeCameraTransform.right;

        // Y軸の値を0にして水平方向のみの移動ベクトルを取得
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * vertical + right * horizontal).normalized;

        // 1人称視点の場合、カメラの向きに合わせてキャラクターを回転
        if (isFirstPersonView)
        {
            transform.rotation = Quaternion.Euler(0, activeCameraTransform.eulerAngles.y, 0);
        }
        // 3人称視点の場合
        else
        {
            // 移動入力がある場合はキャラクターの向き移動方向に回転
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // 走る入力（Shiftキー）を検出
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // 水平速度を適用
        moveDirection *= currentSpeed;

        // Y方向の移動：スペースキーで上昇、Shiftで下降
        moveDirection.y = 0f; // デフォルトでY方向は0
        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection.y = ascendSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveDirection.y = -descendSpeed;
        }

        // アニメーションパラメータ更新（ドローンにアニメーターがある場合）
        if (animator != null)
        {
            animator.SetBool(walkParamID, moveDirection.magnitude > 0.1f && !isRunning);
            animator.SetBool(runParamID, moveDirection.magnitude > 0.1f && isRunning);
            // ジャンプアニメーションはドローンでは使用しない
            animator.SetBool(jumpParamID, false);
        }

        // キャラクターを移動
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void UpdateCamera()
    {
        if (isActive)
        {
            MoveCamera();
        }
    }

    public void EnableCamera()
    {
        isActive = true;
        // Disable Kirby's camera
        // ...code to disable Kirby's camera...
    }

    public void DisableCamera()
    {
        isActive = false;
        // Enable Kirby's camera
        // ...code to enable Kirby's camera...
    }

    private void MoveCamera()
    {
        float moveSpeed = 5f;
        float ascendSpeed = 3f;
        float descendSpeed = 3f;

        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Space)) {
            transform.Translate(Vector3.up * ascendSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            transform.Translate(Vector3.down * descendSpeed * Time.deltaTime);
        }
    }
}