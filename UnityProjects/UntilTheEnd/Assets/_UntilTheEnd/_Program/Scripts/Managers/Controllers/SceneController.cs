using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntilTheEnd
{
    public class SceneController : DontDestroySingleton<SceneController>
    {
        public bool isTransitioning = false; // 씬 전환 중 상태
        [SerializeField] private float _fadeDuration = 1.5f;

        public void LoadScene(string sceneName)
        {
            if (isTransitioning)
            {
                return; // 씬 전환 중이면 무시 (씬 이동 중에 메뉴판 못키게 설정)
            }
            else
            {
                if (UIManager.instance.escMenuPanel.activeSelf) // 메뉴가 활성화된 경우 닫음
                {
                    UIManager.instance.ToggleMenu(0);
                }
            }
            isTransitioning = true; // 씬 전환 시작

            StartCoroutine(_FadeAndLoadScene(sceneName));
        }

        // Fade In - Fade Out
        private IEnumerator _FadeAndLoadScene(string sceneName)
        {
            yield return StartCoroutine(_Fade(1)); // 페이드 아웃 (화면 어둡게)
            SceneManager.LoadScene(sceneName);
            yield return StartCoroutine(_Fade(0)); // 페이드 인 (화면 밝게)
            isTransitioning = false;
        }

        private IEnumerator _Fade(float targetAlpha)
        {
            float startAlpha = UIManager.instance.fadeCanvasGroup.alpha;
            float time = 0;

            if (targetAlpha == 1)
            {
                UIManager.instance.fadeCanvasGroup.gameObject.SetActive(true);
            }

            while (time < _fadeDuration)
            {
                time += Time.deltaTime;
                UIManager.instance.fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
                yield return null;
            }

            UIManager.instance.fadeCanvasGroup.alpha = targetAlpha;

            if (targetAlpha == 0)
            {
                UIManager.instance.fadeCanvasGroup.gameObject.SetActive(false);
            }
        }
    }
}
