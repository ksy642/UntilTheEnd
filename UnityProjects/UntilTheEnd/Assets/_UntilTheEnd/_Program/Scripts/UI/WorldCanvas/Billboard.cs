using UnityEngine;

namespace UntilTheEnd
{
    public class Billboard : MonoBehaviour
    {
        private static Transform _mainCam;

        private void Start()
        {
            _mainCam = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if (_mainCam == null || transform == null)
            {
                // 카메라가 없거나 오브젝트가 없으면 실행하지 않음
                return;
            }

            var targetPosition = transform.position + _mainCam.forward;
            var upDirection = _mainCam.rotation * Vector3.up;

            // 오브젝트가 카메라를 바라보도록 회전
            transform.LookAt(targetPosition, upDirection);
        }
    }
}