using UnityEngine;

namespace Core.Utilities
{
    public sealed class DisableObjectAfterSeconds : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] [Range(1f, 20f)] private float _timeToDisable = 4f;

        private float _currentTimeToDisable;

        private void OnDisable()
        {
            _currentTimeToDisable = 0;
        }

        private void Update()
        {
            _currentTimeToDisable += Time.deltaTime;
            if(_currentTimeToDisable >= _timeToDisable)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
