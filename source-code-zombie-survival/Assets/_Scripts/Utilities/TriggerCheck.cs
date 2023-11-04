using UnityEngine.Events;
using UnityEngine;

namespace Core.Utilities
{
    public sealed class TriggerCheck : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string _collisionTag;

        [Header("Events")]
        [Space(8)]
        [SerializeField] private UnityEvent<GameObject> ReturnCollision;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(_collisionTag == null) return;

            if(collider.tag == _collisionTag)
            {
                if(ReturnCollision == null) return;

                ReturnCollision.Invoke(collider.gameObject);
            }
        }
    }
}
