using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("이동기 세팅")]
    public float moveSpeed = 5.0f; // 플레이어 이동 속도
    public float mouseSensitivity = 100.0f; // 마우스 감도
    public Transform cameraTransform; // 메인 카메라 트랜스폼 (자동으로 찾음)
    //public float jumpHeight = 1.3f; // 점프 높이
    public GameObject playerCameraRoot;

    private CharacterController _characterController;
    private float _cameraPitch = 0f; // 카메라의 수직 회전 값
    private Vector3 _velocity; // 중력 벡터
    private float _gravity = -10.0f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main; // Main Camera를 찾음

            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
                Debug.Log("Main Camera 설정 완료");
            }
            else
            {
                Debug.LogWarning("Main Camera 못찾음");
            }
        }

        // 마우스 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _HandleMovement(); // 플레이어 움직임 처리
        _HandleMouseLook(); // 마우스 입력 처리 (카메라 회전)
    }

    private void _HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 이동 방향 계산
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        // 지면에 닿아 있는지 확인
        if (_characterController.isGrounded)
        {
            // 지면에 있을 때, Y축 속도 초기화
            _velocity.y = 0f;

            // Space 키 입력 시 점프
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    _velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravity); // 점프 속도 계산 (부드러운 점프를 위해 루트 계산 사용)
            //}
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


    // cinemachine
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
}
