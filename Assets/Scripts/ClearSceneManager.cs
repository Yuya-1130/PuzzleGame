using UnityEngine;
using TMPro; 

public class ClearSceneManager : MonoBehaviour
{
    public TextMeshProUGUI resultText; // 「〇回間違えました」を表示するUI
    public TextMeshProUGUI rankText;   // 【おまけ】評価を表示するUI

    void Start()
    {
        // 保存された不正解回数を読み出す（データがなければ0になる）
        int finalMistakes = PlayerPrefs.GetInt("StageMistakes", 0);

        resultText.text = $"見事な推理だ！（指摘したミス：{finalMistakes}回）";

        
        if (rankText != null)
        {
            if (finalMistakes == 0)
            {
                rankText.text = "評価：素晴らしい！まさに天才考古学者だね。";
                rankText.color = Color.gold;
            }
            else if (finalMistakes <= 5)
            {
                rankText.text = "評価：さすがだね。素晴らしい着眼点だったよ。";
                rankText.color = Color.white;
            }
            else
            {
                rankText.text = "評価：ナゾは解けたが、少し回り道をしてしまったようだね。";
                rankText.color = Color.gray;
            }
        }
    }
}