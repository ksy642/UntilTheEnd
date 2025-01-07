using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("�̵��� ����")]
    public float moveSpeed = 5.0f; // �÷��̾� �̵� �ӵ�
    public float mouseSensitivity = 100.0f; // ���콺 ����
    public Transform cameraTransform; // ���� ī�޶� Ʈ������ (�ڵ����� ã��)
    public float jumpHeight = 1.3f; // ���� ����

    private CharacterController _characterController;
    private float _cameraPitch = 0f; // ī�޶��� ���� ȸ�� ��
    private Vector3 _velocity; // �߷� ����
    private float _gravity = -10.0f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main; // Main Camera�� ã��

            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
                Debug.Log("Main Camera ���� �Ϸ�");
            }
            else
            {
                Debug.LogWarning("Main Camera ��ã��");
            }
        }

        // ���콺 Ŀ�� ���
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _HandleMovement(); // �÷��̾� ������ ó��
        _HandleMouseLook(); // ���콺 �Է� ó�� (ī�޶� ȸ��)
    }

    private void _HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // �̵� ���� ���
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        // ���鿡 ��� �ִ��� Ȯ��
        if (_characterController.isGrounded)
        {
            // ���鿡 ���� ��, Y�� �ӵ� �ʱ�ȭ
            _velocity.y = 0f;

            // Space Ű �Է� �� ����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravity); // ���� �ӵ� ��� (�ε巯�� ������ ���� ��Ʈ ��� ���)
            }
        }
        else
        {
            // ���߿����� �߷� ���� ���� + ���� ���ӵ� �߰�
            float fallMultiplier = 2.0f; // ���� �ӵ� ��� (���� Ŭ���� �� ������ ����)
            _velocity.y += _gravity * fallMultiplier * Time.deltaTime;
        }

        // �̵� + �߷� �����Ͽ� ĳ���� �̵�
        _characterController.Move((moveDirection * moveSpeed + _velocity) * Time.deltaTime);
    }

    private void _HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ī�޶� ��ġ(���� ȸ��) ���� (90�� �̻� ȸ�� ����)
        _cameraPitch -= mouseY;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);

        // ī�޶�� �÷��̾��� ȸ�� ó��
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f); // ī�޶� ���� ȸ��
        }

        transform.Rotate(Vector3.up * mouseX); // �÷��̾� ���� ȸ��
    }
}
