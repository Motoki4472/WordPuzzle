using UnityEngine;
using DG.Tweening;

namespace Assets.ButtonSystem
{
    public class SettingButtonInMainScene : ITitleButton
    {
        public void OnClick()
        {
            GameObject.Find("MasterVolume").transform.DOLocalMove(new Vector3(70, 147, 0), 0.5f);
            GameObject.Find("BGMVolume").transform.DOLocalMove(new Vector3(70, 22, 0), 0.5f).SetDelay(0.1f);
            GameObject.Find("SEVolume").transform.DOLocalMove(new Vector3(70, -103, 0), 0.5f).SetDelay(0.2f);
        }
    }
}
