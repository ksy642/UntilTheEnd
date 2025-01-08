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
    private Transform _firstPersonView; // 1인칭
    private Transform _thirdPersonView; // 3인칭
    private bool _isFirstPerson = false; // 현재 시점이 1인칭인지 여부

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // 시점 전환
        {
            ToggleView();
        }
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
                // 1인칭 및 3인칭 카메라 위치 설정
                _thirdPersonView = _spawnedPlayer.transform.GetChild(0);
                _firstPersonView = _spawnedPlayer.transform;

                // 초기 카메라 설정
                cinemachineCamera.Follow = _thirdPersonView; // 3인칭 시점으로 시작
            }
            else
            {
                Debug.LogWarning("CinemachineCamera가 설정되지 않았습니다.");
            }
        }
    }

    private void ToggleView()
    {
        if (_spawnedPlayer == null) 
            return;

        // 시점 전환
        _isFirstPerson = !_isFirstPerson;

        if (_isFirstPerson)
        {
            cinemachineCamera.Follow = _firstPersonView;
            Debug.Log("1인칭 시점으로 전환되었습니다.");
        }
        else
        {
            cinemachineCamera.Follow = _thirdPersonView;
            Debug.Log("3인칭 시점으로 전환되었습니다.");
        }
    }
}
