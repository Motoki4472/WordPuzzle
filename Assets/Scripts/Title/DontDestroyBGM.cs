using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Audio;

namespace Assets.Sound
{
    public class DontDestroyBGM : MonoBehaviour
    {
        private static DontDestroyBGM instance = null;
        public List<AudioClip> bgmList;
        private AudioSource audioSource;
        private AudioClip nextClip;
        public float fadeTime = 10.0f;
        public float volume = 0.5f;
        public AudioMixerGroup bgmMixerGroup;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = bgmMixerGroup;
                audioSource.volume = volume;
                PlayRandomBGM();
            }
            else
            {
                Destroy(gameObject); // すでに存在している場合、新しいものは削除
            }
        }

        private void PlayRandomBGM()
        {
            if (bgmList.Count > 0)
            {
                AudioClip randomBGM = bgmList[Random.Range(0, bgmList.Count)];
                audioSource.clip = randomBGM;
                audioSource.loop = false;
                audioSource.Play();
                audioSource.DOFade(volume, fadeTime);
                ScheduleNextBGM();
            }
        }

        private void ScheduleNextBGM()
        {
            if (bgmList.Count > 1)
            {
                nextClip = bgmList[Random.Range(0, bgmList.Count)];
                while (nextClip == audioSource.clip)
                {
                    nextClip = bgmList[Random.Range(0, bgmList.Count)];
                }
                Invoke("CrossfadeToNextBGM", audioSource.clip.length - fadeTime); // フェード時間前にフェード開始
            }
        }

        private void CrossfadeToNextBGM()
        {
            audioSource.DOFade(0, fadeTime).OnComplete(() =>
            {
                audioSource.clip = nextClip;
                audioSource.Play();
                audioSource.DOFade(volume, fadeTime);
                ScheduleNextBGM();
            });
        }

        public void ChangeBGM(AudioClip newClip, float fadeDuration = 3.0f)
        {
            if (audioSource.clip == newClip) return;

            audioSource.DOFade(0, fadeDuration).OnComplete(() =>
            {
                audioSource.clip = newClip;
                audioSource.Play();
                audioSource.DOFade(volume, fadeDuration);
                ScheduleNextBGM();
            });
        }

        public void SetVolume(float newVolume)
        {
            volume = newVolume;
            audioSource.volume = newVolume;
        }
    }
}