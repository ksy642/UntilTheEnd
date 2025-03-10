using UnityEngine;

namespace UntilTheEnd
{
    public class InputController : DontDestroySingleton<InputController>
    {
        private void Start()
        {
            InputListener.OnPressed_ESC += HandlePressed_ESC;
            InputListener.OnPressed_E += HandlePressed_E;
        }

        private void Update()
        {
            if (SceneController.instance.isTransitioning)
            {
                // 씬 전환 중에는 입력을 무시
                return;
            }

            // 입력 감지 실행
            InputListener.CheckInput();
        }

        private void HandlePressed_ESC()
        {
            Debug.LogError("ESC 눌렀음");


            if (UIManager.instance.isEquipmentMenuOpen)
            {
                UIManager.instance.ToggleMenu(2); // 장비창이 열려있으면 ESC로 닫기
            }
            else
            {
                // timeScale 값이 0이면 false, 1이면 true를 반환
                bool isPaused = Time.timeScale == 1;

                GameManager.instance.TimeStop(isPaused); // 0값이면 1로변환, 1값이면 0으로 변환
                UIManager.instance.ToggleMenu(1);       // ESC 메뉴 열기/닫기
            }
        }

        private void HandlePressed_E()
        {
            Debug.LogError("E눌렀음");

            if (!UIManager.instance.isESCMenuOpen)  // ESC 메뉴가 열려있을 때는 무시
            {
                UIManager.instance.ToggleMenu(2); // 장비창 여닫기
            }
        }

        // 라이프사이클 제일 마지막
        private void OnDestroy()
        {
            // 혹시 파괴가 된다면 메모리 누수 방지를 위해 이벤트 해제
            InputListener.OnPressed_ESC -= HandlePressed_ESC;
            InputListener.OnPressed_E -= HandlePressed_E;
        }
    }
}