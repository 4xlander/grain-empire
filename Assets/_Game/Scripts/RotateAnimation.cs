using UnityEngine;

namespace Game
{
    public class RotateAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _speed;
        [SerializeField] private float _waveDelta = 0.1f;

        private void Update()
        {
            transform.Rotate(_direction * _speed * Time.deltaTime);
            //transform.Translate(0, Mathf.Sin(_waveDelta * Time.deltaTime), 0);
        }
    }
}
