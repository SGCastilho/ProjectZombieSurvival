using UnityEngine;

namespace Core.Controllers
{
    public sealed class SoundtrackClipsController : Singleton<SoundtrackClipsController>
    {
        [Header("Settings")]
        [SerializeField] private AudioClip[] _stageSoundtrackClips;

        [Space(8)]

        [SerializeField] private int _currentSoundtrackPlaying;

        private SoundtrackController _soundtrackController;

        private int _maxStageSoundtrackClips;

        protected override void Awake() 
        {
            if(_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _soundtrackController = SoundtrackController.Instance;

            DontDestroyOnLoad(gameObject);

            if(_instance == null)
                return;
        }

        private void OnEnable() 
        {
            _maxStageSoundtrackClips = _stageSoundtrackClips.Length-1;

            _currentSoundtrackPlaying = Random.Range(0, _maxStageSoundtrackClips);

            _soundtrackController.PlaySoundtrack(_stageSoundtrackClips[_currentSoundtrackPlaying]);
        }

        public void NextSoundtrack()
        {
            _currentSoundtrackPlaying++;

            if(_currentSoundtrackPlaying > _maxStageSoundtrackClips)
            {
                _currentSoundtrackPlaying = 0;
            }

            _soundtrackController.PlaySoundtrack(_stageSoundtrackClips[_currentSoundtrackPlaying]);
        }

        public void DestroySingleton() => Destroy(gameObject);
    }
}
