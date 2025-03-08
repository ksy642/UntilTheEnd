using UnityEngine;

namespace UntilTheEnd
{
    public class UIManager : DontDestroySingleton<UIManager>
    {
        public UIDialogue uiDialogue;
        public CanvasGroup fadeCanvasGroup;

        [Header("1번 : 메뉴창 : ESC")]
        public bool isESCMenuOpen = false;
        public GameObject escMenuPanel;

        [Header("2번 : 장비창 : Equipment")]
        public bool isEquipmentMenuOpen = false;
        [SerializeField] private GameObject _equipmentPanel;

        [Header("3번 : 퀘스트창 : Quest")]
        public bool isQuestMenuOpen = false;
        [SerializeField] private GameObject _questPanel;

        [Header("4번 : 아이템획득창 : Obtain")]
        public bool isObtainMenuOpen = false;
        [SerializeField] private GameObject _obtainPanel;


        public void InitializeUIManager()
        {
            ToggleMenu(0); // 메뉴 다 끔
        }

        public void ToggleMenu(int menuCount)
        {
            switch (menuCount)
            {
                case 0:  //모든 메뉴판을 다 닫음
                    uiDialogue.gameObject.SetActive(false);

                    isESCMenuOpen = false;
                    escMenuPanel.SetActive(false);

                    isEquipmentMenuOpen = false;
                    _equipmentPanel.SetActive(false);

                    isQuestMenuOpen = false;
                    _questPanel.SetActive(false);

                    isObtainMenuOpen = false;
                    _obtainPanel.SetActive(false);

                    Debug.Log("모든 메뉴가 닫혔습니다.");
                    break;


                case 1: // ESC 메뉴
                    isESCMenuOpen = !isESCMenuOpen;
                    escMenuPanel.SetActive(isESCMenuOpen);


                    if (isESCMenuOpen)
                    {
                        // ESC 메뉴가 열리면 장비창을 닫음
                        isEquipmentMenuOpen = false;
                        _equipmentPanel.SetActive(false);
                        UICursor.instance.ChangeUI(false);
                    }
                    else
                    {
                        Debug.LogError("ESC메뉴창을 닫습니다.");
                        UICursor.instance.ChangeUI(true);
                    }
                    break;


                case 2: // 장비창
                    isEquipmentMenuOpen = !isEquipmentMenuOpen;
                    _equipmentPanel.SetActive(isEquipmentMenuOpen);

                    if (isEquipmentMenuOpen)
                    {
                        // 장비창이 열리면 ESC 메뉴를 닫음
                        isESCMenuOpen = false;
                        escMenuPanel.SetActive(false);
                    }
                    break;

                /*
                 * 퀘스트창과 아이템획득 했을 때 나오는 UI 구현해야함
                 */


                default:
                    Debug.LogWarning($"menuCount 번호를 알려줘 : {menuCount}");
                    break;


                    // 게임 일시정지 처리 (옵션), 아직 결정못함 ESC판 열렸을 때 게임 멈추게 할지
                    //Time.timeScale = _isMenuOpen ? 0 : 1;
            }
        }

        public void OnClick_BackToLobby()
        {
            GameManager.instance.OnClick_BackToLobby();
        }
    }
}