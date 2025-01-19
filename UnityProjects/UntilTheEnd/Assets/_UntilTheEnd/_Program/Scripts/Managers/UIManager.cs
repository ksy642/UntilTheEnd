using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class UIManager : DontDestroySingleton<UIManager>
{
    private bool _isTransitioning = false; // 씬 전환 중 상태 플래그

    [Header("1번 : ESC")]
    [SerializeField] private GameObject _escMenuPanel; // 메뉴판 UI
    [SerializeField] private bool _isESCMenuOpen = false; // ESC 메뉴 상태

    [Header("2번 : EquipmentPanel")]
    [SerializeField] private GameObject _equipmentPanel; // 장비창 UI
    [SerializeField] private bool _isEquipmentMenuOpen = false; // 장비창 상태

    [Header("Fade")]
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private float _fadeDuration = 3f; // 페이드 시간

    [Header("FPS")]
    private float _deltaTime = 0.0f;


    private void Start()
    {
        Debug.LogWarning("[테스트용] 첫 시작은 메뉴판 다 꺼버리고 시작");
        _ToggleMenu(0);
    }

    // Fade In - Fade Out 효과 주는 함수
    public void FadeToScene(string sceneName)
    {
        if (_isTransitioning)
        {
            return; // 씬 전환 중이면 무시 (씬 이동 중에 메뉴판 못키게 설정)
        }
        else
        {
            if (_escMenuPanel.activeSelf) // 메뉴가 활성화된 경우 닫음
            {
                _ToggleMenu(0);
            }
        }
        _isTransitioning = true; // 씬 전환 시작
        StartCoroutine(_FadeAndLoadScene(sceneName));
    }

    #region Fade In - Fade Out
    private IEnumerator _FadeAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(_Fade(1)); // 페이드 아웃 (화면을 어둡게)
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(_Fade(0)); // 페이드 인 (화면을 밝게)
        _isTransitioning = false; // 씬 전환 완료
    }

    private IEnumerator _Fade(float targetAlpha)
    {
        float startAlpha = _fadeCanvasGroup.alpha;
        float time = 0;

        if (targetAlpha == 1)
        {
            _fadeCanvasGroup.gameObject.SetActive(true);
        }

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            _fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
            yield return null;
        }

        _fadeCanvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0)
        {
            _fadeCanvasGroup.gameObject.SetActive(false);
        }
    }
    #endregion

    private void Update()
    {
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;

        if (_isTransitioning)
        {
            return; // 씬 전환 중에는 ESC 입력을 무시
        }

        // ESC 키 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isEquipmentMenuOpen)
            {
                // 장비창이 열려있으면 ESC로 장비창을 닫음
                _ToggleMenu(2);
            }
            else
            {
                // ESC 메뉴를 열거나 닫음
                _ToggleMenu(1);
            }
        }

        // E 키 처리
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_isESCMenuOpen) // ESC 메뉴가 열려있을 때는 무시
            {
                _ToggleMenu(2); // 장비창 열기/닫기
            }
        }
    }

    private void _ToggleMenu(int menuCount)
    {
        switch (menuCount)
        {
            case 0:  //모든 메뉴판을 다 닫음
                _isESCMenuOpen = false;
                _escMenuPanel.SetActive(false);

                _isEquipmentMenuOpen = false;
                _equipmentPanel.SetActive(false);

                Debug.Log("모든 메뉴가 닫혔습니다.");
                break;
            case 1: // ESC 메뉴
                _isESCMenuOpen = !_isESCMenuOpen;
                _escMenuPanel.SetActive(_isESCMenuOpen);

                if (_isESCMenuOpen)
                {
                    // ESC 메뉴가 열리면 장비창을 닫음
                    _isEquipmentMenuOpen = false;
                    _equipmentPanel.SetActive(false);
                }
                break;
            case 2: // 장비창
                _isEquipmentMenuOpen = !_isEquipmentMenuOpen;
                _equipmentPanel.SetActive(_isEquipmentMenuOpen);

                if (_isEquipmentMenuOpen)
                {
                    // 장비창이 열리면 ESC 메뉴를 닫음
                    _isESCMenuOpen = false;
                    _escMenuPanel.SetActive(false);
                }
                break;
            default:
                Debug.LogWarning($"menuCount 번호를 알려줘 : {menuCount}");
                break;


                // 게임 일시정지 처리 (옵션), 아직 결정못함 ESC판 열렸을 때 게임 멈추게 할지
                //Time.timeScale = _isMenuOpen ? 0 : 1;
        }
    }

    private void OnGUI()
    {
        float fps = 1.0f / _deltaTime;

        GUIStyle style = new GUIStyle
        {
            fontSize = 30,
            normal = { textColor = Color.red }
        };

        GUI.Label(new Rect(10, 10, 300, 100), $"FPS: {Mathf.Min(fps, 144):0.}", style);
    }

    // 테스트용
    public void OnClick_Test1()
    {
        Debug.Log("테스트용111");
    }
}
