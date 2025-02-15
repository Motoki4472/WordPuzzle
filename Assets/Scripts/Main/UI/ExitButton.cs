using UnityEngine;
using DG.Tweening;
using Assets.Rule;

namespace Assets.ButtonSystem
{
    public class ExitButton : ITitleButton
    {
        private GameObject _background;
        private GameObject processSystem;
        public void OnClick()
        {
            _background = GameObject.Find("MenuBackGround");
            processSystem = GameObject.Find("ProcessSystem");

            if (_background != null)
            {
                // 背景、設定ボタン、タイトルに戻るボタン、×ボタンをDOTWeenでフェードイン表示する
                _background.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
                GameObject.Find("MasterVolume").transform.DOLocalMove(new Vector3(700, 147, 0), 0.4f).SetDelay(0.2f);
                GameObject.Find("BGMVolume").transform.DOLocalMove(new Vector3(700, 22, 0), 0.4f).SetDelay(0.1f);
                GameObject.Find("SEVolume").transform.DOLocalMove(new Vector3(700, -103, 0), 0.4f);
                _background.transform.Find("Setting").transform.Find("SettingText").GetComponent<CanvasGroup>().DOFade(0, 0.3f);
                _background.transform.Find("Title").transform.Find("TitleText").GetComponent<CanvasGroup>().DOFade(0, 0.35f);
                _background.transform.Find("Retry").transform.Find("RetryText").GetComponent<CanvasGroup>().DOFade(0, 0.4f);
                _background.transform.Find("Exit").transform.Find("ExitText").GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
                {
                    _background.transform.localPosition = new Vector3(0, 800, 0);
                    processSystem.GetComponent<ProcessSystem>().UnsetProcessStateToStop();
                });

            }
            else
            {
                Debug.LogError("MenuBackGround GameObject not found.");
            }
        }
    }
}
