using UnityEngine;

/// <summary>
/// 4つのカメラ（Kirby/Drone の 一人称/三人称）を管理し、
/// 必ず1つだけを有効にすることで Unity の複数カメラ描画問題を回避します。
/// </summary>
public class CameraSwitcher : MonoBehaviour
{
    public enum CharacterType
    {
        Kirby = 0,
        Drone = 1,
    }

    public enum ViewMode
    {
        FirstPerson,
        ThirdPerson,
    }

    [Header("Cameras")]
    public Camera playerFirst;
    public Camera playerThird;
    public Camera droneFirst;
    public Camera droneThird;

    [Header("Controllers")]
    public PlayerController playerController;
    public DroneController droneController;

    // ゲーム開始時にデフォルトで使うキャラクターとカメラの設定
    [Header("Initial State")]
    public CharacterType startCharacter = CharacterType.Kirby;
    public ViewMode startView = ViewMode.FirstPerson;

    public CharacterType CurrentCharacter { get; private set; }
    public ViewMode CurrentView { get; private set; }

    public KeyCode toggleKey = KeyCode.V;

    private void Start()
    {
        CurrentCharacter = startCharacter;
        CurrentView = startView;

        // 初期状態を有効に
        UpdateActiveCamera();
    }

    private void Update()
    {
        // Vキーで現在操作中のキャラクターの視点を切り替える
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleView();
        }
    }

    /// 
    /// 操作中キャラクターを切り替えて、必ず一人称視点にリセットする。
    /// CharacterSwitcherからのように，キャラ切り替えで呼び出すときはこれを呼び出すようにする
    public void SwitchToCharacter(CharacterType character)
    {
        CurrentCharacter = character;
        CurrentView = ViewMode.FirstPerson;
        UpdateActiveCamera();
    }

    /// <summary>
    /// 現在のキャラクター視点を切り替える（First &lt;&gt; Third）
    /// </summary>
    public void ToggleView()
    {
        CurrentView = (CurrentView == ViewMode.FirstPerson) ? ViewMode.ThirdPerson : ViewMode.FirstPerson;
        UpdateActiveCamera();
    }

    private void UpdateActiveCamera()
    {
        // すべてのカメラを無効化
        SetCameraEnabled(playerFirst, false);
        SetCameraEnabled(playerThird, false);
        SetCameraEnabled(droneFirst, false);
        SetCameraEnabled(droneThird, false);

        // 有効化すべきカメラを取得
        Camera active = GetActiveCamera();
        SetCameraEnabled(active, true);

        // コントローラーにアクティブなカメラを通知
        if (active != null)
        {
            if (CurrentCharacter == CharacterType.Kirby && playerController != null)
            {
                playerController.SetActiveCamera(active.transform);
            }
            else if (CurrentCharacter == CharacterType.Drone && droneController != null)
            {
                droneController.SetActiveCamera(active.transform);
            }
        }
    }

    private Camera GetActiveCamera()
    {
        if (CurrentCharacter == CharacterType.Kirby)
            return (CurrentView == ViewMode.FirstPerson) ? playerFirst : playerThird;

        return (CurrentView == ViewMode.FirstPerson) ? droneFirst : droneThird;
    }

    private void SetCameraEnabled(Camera cam, bool enabled)
    {
        if (cam == null)
            return;

        cam.enabled = enabled;

        // カメラ制御用のスクリプトも有効/無効を切り替える
        var fp = cam.GetComponent<FirstPersonCamera>();
        if (fp != null)
            fp.enabled = enabled;

        var tp = cam.GetComponent<ThirdPersonCamera>();
        if (tp != null)
            tp.enabled = enabled;
    }
}
