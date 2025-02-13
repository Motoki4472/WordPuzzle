using UnityEngine;

namespace Assets.ButtonSystem
{
    public class StartButton : ITitleButton
    {
        public void OnClick()
        {
            Debug.Log("StartButton OnClick");
            // ゲームを開始する
            //LoadScene.LoadScene("Game");
            //　ゲームシーンがまだない
        }
    }
}
