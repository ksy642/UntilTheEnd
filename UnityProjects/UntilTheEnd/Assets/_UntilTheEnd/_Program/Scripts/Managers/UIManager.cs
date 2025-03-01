using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UntilTheEnd
{
    public class UIManager : DontDestroySingleton<UIManager>
    {
        [SerializeField] private GameObject _uiDialogue;
        private bool _isTransitioning = false; // 씬 전환 중 상태 플래그

        [Header("1번 : ESC")]
        public GameObject escMenuPanel; // 메뉴판 UI
        [SerializeField] private bool _isESCMenuOpen = false; // ESC 메뉴 상태

        [Header("2번 : Equipment")]
        [SerializeField] private GameObject _equipmentPanel; // 장비창 UI
        [SerializeField] private bool _isEquipmentMenuOpen = false; // 장비창 상태

        [Header("3번 : Quest, Obtain")]
        [SerializeField] private GameObject _questPanel;
        [SerializeField] private bool _isQuestMenuOpen = false; // 퀘스트창 UI
        [SerializeField] private GameObject _obtainPanel;
        [SerializeField] private bool _isObtainMenuOpen = false; // 아이템획득창 UI

        [Header("Fade")]
        [SerializeField] private CanvasGroup _fadeCanvasGroup;
        [SerializeField] private float _fadeDuration = 3f; // 페이드 시간


        private void Start()
        {
            Debug.LogWarning("[테스트용] 첫 시작은 메뉴판 다 꺼버리고 시작");
            ToggleMenu(0);

            // ESC 상태 변경 이벤트 구독
            GameManager.OnESCMenuToggled += _UpdateESCMenu;

            // ✅ InputManager의 이벤트를 구독
            InputListener.OnPressed_ESC += HandlePressed_ESC;
            InputListener.OnPressed_E += HandlePressed_E;
        }

        private void OnDestroy()
        {
            // 혹시 모를 경우를 대비해...메모리 누수 방지를 위해 이벤트 해제
            InputListener.OnPressed_ESC -= HandlePressed_ESC;
            InputListener.OnPressed_E -= HandlePressed_E;
        }

        private void Update()
        {
            if (_isTransitioning)
            {
                return; // 씬 전환 중에는 ESC 입력을 무시
            }

            InputListener.CheckInput();
        }

        private void HandlePressed_ESC()
        {
            if (_isEquipmentMenuOpen)
            {
                ToggleMenu(2); // 장비창이 열려있으면 ESC로 장비창을 닫음
            }
            else
            {
                ToggleMenu(1); // ESC 메뉴를 열거나 닫음
            }
        }

        private void HandlePressed_E()
        {
            if (!_isESCMenuOpen)  // ESC 메뉴가 열려있을 때는 무시
            {
                ToggleMenu(2); // 장비창 여닫기
            }
        }

        public void ToggleMenu(int menuCount)
        {
            switch (menuCount)
            {
                case 0:  //모든 메뉴판을 다 닫음
                    _uiDialogue.SetActive(false);

                    _isESCMenuOpen = false;
                    escMenuPanel.SetActive(false);

                    _isEquipmentMenuOpen = false;
                    _equipmentPanel.SetActive(false);

                    _isQuestMenuOpen = false;
                    _questPanel.SetActive(false);

                    _isObtainMenuOpen = false;
                    _obtainPanel.SetActive(false);

                    Debug.Log("모든 메뉴가 닫혔습니다.");
                    break;


                case 1: // ESC 메뉴

                    //UICursor.instance.ChangeUI(_isESCMenuOpen);

                    _isESCMenuOpen = !_isESCMenuOpen;
                    escMenuPanel.SetActive(_isESCMenuOpen);

                    // ★★★ 게임매니저한테 ESC 상태 전달 ★★★
                    GameManager.instance.ToggleESCMenu(_isESCMenuOpen);

                    if (_isESCMenuOpen)
                    {
                        // ESC 메뉴가 열리면 장비창을 닫음
                        _isEquipmentMenuOpen = false;
                        _equipmentPanel.SetActive(false);
                    }
                    break;


                case 2: // 장비창
                    _isEquipmentMenuOpen = !_isEquipmentMenuOpen;
                    _equipmentPanel.SetActive(_isEquipmentMenuOpen);

                    if (_isEquipmentMenuOpen)
                    {
                        // 장비창이 열리면 ESC 메뉴를 닫음
                        _isESCMenuOpen = false;
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
            _isESCMenuOpen = isOpen;
            escMenuPanel.SetActive(isOpen);

            if (_isESCMenuOpen)
            {
                _isEquipmentMenuOpen = false;
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
                    Debug.LogError("equipmentSlotPrefab이 null입니다. 슬롯 UI 프리팹을 확인하세요.");
                }
            }
        }
    }
}