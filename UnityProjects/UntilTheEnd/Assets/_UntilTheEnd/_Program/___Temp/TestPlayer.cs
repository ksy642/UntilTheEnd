using UnityEngine;

namespace UntilTheEnd
{
    public class TestPlayer : MonoBehaviour
    {
        [Header("이동기 세팅")]
        public GameObject playerCameraRoot;
        public float moveSpeed = 4.0f; // 플레이어 이동 속도
        public float mouseSensitivity = 100.0f; // 마우스 감도

        private CharacterController _characterController;
        private Transform _cameraTransform; // 메인 카메라 트랜스폼 (자동으로 찾음)
        private Transform _trainParent;
        private float _cameraPitch = 0f; // 카메라의 수직 회전 값
        private float _gravity = -10.0f;
        private Vector3 _velocity; // 중력 벡터
        private Vector3 _lastTrainPosition = Vector3.zero;
        private bool _isOnTrain = false; // 기차에 탑승했는지 여부 확인

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();

            if (_cameraTransform == null)
            {
                Camera mainCamera = Camera.main; // Main Camera를 찾음

                if (mainCamera != null)
                {
                    _cameraTransform = mainCamera.transform;
                    Debug.Log("Main Camera 설정 완료");
                }
                else
                {
                    Debug.LogWarning("Main Camera 못찾음");
                }
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Train"))
            {
                // 기차를 부모로 설정
                _trainParent = other.transform.root;

                // 처음 기차에 탑승하는 순간 현재 기차 위치를 기억
                // 아래 코드한줄 없으면 처음 기차탈 때 플레이어가 튕기는 현상이 발생함..
                _lastTrainPosition = _trainParent.position;
                _isOnTrain = true;
                transform.SetParent(_trainParent); // 부모 설정 (기차와 같이 이동)
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Train"))
            {
                transform.SetParent(null);
                _trainParent = null;
                _isOnTrain = false;
            }
        }

        private void Update()
        {
            _InputSpaceBar();

            _HandleMovement();   // 플레이어 움직임 처리
            _HandleMouseLook(); // 마우스 입력 처리 (카메라 회전)
            _InputSpaceBar();

            _OnTrain();
        }



        private void _InputSpaceBar()
        {
            if (Input.GetKeyDown(KeyCode.Space)) // 일단 SpaceBar 누르면...?!
            {
                if (EquipmentManager.instance.isInteractedObject)
                {
                    // 아이템에 가까이 갔을 때 SpaceBar 문구 떠서 상호작용 할 때

                    Debug.Log("상호작용 캐릭터에서 스페이스바 누른거임");


                    // 여기서 SpaceBar UI가 떠있는 상태에서 스페이스바 연타 가능하잖아?
                    // 한번 눌렀을 때 1. 아이템이 사라지던가, 2. 아이템이 고정형으로 설명해주던가
                    // 이거 처리 잘 해야될거임
                    // 그렇지 않으면 오브젝트의 OnTrigger쪽에 EquipmentManager.instance.isInteractedObject 값을
                    // 바꿔주는곳에서 충돌나면 답없어진다 !!

                }
            }
        }

        private void _HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Shift = 달리고, Ctrl = 앉아서, 디폴트 값은 4.0f
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 5.2f; // Shift: 빠르게 이동
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                moveSpeed = 1.8f; // Ctrl: 느리게 이동
            }
            else
            {
                moveSpeed = 4.0f; // 기본 이동 속도
            }


            // 이동 방향 계산
            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

            // 지면에 닿아 있는지 확인
            if (_characterController.isGrounded)
            {
                // 지면에 있을 때, Y축 속도 초기화
                _velocity.y = 0f;
            }
            else
            {
                // 공중에서는 중력 지속 적용 + 낙하 가속도 추가
                float fallMultiplier = 2.0f; // 낙하 속도 배수 (값이 클수록 더 빠르게 낙하)
                _velocity.y += _gravity * fallMultiplier * Time.deltaTime;
            }

            // 이동 + 중력 적용하여 캐릭터 이동
            _characterController.Move((moveDirection * moveSpeed + _velocity) * Time.deltaTime);
        }


        private void _HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


            // 수직 각도 제한을 위한 변수 (_cameraPitch)
            _cameraPitch -= mouseY;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -30f, 30f); // 내려가는거, 올라가는거

            // playerCameraRoot의 로컬 회전 설정 (x: 상하, y: 좌우)
            if (playerCameraRoot != null)
            {
                playerCameraRoot.transform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
            }

            // 플레이어 오브젝트 자체의 좌우 회전 (y축)
            transform.Rotate(Vector3.up * mouseX);
        }

        private void _OnTrain()
        {
            if (_trainParent != null && _isOnTrain)
            {
                Vector3 trainMovement = _trainParent.position - _lastTrainPosition; // 기차의 이동 거리 계산
                _characterController.Move(trainMovement);    // 이동량을 플레이어에 적용
                _lastTrainPosition = _trainParent.position; // 최신 기차 위치 저장
            }
        }
    }
}