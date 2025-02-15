using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Asset.UI
{
    public class Slider : MonoBehaviour
    {
        public AudioMixerGroup audioMixerGroup;
        public string parameterName;

        private UnityEngine.UI.Slider _slider;

        void Start()
        {
            _slider = GetComponent<UnityEngine.UI.Slider>();
            _slider.onValueChanged.AddListener(SetAudioMixerValue);
        }

        private void SetAudioMixerValue(float value)
        {
            audioMixerGroup.audioMixer.SetFloat(parameterName, Mathf.Log10(value) * 20);
        }
    }
}
