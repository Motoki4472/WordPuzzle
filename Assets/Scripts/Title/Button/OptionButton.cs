using UnityEngine;

namespace Assets.TitleButton
{
    public class OptionButton : ITitleButton
    {
        public void OnClick()
        {
            Debug.Log("OptionButton OnClick");
            // BGMの音量を変更する
            // SEの音量を変更する
        }
    }
}