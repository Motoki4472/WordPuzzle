using UnityEngine;
using DG.Tweening;

namespace Assets.ButtonSystem
{
    public class ExitButton : ITitleButton
    {
        private GameObject _background;
        public void OnClick()
        {
            _background = GameObject.Find("MenuBackGround");

            if (_background != null)
            {
                // 背景、設定ボタン、タイトルに戻るボタン、×ボタンをDOTWeenでフェードイン表示する
                _background.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
                _background.transform.Find("Setting").transform.Find("SettingText").GetComponent<CanvasGroup>().DOFade(0, 0.3f);
                _background.transform.Find("Title").transform.Find("TitleText").GetComponent<CanvasGroup>().DOFade(0, 0.3f);
                _background.transform.Find("Exit").transform.Find("ExitText").GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
                {
                    _background.transform.localPosition = new Vector3(0, 800, 0);
                });

            }
            else
            {
                Debug.LogError("MenuBackGround GameObject not found.");
            }
        }
    }
}
