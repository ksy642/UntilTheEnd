using UnityEngine;

namespace UntilTheEnd
{
    public class UILobby : MonoBehaviour
    {
        public void OnClick_ToMainScene()
        {
            Debug.LogWarning("Main지하철씬으로 이동");
            UIManager.instance.FadeToScene(StringValues.Scene.main);
        }

        public void OnClick_Options()
        {
            Debug.LogWarning("옵션활성화");
        }

        public void OnClick_DevelopmentTeam()
        {
            Debug.LogWarning("제작진 정보");
        }

        public void OnClick_EndGame()
        {
            Debug.LogWarning("게임을 종료합");
            Application.Quit();
        }
    }
}