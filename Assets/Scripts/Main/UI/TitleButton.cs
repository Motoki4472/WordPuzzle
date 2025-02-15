using UnityEngine;

namespace Assets.ButtonSystem
{
    public class TitleButton : ITitleButton
    {
        public void OnClick()
        {
            // タイトル画面に遷移
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Develop/Title");
        }
    }
}
