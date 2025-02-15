using UnityEngine;

namespace Assets.ButtonSystem
{
    public class StartButton : ITitleButton
    {
        public void OnClick()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Develop/Game");
        }
    }
}
