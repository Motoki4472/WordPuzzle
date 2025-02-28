using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Assets.ButtonSystem
{
    public class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button button;
        private CanvasGroup canvasGroup;
        [SerializeField] private int buttonType;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip mouseOverSE;
        [SerializeField] private AudioClip clickSE;
        private ITitleButton iButton;
        private Tween fadeTween;

        public void Start()
        {
            button = GetComponent<Button>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 1f; // 初期値は完全表示

            switch (buttonType)
            {
                case 0:
                    iButton = new StartButton();
                    break;
                case 1:
                    iButton = new SettingButton();
                    break;
                case 2:
                    iButton = new CreditsButton();
                    break;
                case 3:
                    iButton = new HamburgerMenu();
                    break;
                case 4:
                    iButton = new SettingButtonInMainScene();
                    break;
                case 5:
                    iButton = new TitleButton();
                    break;
                case 6:
                    iButton = new ExitButton();
                    break;
                case 7:
                    iButton = new RetryButton();
                    break;
                case 8:
                    iButton = new PlayClassic();
                    break;
                case 9:
                    iButton = new PlayEndless();
                    break;
                case 10:
                    iButton = new PlayBountyHunt();
                    break;
            }

            button.onClick.AddListener(PlayClickAnimation);
            button.onClick.AddListener(iButton.OnClick);
            button.onClick.AddListener(PlayClickSE); // 追加
        }

        public void PlayClickAnimation()
        {
            Sequence clickSequence = DOTween.Sequence();

            clickSequence
                .Append(transform.DOScale(1.05f, 0.1f).SetEase(Ease.OutQuad)) // 1.1倍に拡大
                .Join(canvasGroup.DOFade(0.6f, 0.1f)) // 同時に透明度を0.6に
                .Append(transform.DOScale(0.95f, 0.12f).SetEase(Ease.InQuad)) // 0.9倍に縮小
                .Join(canvasGroup.DOFade(1f, 0.1f)) // 透明度を元に戻す
                .Append(transform.DOScale(1f, 0.1f).SetEase(Ease.OutQuad)) // 元のサイズに戻る
                .SetLink(gameObject); // オブジェクトが破棄されたらアニメーションも停止
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayMouseOverAnimation();
            PlayMouseOverSE();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopMouseOverAnimation();
        }

        private void PlayMouseOverAnimation()
        {
            fadeTween = canvasGroup.DOFade(0.8f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo) // ループで0.8 ↔ 1を繰り返す
                .SetEase(Ease.InOutSine);
        }

        private void StopMouseOverAnimation()
        {
            fadeTween.Kill(); // ループを停止
            canvasGroup.DOFade(1f, 0.2f); // 透明度を元に戻す
        }

        private void PlayMouseOverSE()
        {
            if (audioSource != null && mouseOverSE != null)
            {
                audioSource.PlayOneShot(mouseOverSE);
            }
        }

        private void PlayClickSE() // 追加
        {
            if (audioSource != null && clickSE != null)
            {
                audioSource.PlayOneShot(clickSE);
            }
        }
    }
}
