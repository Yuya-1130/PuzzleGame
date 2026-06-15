using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int width = 6;
    public int height = 6;
    public float gridSize = 1.0f;

    private int mistakeCount = 0; // このステージでの不正解回数

    public TextMeshProUGUI UnText;

    // --- 【重要】インスペクターで設定する正解データ構造 ---
    [System.Serializable]
    public struct CorrectAnswer
    {
        public string blockID;   // ブロックの名前 (例: "Lcube")
        public Vector2Int gridPos; // 正解のマス座標 (例: X=1, Y=0)
    }

    [Header("ステージの正解配置データ")]
    public List<CorrectAnswer> stageAnswers;

    [Header("クリア時に移動するシーンの名前")]
    public string nextSceneName = "Stage2";

    // 現在の盤面に「どのIDのブロックが置かれているか」を記録する2次元配列
    private string[,] currentGrid;

    private float offsetX;
    private float offsetY;

    void Awake() { Instance = this; }

    void Start()
    {
        currentGrid = new string[width, height]; // 文字列（ID）を記録する配列に変更

        offsetX = (width - 1) / 2f * gridSize;
        offsetY = (height - 1) / 2f * gridSize;

        // 今回は「確定ボタン」で判定するため、背景の生成だけ行います
        CreateBackgroundGrid();
    }

    void CreateBackgroundGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 spawnPosition = GetWorldPosition(x, y);
                Instantiate(gridSlotPrefab, spawnPosition, Quaternion.identity).transform.SetParent(this.transform);
            }
        }
    }
    public GameObject gridSlotPrefab;

    // --- ブロックが配置されたらIDを登録 ---
    public void RegisterBlockID(int x, int y, string id)
    {
        if (IsInsideGrid(x, y)) currentGrid[x, y] = id;
    }

    // --- ブロックが持ち上げられたら消去 ---
    public void UnregisterBlockID(int x, int y)
    {
        if (IsInsideGrid(x, y)) currentGrid[x, y] = null;
    }

    // --- 【確定ボタン用】正解か不正解かのチェックロジック ---
    public void OnConfirmButtonPressed()
    {
        Debug.Log("答え合わせを開始します...");
       
        // リストに登録したすべての正解条件をループでチェックする
        foreach (CorrectAnswer answer in stageAnswers)
        {
            int targetX = answer.gridPos.x;
            int targetY = answer.gridPos.y;

            // チェック①：正解のマスに、指定されたIDのブロックがいるか？
            if (currentGrid[targetX, targetY] != answer.blockID)
            {
                Debug.Log($"不正解！ マス [{targetX}, {targetY}] に {answer.blockID} がいません。");
                UnText.text = "不正解！";
                // 不正解だからカウントを増やす
                mistakeCount++;

                Debug.Log($"現在の合計ミス回数: {mistakeCount}");

                return; // 1つでも違ったらその時点で「不正解」として終了
            }
        }

        // すべての条件をパスしたらクリア！
        GameClear();
    }

    void GameClear()
    {
        Debug.Log("★★★★★ 正解！ステージクリア！ ★★★★★");
        PlayerPrefs.SetInt("StageMistakes", mistakeCount);
        PlayerPrefs.Save(); // データをディスクに書き込み

        Debug.Log("ナゾ解明！不正解回数: " + mistakeCount);
        // 指定した名前のシーンへ画面を切り替える
        SceneManager.LoadScene(nextSceneName);
    }

    public Vector3 GetWorldPosition(int x, int y) => new Vector3(x * gridSize - offsetX, y * gridSize - offsetY, 0);
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x + offsetX) / gridSize);
        int y = Mathf.RoundToInt((worldPosition.y + offsetY) / gridSize);
        return new Vector2Int(x, y);
    }
    public bool IsInsideGrid(int x, int y) => (x >= 0 && x < width && y >= 0 && y < height);
}