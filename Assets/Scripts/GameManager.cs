using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 枠の大きさをインスペクターから自由に変えられるようにする
    public int width = 6;  // 横6マス
    public int height = 6; // 縦6マス

    // 枠の中に「何が入っているか」を記録する2次元配列（データ上の枠）
    private GameObject[,] grid;

    void Start()
    {
        // 枠のサイズ分のデータ領域を確保
        grid = new GameObject[width, height];

        // 枠の背景（見た目）を生成する関数を呼び出す
        CreateBackgroundGrid();
    }

    public GameObject gridSlotPrefab; // 枠の背景画像プレハブをここにドラッグ＆ドロップ

    void CreateBackgroundGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // $1 \times 1$ なので、座標 (x, y) にそのまま配置すれば綺麗に並ぶ
                float offsetX = (width - 1) / 2f;
                float offsetY = (height - 1) / 2f;
                Vector3 spawnPosition = new Vector3(x - offsetX, y - offsetY, 0);

                // 枠の背景を画面に生成
                GameObject slot = Instantiate(gridSlotPrefab, spawnPosition, Quaternion.identity);

                // 整理しやすいように、GameManagerの子オブジェクトにする
                slot.transform.SetParent(this.transform);
            }
        }
    }
    // 動かそうとした座標 (x, y) が枠内なら true、外なら false を返す関数
    public bool IsInsideGrid(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }
}

