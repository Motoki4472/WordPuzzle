using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Assets.Rule;

namespace Assets.ButtonSystem
{
    public class PlayBountyHunt : ITitleButton
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
                SceneManager.sceneLoaded += BountyHuntSceneLoaded;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Develop/BountyHunt");
            });
        }

        private void BountyHuntSceneLoaded(Scene next, LoadSceneMode mode)
        {
            var ProcessSystem = GameObject.Find("ProcessSystem").GetComponent<ProcessSystem>();
            ProcessSystem.gameMode = 1;
            SceneManager.sceneLoaded -= BountyHuntSceneLoaded;
        }
    }
}