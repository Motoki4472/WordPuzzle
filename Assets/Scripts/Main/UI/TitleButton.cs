using UnityEngine;

namespace Assets.ButtonSystem
{
    public class TitleButton : ITitleButton
    {
        public void OnClick()
        {
            // タイトルに戻る
            Debug.Log("Title OnClick");
        }
    }
}
