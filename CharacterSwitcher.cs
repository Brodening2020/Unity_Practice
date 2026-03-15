using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 複数のキャラクターを切り替えるためのシンプルなスイッチャー
/// Qキーで操作キャラを交代し、CameraSwitcherへ現在のキャラクターと視点を通知します。
/// </summary>
public class CharacterSwitcher : MonoBehaviour
{
    // 現在操作しているキャラクターのインデックス
    private int nowChara = 0;

    // 操作可能なゲームキャラクター
    [SerializeField]
    private List<GameObject> charaList;

    // カメラ切り替え管理
    [Header("Camera Switcher")]
    public CameraSwitcher cameraSwitcher;

    void Start()
    {
        if (charaList == null || charaList.Count == 0)
        {
            Debug.LogError("CharacterSwitcher: charaList が空です。インスペクタでキャラクターを登録してください。");
            return;
        }

        // 全キャラクターを一旦無効化してから、先頭キャラのみ操作可能にする
        for (int i = 0; i < charaList.Count; i++)
        {
            SetCharacterActive(i, false);
        }
        SetCharacterActive(nowChara, true);

        // カメラは操作キャラの1人称にリセット
        if (cameraSwitcher != null)
            cameraSwitcher.SwitchToCharacter((CameraSwitcher.CharacterType)nowChara);
    }

    void Update()
    {
        // Qキーで操作キャラを切り替える
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeCharacter(nowChara);
        }
    }

    // 操作キャラクター変更メソッド
    void ChangeCharacter(int tempNowChara)
    {
        // 現在操作しているキャラクターを無効に
        SetCharacterActive(tempNowChara, false);

        // 次のキャラクターの番号を設定
        var nextChara = tempNowChara + 1;
        if (nextChara >= charaList.Count)
            nextChara = 0;

        // 次のキャラクターを有効に
        SetCharacterActive(nextChara, true);

        // カメラは新しい操作キャラの1人称視点へリセット
        if (cameraSwitcher != null)
            cameraSwitcher.SwitchToCharacter((CameraSwitcher.CharacterType)nextChara);

        // 現在のキャラクター番号を保持
        nowChara = nextChara;
    }

    private void SetCharacterActive(int index, bool active)
    {
        if (charaList == null || index < 0 || index >= charaList.Count)
            return;

        var go = charaList[index];
        if (go == null)
            return;

        // Kirby_Controller または RemoteCam_Controller のどちらかを探す
        var kirby = go.GetComponent<Kirby_Controller>();
        if (kirby != null)
        {
            kirby.ChangeControl(active);
            return;
        }

        var drone = go.GetComponent<RemoteCam_Controller>();
        if (drone != null)
        {
            drone.isActive = active;
            return;
        }

        Debug.LogWarning($"CharacterSwitcher: キャラクター {go.name} に対応するコントローラが見つかりませんでした。");
    }

}

