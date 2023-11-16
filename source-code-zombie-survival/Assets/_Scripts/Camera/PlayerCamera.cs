using UnityEngine;

namespace Core.Camera
{
    public sealed class PlayerCamera : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform _cameraTarget;

        [Space(8)]

        [SerializeField] private float _cameraXDelimitator = 23.9f;

        private Transform _transform;
        private Vector3 _cameraVector;

        private void Awake() 
        {
            if(_cameraTarget == null) 
            {
                _cameraTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            }

            _transform = gameObject.transform;
        }

        private void Update()
        {
            _cameraVector = new Vector3(_cameraTarget.position.x, _transform.position.y, _transform.position.z);

            if(_cameraVector.x > _cameraXDelimitator || _cameraVector.x < -_cameraXDelimitator) return;

            _transform.position = _cameraVector;
        }
    }
}
