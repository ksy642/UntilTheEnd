using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// UIManager 자식으로 Canvas와 EventSystem을 넣어서 캐릭터의 전반적인 UI를 넣을 예정
/// </summary>
public class UIManager : DontDestroySingleton<UIManager>
{
    [Header("ESC")]
    [SerializeField] private GameObject _escMenuPanel; // 메뉴판 UI
    [SerializeField] private bool _isMenuOpen = false;

    [Header("Fade")]
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private float _fadeDuration = 3f; // 페이드 시간

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(_FadeAndLoadScene(sceneName));
    }

    #region Fade In - Fade Out
    private IEnumerator _FadeAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(_Fade(1)); // 페이드 아웃 (화면을 어둡게)
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(_Fade(0)); // 페이드 인 (화면을 밝게)
    }

    private IEnumerator _Fade(float targetAlpha)
    {
        float startAlpha = _fadeCanvasGroup.alpha;
        float time = 0;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;
            _fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
            yield return null;
        }

        _fadeCanvasGroup.alpha = targetAlpha;
    }
    #endregion


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _ToggleMenu();
        }
    }

    private void _ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        _escMenuPanel.SetActive(_isMenuOpen);

        // 게임 일시정지 처리 (옵션)
        //Time.timeScale = _isMenuOpen ? 0 : 1;
    }
}
