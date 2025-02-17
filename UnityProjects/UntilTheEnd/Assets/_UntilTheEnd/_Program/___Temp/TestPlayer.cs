using UnityEngine;

namespace UntilTheEnd
{
    public class TestPlayer : MonoBehaviour
    {
        // 상호작용 타입 Enum
        public enum InteractionType
        { 
            None,
            Item,
            Door,
            NPC
        }
        public InteractionType CurrentInteraction { get; private set; }
        public GameObject InteractableObject { get; private set; }

        [Header("이동기 세팅")]
        public GameObject playerCameraRoot;

  

        private IPlayerState _currentState;
        private float _mouseSensitivity = 100.0f; // 마우스 감도
        private float _defaultMoveSpeed = 4.0f;
        private float _runSpeed = 5.2f;
        private float _crouchSpeed = 1.8f;
        private float _cameraPitch = 0f; // 카메라의 수직 회전 값
        private float _gravity = -10.0f;

        private CharacterController _characterController;
        private Transform _cameraTransform; // 메인 카메라 트랜스폼 (자동으로 찾음)
        private Transform _trainParent;
        private Vector3 _velocity; // 중력 벡터
        private Vector3 _lastTrainPosition = Vector3.zero;
        private bool _isOnTrain = false; // 기차에 탑승했는지 여부 확인

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();

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
            

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ChangeState(IPlayerState newState)
        {
            Debug.Log("먼저 나가고... " + newState);
            _currentState.ExitState(this);
            _currentState = newState;

            Debug.Log("먼저 상태진입한다. " + newState);
            _currentState.EnterState(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Train"))
            {
                // _lastTrainPosition을 통해서 프레임마다 기차가 얼마나 움직였는지 추적
                _trainParent = other.transform.root;
                _lastTrainPosition = _trainParent.position;
                _isOnTrain = true;

                // 캐릭터 컨트롤러를 사용하고 있어서 굳이 부모밑에다가 배치해주는게 의미가 없을 수 있음!!
                //transform.SetParent(_trainParent); // 부모 설정 (기차와 같이 이동)
            }



            // NPC, Item, Door 상호작용
            if (other.CompareTag("NPC"))
            {
                CurrentInteraction = InteractionType.NPC;
                InteractableObject = other.gameObject;
            }
            else if (other.CompareTag("Item"))
            {
                CurrentInteraction = InteractionType.Item;
                InteractableObject = other.gameObject;
            }
            else if (other.CompareTag("Door"))
            {
                CurrentInteraction = InteractionType.Door;
                InteractableObject = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Train"))
            {
                //transform.SetParent(null);
                _trainParent = null;
                _isOnTrain = false;
            }



            if (InteractableObject == other.gameObject)
            {
                CurrentInteraction = InteractionType.None;
                InteractableObject = null;
            }
        }

        private void Update()
        {
            _InputSpaceBar();
            _HandleMovement();   // 플레이어 움직임 처리
            _HandleMouseLook(); // 마우스 입력 처리 (카메라 회전)

            _OnTrain();
        }

        private void _InputSpaceBar()
        {
            if (Input.GetKeyDown(KeyCode.Space)) // 일단 SpaceBar 누르면...?!
            {
                // NPC와 Item에 관해 상호작용하는 인터페이스
                IInteractable interactable = InteractableObject?.GetComponent<IInteractable>();

                if (interactable != null && !UIWorldCanvasController.instance.isWorldCanvasActive)
                {
                    // NPC나 Item이나 둘다 작용함
                    interactable.Interact();


                    UIWorldCanvasController.instance.isWorldCanvasActive = true;

                    Debug.LogError("여기가 상호작용한거잖아 지금?" + UIWorldCanvasController.instance.isWorldCanvasActive);
                }



                /*
                 // 잠시 여기 보류!!!



                // 아이템에 가까이 갔을 때 SpaceBar 문구 떠서 상호작용 할 때
                if (EquipmentManager.instance.isInteractedObject)
                {
                    

                    Debug.Log("상호작용 캐릭터에서 스페이스바 누른거임");


                    // 여기서 SpaceBar UI가 떠있는 상태에서 스페이스바 연타 가능하잖아?
                    // 한번 눌렀을 때 1. 아이템이 사라지던가, 2. 아이템이 고정형으로 설명해주던가
                    // 이거 처리 잘 해야될거임
                    // 그렇지 않으면 오브젝트의 OnTrigger쪽에 EquipmentManager.instance.isInteractedObject 값을
                    // 바꿔주는곳에서 충돌나면 답없어진다 !!

                }
                */
            }
        }

        private void _HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float speed = _defaultMoveSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = _runSpeed;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                speed = _crouchSpeed; 
            }

            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

            if (_characterController.isGrounded)
            {
                // 약간의 중력을 줘서 땅에 계속 붙어있도록 유지
                _velocity.y = -0.5f;
            }
            else
            {
                _velocity.y += _gravity * 2.0f * Time.deltaTime;
            }

            _characterController.Move((moveDirection * speed + _velocity) * Time.deltaTime);
        }


        private void _HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;


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