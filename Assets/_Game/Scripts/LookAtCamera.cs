using UnityEngine;

namespace Game
{
    public class LookAtCamera : MonoBehaviour
    {
        private enum Mode
        {
            CameraForward,
            CameraForwardInverted,
            LookAtCamera,
            LookAtCameraInverted,
        }

        [SerializeField] private Mode _mode;

        private void LateUpdate()
        {
            switch (_mode)
            {
                case Mode.LookAtCamera:
                    transform.LookAt(Camera.main.transform);
                    break;

                case Mode.LookAtCameraInverted:
                    var dirFromCamera = transform.position - Camera.main.transform.position;
                    transform.LookAt(transform.position + dirFromCamera);
                    break;

                case Mode.CameraForward:
                    transform.forward = Camera.main.transform.forward;
                    break;

                case Mode.CameraForwardInverted:
                    transform.forward = -Camera.main.transform.forward;
                    break;
            }
        }
    }
}
