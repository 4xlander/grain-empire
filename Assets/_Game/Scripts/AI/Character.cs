using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class Character : MonoBehaviour
    {
        public event Action<Character> OnStateChanged;
        public enum State
        {
            Free,
            Walk,
            Gathering,
            IsFilled,
        }

        [SerializeField] private NavMeshAgent _navAgent;
        [SerializeField] private Animator _animator;

        private State _state;
        private ResourceData _resource;
        private Action<Character> _onDestination;

        private void Update()
        {
            bool isMoving = _navAgent.velocity.magnitude > 0.1f;
            _animator.SetBool("Walk", isMoving);
            _animator.SetFloat("Speed", _navAgent.velocity.magnitude / _navAgent.speed);

            if (_navAgent.velocity.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_navAgent.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            if (!_navAgent.pathPending && _navAgent.remainingDistance <= _navAgent.stoppingDistance)
            {
                if (!_navAgent.hasPath || _navAgent.velocity.magnitude == 0f)
                {
                    OnDestinationReached();
                }
            }
        }

        public void MoveTo(Vector3 position, Action<Character> onComplete)
        {
            _onDestination = onComplete;
            _navAgent.SetDestination(position);
            ChangeState(State.Walk);
        }

        public void ApplyResource(ResourceData resource)
        {
            _resource = resource;
        }

        public ResourceData GrabResource()
        {
            var result = _resource;
            _resource = null;
            ChangeState(State.Free);
            return result;
        }

        public bool IsFree() =>
            _state == State.Free;

        private void OnDestinationReached()
        {
            var prevCallback = _onDestination;
            _onDestination = null;
            prevCallback?.Invoke(this);
        }

        private void ChangeState(State newState)
        {
            _state = newState;
            OnStateChanged?.Invoke(this);
        }
    }
}
