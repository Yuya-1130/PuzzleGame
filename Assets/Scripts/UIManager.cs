using UnityEngine;

public class UIManager : MonoBehaviour
{
    // UI画面のゲームオブジェクトを格納する変数
    // インスペクターウィンドウからゲームオブジェクトを設定する
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject logsView;

    // UI画面の表示状態を格納する変数
    bool isSettingsPanel = false;
    bool isLogsView = false;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化。すべてのUIを画面を非表示にする。
       // InactivateAll();
    }

    // Update is called once per frame
    void Update() { }

    // Settings Buttonが押されたときの処理
    public void ShowSettingsPanel()
    {
        if (!isSettingsPanel)
        {
            settingsPanel.SetActive(true);
            isSettingsPanel = true;
        }
        else
        {
            settingsPanel.SetActive(false);
            isSettingsPanel = false;
        }
    }

    // ChatLogs Buttonが押されたときの処理
    public void ShowLogsView()
    {
        if (!isLogsView)
        {
            logsView.SetActive(true);
            isLogsView = true;
        }
        else
        {
            logsView.SetActive(false);
            isLogsView = false;
        }

    }

    // すべてのUI画面を非アクティブにする
    //public void InactivateAll()
    //{
    //    settingsPanel.SetActive(false);
    //    logsView.SetActive(false);
    //}
}