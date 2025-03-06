using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntilTheEnd
{
    public class SceneController : DontDestroySingleton<SceneController>
    {
        public bool isTransitioning = false; // 씬 전환 중 상태
        [SerializeField] private float _fadeDuration = 1.5f;

        public string SceneName()
        {
            // 현재 씬이 어디에 있는지 if문으로 제시할 때 대비해서 만들어둠
            return SceneManager.GetActiveScene().name;
        }

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
            if (Time.timeScale == 0)
            {
                // ESC를 눌러서 timeScale값이 0 인 상태, 따라서 1로 설정해주고 Lobby씬으로 보내면 된다.
                Time.timeScale = 1;
            }

            float startAlpha = UIManager.instance.fadeCanvasGroup.alpha;
            float time = 0;

            if (targetAlpha == 1)
            {
                UIManager.instance.fadeCanvasGroup.gameObject.SetActive(true);
            }

            while (time < _fadeDuration)
            {
                time += Time.deltaTime;

                // 무한 루프 방지: time이 너무 작은 값이면 강제로 탈출
                if (time >= _fadeDuration * 1.5f)
                {
                    Debug.LogError("_Fade() 루프 탈출! time이 예상보다 큼 = while문 break");
                    break;
                }

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
