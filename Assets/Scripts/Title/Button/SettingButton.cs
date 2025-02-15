using UnityEngine;

namespace Assets.ButtonSystem
{
    public class SettingButton : ITitleButton
    {
        public void OnClick()
        {
            Debug.Log("OptionButton OnClick");
            // BGMの音量を変更する
            // SEの音量を変更する
        }
    }
}