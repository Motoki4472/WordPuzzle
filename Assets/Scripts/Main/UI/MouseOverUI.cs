using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Assets.WordBlock;

namespace Assets.UISystem
{
    public class MouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private WordList wordList;

        [SerializeField] private TMP_Text MainText = default;
        [SerializeField] private TMP_Text meanText = default;
        [SerializeField] private TMP_Text wordText = default;

        public void Start()
        {
            wordList = new WordList();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // マウスオーバーしたワードの意味をセット
            if (MainText.text == "") return;
            SetWordOneByOne(MainText.text.ToLower()); // 修正: 引数に渡す前に小文字に変換
            // 各文字をフェードイン
            meanText.DOFade(1, 0.5f);
            wordText.DOFade(1, 0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // 各文字をフェードアウト
            meanText.DOFade(0, 0.5f);
            wordText.DOFade(0, 0.5f);
        }

        public void SetWordOneByOne(string word)
        {
            wordText.text = "";
            meanText.text = "";

            foreach (char c in word)
            {
                wordText.text += c.ToString().ToUpper();
            }
            foreach (char c in wordList.GetMeaning(word))
            {
                meanText.text += c;
            }
        }
    }
}