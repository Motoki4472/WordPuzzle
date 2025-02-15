using UnityEngine;
using DG.Tweening;

namespace Assets.ButtonSystem
{
    public class StartButton : ITitleButton
    {
        public GameObject loadNextScenePrefab;

        public void OnClick()
        {
            loadNextScenePrefab = Resources.Load<GameObject>("Prefab/LoadNextScene");

            GameObject loadNextScene = GameObject.Instantiate(loadNextScenePrefab, GameObject.Find("Canvas").transform);
            loadNextScene.transform.SetAsLastSibling();

            CanvasGroup canvasGroup = loadNextScene.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            loadNextScene.GetComponent<CanvasGroup>().DOFade(1, 1f).OnComplete(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Develop/Game");
            });
        }
    }
}