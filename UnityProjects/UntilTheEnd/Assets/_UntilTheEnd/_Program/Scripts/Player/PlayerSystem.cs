using Unity.Cinemachine;

using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    [Header("Spawn Player")]
    public Transform spawnPoint; // �÷��̾ ������ ��ġ
    [SerializeField] private GameObject _playerPrefab; // ������
    private GameObject _spawnedPlayer; // ������ �÷��̾ ������ ����

    [Header("Camera")]
    public CinemachineCamera cinemachineCamera; // �ó׸ӽ� ī�޶�
    //public Vector3 targetAddOffset; // ī�޶� ������

    private void Start()
    {
        Debug.LogWarning("SpawnPlayer �Լ� ����");

        // �÷��̾� ����
        SpawnPlayer();
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
                cinemachineCamera.Follow = _spawnedPlayer.transform; // ������ �÷��̾ ī�޶� ���󰡵��� ����
                cinemachineCamera.LookAt = _spawnedPlayer.transform; // ī�޶� �÷��̾ �ٶ󺸵��� ����
            }
            else
            {
                Debug.LogWarning("CinemachineCamera�� �������� �ʾҽ��ϴ�.");
            }
        }
    }
}