using UnityEngine;

namespace Assets.ButtonSystem
{
    public class RetryButton : ITitleButton
    {
        public void OnClick()
        {
            //　現在のシーンを再読み込み
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
