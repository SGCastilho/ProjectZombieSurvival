using UnityEngine;

namespace Core.Utilities
{
    public class StateMachine : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected State _initState;

        protected State _currentState;

        protected bool _changingState;

        protected virtual void Awake() 
        {
            if(_initState == null)
            {
                GetComponent<StateMachine>().enabled = false;
            }

            _currentState = _initState;
        }

        protected virtual void OnDisable()
        {
            _changingState = false;
            _currentState = _initState;
        }

        protected virtual void Update() 
        {
            if(_changingState) return;

            _currentState.PlayState();
        }

        public void ChangeState(State newState)
        {
            if(newState == null) return;

            _changingState = true;

            _currentState = newState;

            _changingState = false;
        }
    }
}
