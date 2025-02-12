using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

namespace Assets.UISystem
{
    public class UISystem : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText = default;
        [SerializeField] private TMP_Text highScoreText = default;
        [SerializeField] private TMP_Text turnText = default;
        [SerializeField] private TMP_Text[] Words = new TMP_Text[8];
        private List<string> InputWords = new List<string>();
        private bool isAnimating = false; // フラグを追加

        public void Start()
        {
            scoreText.text = "0";
            highScoreText.text = "0";
            turnText.text = "0";
            for (int i = 0; i < Words.Length; i++)
            {
                Words[i].text = "";
            }
        }

        public void SetScoreText(int score)
        {
            scoreText.text = score.ToString();
        }

        public void SetHighScoreText(int highScore)
        {
            highScoreText.text = highScore.ToString();
        }

        public void SetTurnText(int turn)
        {
            turnText.text = turn.ToString();
        }

        public void SetWord(string Word)
        {
            InputWords.Add(Word);
            if (!isAnimating) // アニメーション中でない場合のみ実行
            {
                SetWordToText(InputWords[0]);
            }
        }

        private void SetWordToText(string Word)
        {
            isAnimating = true; // アニメーション開始

            // 全てのテキストをy軸に-35移動
            for (int i = 0; i < Words.Length; i++)
            {
                Words[i].transform.DOMoveY(212.5071f + 64f - 35 * i, 0.5f);
            }

            // words[7]をフェードアウトさせつつテキストの内容を後ろの文字から1文字ずつ消去
            Words[7].DOFade(0, 0.5f).OnComplete(() =>
            {
                Words[7].text = "";
                // words[0]をフェードインさせつつ文字を1文字ずつ入力
                Words[0].text = "";
                Words[0].DOFade(1, 0.5f);
                for (int i = 0; i < Word.Length; i++)
                {
                    int index = i;
                    DOVirtual.DelayedCall(0.1f * i, () => Words[0].text += Word[index]);
                }

                // words[7]をY=90に移動
                Words[7].transform.DOMoveY(90, 0.5f).OnComplete(() =>
                {
                    // 配列の中身を1ずつずらす
                    TMP_Text temp = Words[7];
                    for (int i = 7; i > 0; i--)
                    {
                        Words[i] = Words[i - 1];
                    }
                    Words[0] = temp;

                    isAnimating = false; // アニメーション終了
                });
                InputWords.RemoveAt(0);

            });
        }

        public void Update()
        {
            if (!isAnimating && InputWords.Count > 0) // リストが空でないことを確認
            {
                SetWordToText(InputWords[0]);
            }
        }
    }
}