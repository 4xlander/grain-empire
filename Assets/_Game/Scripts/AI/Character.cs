using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
            bool isMoving = _navAgent.velocity.magnitude > 0.2f;
            _animator.SetBool("Walk", isMoving);
            _animator.SetFloat("Speed", _navAgent.velocity.magnitude / _navAgent.speed);

            if (isMoving)
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

        public void MoveTo(Vector3 targetPosition, Action<Character> onComplete)
        {
            _onDestination = onComplete;
            var offset = GetRandomOffset(_navAgent.transform.position, targetPosition, 0.4f, 60f);
            var position = targetPosition + offset;
            _navAgent.SetDestination(position);
            ChangeState(State.Walk);
        }

        public void ApplyResource(ResourceData resource)
        {
            _navAgent.avoidancePriority = 30;
            _resource = resource;
        }

        public ResourceData GrabResource()
        {
            _navAgent.avoidancePriority = 60;
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

        private Vector3 GetRandomOffset(Vector3 agentPos, Vector3 targetPos, float distance, float angleDeg)
        {
            var direction = (agentPos - targetPos).normalized;

            var halfAngle = angleDeg * 0.5f;
            var randomAngle = Random.Range(-halfAngle, halfAngle);

            var rotation = Quaternion.Euler(0, randomAngle, 0);
            var offsetDir = rotation * direction;

            return offsetDir * distance;
        }
    }
}
