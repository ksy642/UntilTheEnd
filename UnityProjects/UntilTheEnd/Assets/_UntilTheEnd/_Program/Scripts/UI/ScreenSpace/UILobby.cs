using UnityEngine;
using UnityEngine.UI;

namespace UntilTheEnd
{
    /// <summary>
    /// 최상단 UI관리
    /// </summary>
    public class UILobby : MonoBehaviour
    {
        public UIButtonNavigation uiButtonNavigation;
        public UILobbyOptions panel_UILobbyOptions;
        public GameObject panel_UILobbyDevelopmentTeam;

        private void Start()
        {
            //panel_Options.gameObject.SetActive(false);
            //panel_DevelopmentTeam.SetActive(false);
        }

        #region Panel_ButtonNavigation
        public void OnClick_NewGame()
        {
            Debug.LogWarning("새게임을 시작합니다...Main지하철씬으로 이동");
            SceneController.instance.LoadScene(StringValues.Scene.main);
        }

        public void OnClick_LoadGame()
        {
            Debug.LogWarning("저장된 곳에서 시작합니다...");
            SceneController.instance.LoadScene(StringValues.Scene.main);
        }

        public void OnClick_Options()
        {
            Debug.LogWarning("옵션활성화");
            
            uiButtonNavigation.SetNavigationLock(true);
            panel_UILobbyOptions.SetOptionsNavigationLock(false); // 옵션 네비게이션 활성화

            panel_UILobbyOptions.gameObject.SetActive(true);

            Button firstButtonUILobbyOptions = panel_UILobbyOptions.NavigateFirstButton(0);
            firstButtonUILobbyOptions.Select();
        }

        public void OnClick_DevelopmentTeam()
        {
            Debug.LogWarning("제작진 정보");

            uiButtonNavigation.SetNavigationLock(true);


            panel_UILobbyDevelopmentTeam.SetActive(true);
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
            Debug.Log("닫기버튼을 눌렀습니다.");

            uiButtonNavigation.SetNavigationLock(false);

            if(panel_UILobbyOptions.gameObject.activeSelf)
            {
                // 설정 창
                panel_UILobbyOptions.SetOptionsNavigationLock(true); // 옵션 네비게이션 비활성화

                panel_UILobbyOptions.gameObject.SetActive(false);

                Debug.Log("여기 되돌아온거아님?");
                Button firstButtonUIButtonNavigation = uiButtonNavigation.NavigateFirstButton(1);
                firstButtonUIButtonNavigation.Select();
            }
            else if (panel_UILobbyDevelopmentTeam.activeSelf)
            {
                // 제작진 창
                //panel_UILobbyOptions.SetOptionsNavigationLock(true); // 옵션 네비게이션 비활성화

                panel_UILobbyDevelopmentTeam.SetActive(false);

                Debug.Log("여기 되돌아온거아님?22");
                Button firstButtonUIButtonNavigation = uiButtonNavigation.NavigateFirstButton(2);
                firstButtonUIButtonNavigation.Select();
            }
        }
    }
}