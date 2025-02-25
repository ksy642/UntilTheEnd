using UnityEngine;

namespace UntilTheEnd
{
    public class TestPlayer : MonoBehaviour//, IPlayerState
    {
        // 상호작용 타입 Enum
        public enum InteractionType
        {
            None,
            Item,
            Door,
            NPC
        }

        // InteractionType + 관련 GameObject를 저장하는 구조체
        public struct InteractionData
        {
            public InteractionType Type { get; }
            public GameObject Object { get; }

            public InteractionData(InteractionType type, GameObject obj)
            {
                Type = type;
                Object = obj;
            }

            public static InteractionData None
            {
                get { return new InteractionData(InteractionType.None, null); }
            }
        }

        // 이동 관련 설정 변수 (Inspector에서 조정 가능)
        [Header("이동기 세팅")]
        public GameObject playerCameraRoot;

        // 현재 플레이어 상태
        private IPlayerState _currentState;

        // 이동 속도 관련 변수
        private float _defaultMoveSpeed = 4.0f;
        private float _runSpeed = 5.2f;
        private float _crouchSpeed = 1.8f;

        // 카메라 및 입력 관련 변수
        private float _mouseSensitivity = 100.0f; // 마우스 감도
        private float _cameraPitch = 0f; // 카메라 수직 회전 값

        // 물리 관련 변수
        private float _gravity = -10.0f;
        private Vector3 _velocity; // 중력 벡터, 땅에 붙어있게 할라고 설정하는거

        // 기차 관련 변수
        private Transform _trainParent;
        private Vector3 _lastTrainPosition = Vector3.zero;
        private bool _isOnTrain = false; // 기차에 탑승 여부

        // Unity 컴포넌트 변수
        private CharacterController _characterController;
        private Transform _cameraTransform; // 메인 카메라 트랜스폼 (자동으로 찾음)

        // 현재 상호작용 정보 (InteractionType + GameObject)
        public InteractionData CurrentInteraction { get; private set; } = InteractionData.None;


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

            // 처음에 시작할 때, 캐릭터가 생성될 때는 Idle 상태로 생성
            _currentState = new IdleState();
            _currentState.EnterState(this);



            // 근데 플레이어가 마우스 안보이게 처리하는게 맞나?
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        public void ChangeState(IPlayerState newState)
        {
            Debug.Log("현재 상태는?... " + _currentState);
            _currentState.ExitState(this);
            _currentState = newState;

            Debug.Log("진입한 현재 상태는?... " + _currentState);
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
                CurrentInteraction = new InteractionData(InteractionType.NPC, other.gameObject);
                Debug.Log($"[TestPlayer] NPC 감지됨: {CurrentInteraction.GetType()}");
        }
            else if (other.CompareTag("Item"))
            {
                CurrentInteraction = new InteractionData(InteractionType.Item, other.gameObject);
            }
            else if (other.CompareTag("Door"))
            {
                CurrentInteraction = new InteractionData(InteractionType.Door, other.gameObject);
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


            // 현재 상호작용 중인 오브젝트에서 벗어났다면 초기화
            if (CurrentInteraction.Object == other.gameObject)
            {
                Debug.Log($"[TestPlayer] {other.gameObject.name} 나감, 상호작용 종료.");

                // 대화 도중 WASD로 벗어나면 강제로 종료
                if (DialogueManager.instance.isTalking)
                {
                    DialogueManager.instance.EndDialogue();
                }

                CurrentInteraction = InteractionData.None;
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
            if (Input.GetKeyDown(KeyCode.Space)) // 대화중인 조건문이 필요해
            {
                if (!DialogueManager.instance.isTalking)
                {
                    // 대화를 안할 때 Space를 누르면 대화를 시작해야겠지?
                    _currentState.UpdateState(this);


// 여기 일단 수정해야함!!
// 대화 중에 조건문 필요함!!


                    // 객체 is 타입
                    //if (_currentState is InteractNPCState)
                    //{
                    //    _currentState.UpdateState(this);
                    //}
                }
                else
                {
                    _currentState.UpdateState(this);
                }

                /*

                // NPC와 Item에 관해 상호작용하는 인터페이스
                IInteractable interactable = InteractableObject?.GetComponent<IInteractable>();

                if (interactable != null && !UIWorldCanvasController.instance.isWorldCanvasActive)
                {
                    // NPC나 Item이나 둘다 작용함
                    interactable.Interact();


                    UIWorldCanvasController.instance.isWorldCanvasActive = true;

                    Debug.LogError("여기가 상호작용한거잖아 지금?" + UIWorldCanvasController.instance.isWorldCanvasActive);
                }



                
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