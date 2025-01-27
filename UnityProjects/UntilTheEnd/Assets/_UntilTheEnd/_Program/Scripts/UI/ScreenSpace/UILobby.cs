using UnityEngine;

namespace UntilTheEnd
{
    /// <summary>
    /// 최상단 UI관리
    /// </summary>
    public class UILobby : MonoBehaviour
    {
        public UIButtonNavigation buttonNavigation;
        public UILobbyOptions panel_Options;
        public GameObject panel_DevelopmentTeam;

        private void Start()
        {
            //panel_Options.gameObject.SetActive(false);
            //panel_DevelopmentTeam.SetActive(false);
        }

        #region Panel_ButtonNavigation
        public void OnClick_ToMainScene()
        {
            Debug.LogWarning("Main지하철씬으로 이동");
            UIManager.instance.FadeToScene(StringValues.Scene.main);
        }

        public void OnClick_Options()
        {
            Debug.LogWarning("옵션활성화");

            buttonNavigation.SetNavigationLock(true);
            panel_Options.SetOptionsNavigationLock(false); // 옵션 네비게이션 활성화

            panel_Options.gameObject.SetActive(true);
        }

        public void OnClick_DevelopmentTeam()
        {
            Debug.LogWarning("제작진 정보");

            buttonNavigation.SetNavigationLock(true);


            panel_DevelopmentTeam.SetActive(true);
        }

        public void OnClick_EndGame()
        {
            Debug.LogWarning("게임을 종료합");
            Application.Quit();
        }
        #endregion


        // 우측상단의 X 버튼 눌렀을 때
        public void OnClick_ClosePanel()
        {
            buttonNavigation.SetNavigationLock(false);

            if(panel_Options.gameObject.activeSelf || panel_DevelopmentTeam.activeSelf)
            {
                panel_Options.SetOptionsNavigationLock(true); // 옵션 네비게이션 비활성화

                panel_Options.gameObject.SetActive(false);
                panel_DevelopmentTeam.SetActive(false);
            }
        }
    }
}