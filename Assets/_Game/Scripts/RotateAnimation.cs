using UnityEngine;

namespace Game
{
    public class RotateAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _speed;

        private void Update()
        {
            transform.Rotate(_direction * _speed * Time.deltaTime);
        }
    }
}
