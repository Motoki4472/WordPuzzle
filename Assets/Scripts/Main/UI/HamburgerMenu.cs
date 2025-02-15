using UnityEngine;
using DG.Tweening;

namespace Assets.ButtonSystem
{
    public class HamburgerMenu : ITitleButton
    {
        private GameObject _background;
        public void OnClick()
        {
            // 背景を取得
            _background = GameObject.Find("MenuBackGround");
            
            if (_background != null)
            {
                // 背景、設定ボタン、タイトルに戻るボタン、×ボタンをDOTWeenでフェードイン表示する
                _background.transform.localPosition = new Vector3(0, 0, 0);
                _background.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
                _background.transform.Find("Setting").transform.Find("SettingText").GetComponent<CanvasGroup>().DOFade(1, 1f);
                _background.transform.Find("Title").transform.Find("TitleText").GetComponent<CanvasGroup>().DOFade(1, 1f);
                _background.transform.Find("Exit").transform.Find("ExitText").GetComponent<CanvasGroup>().DOFade(1, 1f);
                _background.transform.Find("Retry").transform.Find("RetryText").GetComponent<CanvasGroup>().DOFade(1, 1f);
                
            }
            else
            {
                Debug.LogError("MenuBackGround GameObject not found.");
            }
        }
    }
}