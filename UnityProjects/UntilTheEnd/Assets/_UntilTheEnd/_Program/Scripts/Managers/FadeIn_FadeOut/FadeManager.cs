using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : DontDestroySingleton<FadeManager>
{
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [SerializeField] private float _fadeDuration = 3f; // 페이드 시간

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(_FadeAndLoadScene(sceneName));
    }

    private IEnumerator _FadeAndLoadScene(string sceneName)
    {
        // 페이드 아웃 (화면을 어둡게)
        yield return StartCoroutine(_Fade(1));

        SceneManager.LoadScene(sceneName);

        // 페이드 인 (화면을 밝게)
        yield return StartCoroutine(_Fade(0));
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
}
