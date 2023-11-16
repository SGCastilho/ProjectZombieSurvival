using System.Collections;
using UnityEngine;

namespace Core.Controllers
{
    public sealed class SoundtrackController : Singleton<SoundtrackController>
    {
        private AudioSource _audioSource;

        [Header("Settings")]
        [SerializeField] [Range(0.2f, 1f)] private float _maxVolume = 0.6f;
        [SerializeField] [Range(0.2f, 1f)] private float _fadeDuration = 0.6f;

        private bool _executingFade;

        protected override void Awake() 
        {
            if(_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = GetComponent<SoundtrackController>();
            _audioSource = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);

            if(_instance == null)
                return;
        }

        public void PlaySoundtrack(AudioClip audioClip)
        {
            if(_executingFade) return;

            StartCoroutine(ChangeSoundtrack(audioClip));
        }

        public void FadeIn()
        {
            if(_executingFade) return;

            StartCoroutine(Fade(true));
        }

        public void FadeOut()
        {
            if(_executingFade) return;

            StartCoroutine(Fade(false));
        }

        private IEnumerator Fade(bool fadeIn)
        {
            _executingFade = true;

            float time = 0f;
            float startVol = _audioSource.volume;
            float targetVolume = fadeIn ? _maxVolume : 0f;

            while(time < _fadeDuration)
            {
                time += Time.deltaTime;
                _audioSource.volume = Mathf.Lerp(startVol, targetVolume, time / _fadeDuration);

                yield return null;
            }

            _executingFade = false;

            yield break;
        }

        private IEnumerator ChangeSoundtrack(AudioClip clipToPlay)
        {
            _executingFade = true;

            float time = 0f;
            float startVol = 0f;
            float targetVolume = 0f;

            if(_audioSource.isPlaying)
            {
                startVol = _audioSource.volume;

                while(time < _fadeDuration)
                {
                    time += Time.deltaTime;
                    _audioSource.volume = Mathf.Lerp(startVol, targetVolume, time / _fadeDuration);

                    yield return null;
                }

                _audioSource.Stop();
            }

            _audioSource.clip = clipToPlay;
            _audioSource.volume = _maxVolume;

            yield return new WaitForSecondsRealtime(_fadeDuration);

            _audioSource.Play();

            _executingFade = false;

            yield break;
        }
    }
}
