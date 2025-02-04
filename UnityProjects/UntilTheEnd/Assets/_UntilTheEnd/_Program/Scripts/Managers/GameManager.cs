using System;
using UnityEngine;

/// <summary>
/// Lobby창에서 GameManager유무를 체크하고 모든 매니저들을 관리하는 역할
/// </summary>
public class GameManager : DontDestroySingleton<GameManager>
{

    // 일단 여기 헤더 두개 사용안하게 되긴했는데...
    [Header("Prefab")]
    public GameObject dreamManagerPrefab;
    public GameObject uiManagerPrefab;

    [Header("소환된 Manager")]
    [SerializeField] private GameObject _spawnedDreamManager;
    [SerializeField] private GameObject _spawnedUIManager;






    // UIManager에서 게임 일시정지를 감지하도록 이벤트 추가
    // 일단, 기획이 완벽하지 않아서 ESC를 눌렀을 때 멈추가 하냐 마냐가 관건인데...결정나면 수정할듯함
    // 그리고 게임매니저에서 UI매니저를 소환하는것보다 이미 생성시켜놓고 이벤트로 주는게 더 괜찮은 방법같음
    public static event Action<bool> OnESCMenuToggled;
    public bool isESCMenuOpen = false;

    private void Start()
    {
        Debug.Log("GameManager Start() 함수 실행");

        // UIManager가 존재하지 않으면 새로 생성
        //if (_spawnedUIManager == null)
        //{
        //    Debug.Log("UIManager가 없어서 생성합니다.");
        //    _spawnedUIManager = Instantiate(uiManagerPrefab);
        //}
        //else
        //{
        //    Debug.Log("이 경우 로비로 다시 되돌아 왔을 때 뜨는 문구일거임 !!");
        //    Destroy(_spawnedUIManager);
        //    _spawnedUIManager = Instantiate(uiManagerPrefab);
        //}

    }

    // UIManager에 ESC 메뉴 상태 전달
    public void ToggleESCMenu(bool isOpen)
    {
        isESCMenuOpen = isOpen;
        OnESCMenuToggled?.Invoke(isESCMenuOpen);
        Debug.Log("GameManager-UImanger ESC 메뉴 상태 변경 : " + isESCMenuOpen);
    }
}
