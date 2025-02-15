using UnityEngine;

namespace Assets.ButtonSystem
{
    public class SettingButtonInMainScene : ITitleButton
    {
        public void OnClick()
        {
            // 音量のスライダーを表示する
            Debug.Log("HamburgerMenu OnClick");
        }
    }
}
