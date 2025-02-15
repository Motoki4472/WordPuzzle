using UnityEngine;

namespace Assets.ButtonSystem
{
    public class RetryButton : ITitleButton
    {
        public void OnClick()
        {
            // 今のシーンを再ロード
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
