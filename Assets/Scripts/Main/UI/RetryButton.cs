using UnityEngine;

namespace Assets.ButtonSystem
{
    public class RetryButton : ITitleButton
    {
        public void OnClick()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Develop/Game");
        }
    }
}
