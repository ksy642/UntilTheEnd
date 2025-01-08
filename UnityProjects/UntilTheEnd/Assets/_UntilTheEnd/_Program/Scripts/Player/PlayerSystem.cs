using Unity.Cinemachine;

using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    [Header("Spawn Player")]
    public Transform spawnPoint;
    [SerializeField] private GameObject _playerPrefab;
    private GameObject _spawnedPlayer;

    [Header("Camera")]
    public CinemachineCamera cinemachineCamera;
    private Transform _firstPersonView; // 1��Ī
    private Transform _thirdPersonView; // 3��Ī
    private bool _isFirstPerson = false; // ���� ������ 1��Ī���� ����

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // ���� ��ȯ
        {
            ToggleView();
        }
    }

    public void SpawnPlayer()
    {
        if (_spawnedPlayer != null)
        {
            Debug.LogWarning("�̹� �÷��̾ �����Ǿ� �ֽ��ϴ�.");
            return;
        }
        else
        {
            Debug.LogWarning("�÷��̾ �����մϴ�.");

            // 1. ������ �ε� (�ʿ� �� Resources���� �ε�)
            if (_playerPrefab == null)
            {
                _playerPrefab = Resources.Load<GameObject>("TestPlayer"); // Resources �������� �ε�
            }

            // 2. �÷��̾� ����
            _spawnedPlayer = Instantiate(_playerPrefab, spawnPoint.position, Quaternion.identity);

            Debug.Log("�÷��̾ �����Ǿ����ϴ�: " + _spawnedPlayer.name);

            // 3. ī�޶� Ÿ�� ����
            if (cinemachineCamera != null)
            {
                // 1��Ī �� 3��Ī ī�޶� ��ġ ����
                _thirdPersonView = _spawnedPlayer.transform.GetChild(0);
                _firstPersonView = _spawnedPlayer.transform;

                // �ʱ� ī�޶� ����
                cinemachineCamera.Follow = _thirdPersonView; // 3��Ī �������� ����
            }
            else
            {
                Debug.LogWarning("CinemachineCamera�� �������� �ʾҽ��ϴ�.");
            }
        }
    }

    private void ToggleView()
    {
        if (_spawnedPlayer == null) 
            return;

        // ���� ��ȯ
        _isFirstPerson = !_isFirstPerson;

        if (_isFirstPerson)
        {
            cinemachineCamera.Follow = _firstPersonView;
            Debug.Log("1��Ī �������� ��ȯ�Ǿ����ϴ�.");
        }
        else
        {
            cinemachineCamera.Follow = _thirdPersonView;
            Debug.Log("3��Ī �������� ��ȯ�Ǿ����ϴ�.");
        }
    }
}
