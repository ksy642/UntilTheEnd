using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UntilTheEnd
{
    public class UIManager : DontDestroySingleton<UIManager>
    {
        public bool IsTransitioning
        {
            // 씬 전환 중 상태 플래그
            get;
            private set;
        }

        [SerializeField] private GameObject _uiDialogue;

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


        private void Start()
        {
            Debug.LogWarning("[테스트용] 첫 시작은 메뉴판 다 꺼버리고 시작");
            ToggleMenu(0);

            // ESC 상태 변경 이벤트 구독
            GameManager.OnESCMenuToggled += _UpdateESCMenu;
        }

        public void ToggleMenu(int menuCount)
        {
            switch (menuCount)
            {
                case 0:  //모든 메뉴판을 다 닫음
                    _uiDialogue.SetActive(false);

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

                    //UICursor.instance.ChangeUI(_isESCMenuOpen);

                    isESCMenuOpen = !isESCMenuOpen;
                    escMenuPanel.SetActive(isESCMenuOpen);

                    // ★★★ 게임매니저한테 ESC 상태 전달 ★★★
                    GameManager.instance.ToggleESCMenu(isESCMenuOpen);

                    if (isESCMenuOpen)
                    {
                        // ESC 메뉴가 열리면 장비창을 닫음
                        isEquipmentMenuOpen = false;
                        _equipmentPanel.SetActive(false);
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


        private void _UpdateESCMenu(bool isOpen)
        {
            isESCMenuOpen = isOpen;
            escMenuPanel.SetActive(isOpen);

            if (isESCMenuOpen)
            {
                isEquipmentMenuOpen = false;
                _equipmentPanel.SetActive(false);
            }
        }




        // UI 슬롯 오브젝트
        [SerializeField] private Dictionary<string, GameObject> _equipmentSlots = new Dictionary<string, GameObject>(); // UI 슬롯 오브젝트

        public GameObject equipmentSlotPrefab;

        //// UI 갱신 (EquipmentManager에서 호출)
        public void UpdateEquipmentUI(string slot, Item item)
        {
            Debug.LogWarning("아이템 닿은 후 UIManger에서 불러옴");


            if (_equipmentSlots.ContainsKey(slot))
            {
                GameObject slotUI = _equipmentSlots[slot];

                if (item != null)
                {
                    Debug.LogWarning("1 : " + slot);

                    // 아이템을 슬롯 UI에 표시
                    slotUI.GetComponentInChildren<UnityEngine.UI.Image>().sprite = item.iconSprite;


                }
                else
                {
                    Debug.LogWarning("2");

                    // 슬롯 UI 초기화 (아이템이 제거됨)
                    slotUI.GetComponentInChildren<UnityEngine.UI.Image>().sprite = null;
                }
            }
            else
            {
                // 슬롯 UI가 없다면 새로운 슬롯을 생성
                if (equipmentSlotPrefab != null)
                {
                    GameObject newSlotUI = Instantiate(equipmentSlotPrefab, _equipmentPanel.transform);
                    _equipmentSlots[slot] = newSlotUI;

                    if (item != null)
                    {
                        // 아이템을 설정
                        newSlotUI.GetComponentInChildren<UnityEngine.UI.Image>().sprite = item.iconSprite;
                    }
                }
                else
                {
                    Debug.LogError("EquipmentSlotPrefab이 null입니다. 슬롯 UI 프리팹을 확인하세요.");
                }
            }
        }
    }
}