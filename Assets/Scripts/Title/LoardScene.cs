using UnityEngine;
using DG.Tweening;

public class LoardScene : MonoBehaviour
{
    public GameObject loardNextScenePrefab;
    public Canvas canvas;


    void Awake()
    {
        GameObject loardNextScene = Instantiate(loardNextScenePrefab, canvas.transform);
        loardNextScene.transform.SetAsLastSibling();

        CanvasGroup canvasGroup = loardNextScene.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;

        canvasGroup.DOFade(0, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            loardNextScene.SetActive(false);
        });

    }
}
