using UnityEngine;

/// <summary>
/// Lobby창에서 GameManager유무를 체크하고 모든 매니저들을 관리하는 역할
/// </summary>
public class GameManager : DontDestroySingleton<GameManager>
{
    [Header("Prefab")]
    public GameObject dreamManagerPrefab;
    public GameObject uiManagerPrefab;

    [Header("소환된 Manager")]
    [SerializeField] private GameObject _spawnedDreamManager;
    [SerializeField] private GameObject _spawnedUIManager;


    // 매니저들이 있으면 삭제시키고 재소환 혹은 대기 => 게임도중 로비창으로 되돌아갈 때 대비
    // 없으면 소환시키고 => 처음 시작할 때 정도?

    private void Start()
    {
        Debug.Log("GameManager Start() 함수 실행");

        // UIManager가 존재하지 않으면 새로 생성
        if (_spawnedUIManager == null)
        {
            Debug.Log("UIManager가 없어서 생성합니다.");
            _spawnedUIManager = Instantiate(uiManagerPrefab);
        }
        else
        {
            Debug.Log("이 경우 로비로 다시 되돌아 왔을 때 뜨는 문구일거임 !!");
            Destroy(_spawnedUIManager);
            _spawnedUIManager = Instantiate(uiManagerPrefab);
        }







    }
}
