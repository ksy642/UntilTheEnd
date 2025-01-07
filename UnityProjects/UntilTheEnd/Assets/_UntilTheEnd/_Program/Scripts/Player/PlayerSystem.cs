using Unity.Cinemachine;

using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    [Header("Spawn Player")]
    public Transform spawnPoint; // 플레이어가 생성될 위치
    [SerializeField] private GameObject _playerPrefab; // 프리팹
    private GameObject _spawnedPlayer; // 생성된 플레이어를 참조할 변수

    [Header("Camera")]
    public CinemachineCamera cinemachineCamera; // 시네머신 카메라
    //public Vector3 targetAddOffset; // 카메라 오프셋

    private void Start()
    {
        Debug.LogWarning("SpawnPlayer 함수 실행");

        // 플레이어 스폰
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (_spawnedPlayer != null)
        {
            Debug.LogWarning("이미 플레이어가 생성되어 있습니다.");
            return;
        }
        else
        {

            Debug.LogWarning("플레이어를 생성합니다.");

            // 1. 프리팹 로드 (필요 시 Resources에서 로드)
            if (_playerPrefab == null)
            {
                _playerPrefab = Resources.Load<GameObject>("TestPlayer"); // Resources 폴더에서 로드
            }

            // 2. 플레이어 생성
            _spawnedPlayer = Instantiate(_playerPrefab, spawnPoint.position, Quaternion.identity);

            Debug.Log("플레이어가 생성되었습니다: " + _spawnedPlayer.name);

            // 3. 카메라 타겟 설정
            if (cinemachineCamera != null)
            {
                cinemachineCamera.Follow = _spawnedPlayer.transform; // 생성된 플레이어를 카메라가 따라가도록 설정
                cinemachineCamera.LookAt = _spawnedPlayer.transform; // 카메라가 플레이어를 바라보도록 설정
            }
            else
            {
                Debug.LogWarning("CinemachineCamera가 설정되지 않았습니다.");
            }
        }
    }
}