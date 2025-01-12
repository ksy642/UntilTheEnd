using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class UIManager : DontDestroySingleton<UIManager>
{
    private bool _isTransitioning = false; // 씬 전환 중 상태 플래그

    [Header("ESC")]
    [SerializeField] private GameObject _escMenuPanel; // 메뉴판 UI
    [SerializeField] private bool _isMenuOpen = false;

    [Header("Fade")]
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private float _fadeDuration = 3f; // 페이드 시간

    [Header("FPS")]
    private float _deltaTime = 0.0f;

    
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
                _ToggleMenu();
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
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _ToggleMenu();
            }
        }
    }

    private void _ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        _escMenuPanel.SetActive(_isMenuOpen);

        // 게임 일시정지 처리 (옵션)
        //Time.timeScale = _isMenuOpen ? 0 : 1;
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
