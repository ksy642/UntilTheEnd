using UnityEngine;

namespace UntilTheEnd
{
    public class UILobbyOptions : UIButtonNavigation
    {
        [Header("UILobbyOptions.cs  ... ↑↑↑ UIButtonNavigation 상속받는 중 ↑↑↑")]
        public bool isOptionsNavigationLocked = false;
        public bool check;

        private void Start()
        {
            Debug.Log($"Navigation Lock State (After): {IsNavigationLocked}");

            SetNavigationLock(true);

        }

        private void Update()
        {
            if (isOptionsNavigationLocked)
            {
                // 옵션이 잠겨있으면 동작하지 않음
                return;
            }

            KeyBoardNavigation();
        }

        // 옵션 네비게이션 활성화/비활성화 메서드 => UILobby.cs 에서 활용해줌 !!!
        public void SetOptionsNavigationLock(bool isLocked)
        {
            isOptionsNavigationLocked = isLocked;
        }

        public void Onclick_Test1(int Test1)
        {
            Debug.Log(Test1);
        }
    }
}