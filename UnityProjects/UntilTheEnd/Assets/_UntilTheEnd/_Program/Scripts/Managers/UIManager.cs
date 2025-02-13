using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UntilTheEnd
{
    public class UIManager : DontDestroySingleton<UIManager>
    {
        private bool _isTransitioning = false; // 씬 전환 중 상태 플래그

        [Header("1번 : ESC")]
        [SerializeField] private GameObject _escMenuPanel; // 메뉴판 UI
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

        [Header("FPS")]
        private float _deltaTime = 0.0f;


        private void Start()
        {
            Debug.LogWarning("[테스트용] 첫 시작은 메뉴판 다 꺼버리고 시작");
            _ToggleMenu(0);
        }

        private void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;

            if (_isTransitioning)
            {
                return; // 씬 전환 중에는 ESC 입력을 무시
            }

            // ESC 키 처리
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isEquipmentMenuOpen)
                {
                    // 장비창이 열려있으면 ESC로 장비창을 닫음
                    _ToggleMenu(2);
                }
                else
                {
                    // ESC 메뉴를 열거나 닫음
                    _ToggleMenu(1);
                }
            }

            // E 키 처리
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!_isESCMenuOpen) // ESC 메뉴가 열려있을 때는 무시
                {
                    _ToggleMenu(2); // 장비창 열기/닫기
                }
            }
        }


        private void _ToggleMenu(int menuCount)
        {
            switch (menuCount)
            {
                case 0:  //모든 메뉴판을 다 닫음
                    _isESCMenuOpen = false;
                    _escMenuPanel.SetActive(false);

                    _isEquipmentMenuOpen = false;
                    _equipmentPanel.SetActive(false);

                    _isQuestMenuOpen = false;
                    _questPanel.SetActive(false);

                    _isObtainMenuOpen = false;
                    _obtainPanel.SetActive(false);

                    Debug.Log("모든 메뉴가 닫혔습니다.");
                    break;


                case 1: // ESC 메뉴
                    _isESCMenuOpen = !_isESCMenuOpen;
                    _escMenuPanel.SetActive(_isESCMenuOpen);

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
                        _escMenuPanel.SetActive(false);
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

        // UI장비창 관련
        private HashSet<string> _updateEquipmentUI = new HashSet<string>();

        public void OnClick_MicroScopeButton() // 12개 다 찾았을 때 현미경 앞에서 누르는 버튼!! [마지막 동작 체크용]
        {
            // 초기화하고, 기존버튼 다시 체크
            _updateEquipmentUI.Clear();

            // _equipmentPanel의 자식들 중 이미 생성된 장비이미지들을 탐색
            foreach (Transform child in _equipmentPanel.transform)
            {
                Image image = child.GetComponent<Image>();
                if (image != null && image.sprite != null)
                {
                    _updateEquipmentUI.Add(image.sprite.name);
                }

            }
        }













        // Fade In - Fade Out 효과 주는 함수
        public void FadeToScene(string sceneName)
        {
            if (_isTransitioning)
            {
                return; // 씬 전환 중이면 무시 (씬 이동 중에 메뉴판 못키게 설정)
            }
            else
            {
                if (_escMenuPanel.activeSelf) // 메뉴가 활성화된 경우 닫음
                {
                    _ToggleMenu(0);
                }
            }
            _isTransitioning = true; // 씬 전환 시작
            StartCoroutine(_FadeAndLoadScene(sceneName));
        }

        #region Fade In - Fade Out
        private IEnumerator _FadeAndLoadScene(string sceneName)
        {
            yield return StartCoroutine(_Fade(1)); // 페이드 아웃 (화면을 어둡게)
            SceneManager.LoadScene(sceneName);
            yield return StartCoroutine(_Fade(0)); // 페이드 인 (화면을 밝게)
            _isTransitioning = false; // 씬 전환 완료
        }

        private IEnumerator _Fade(float targetAlpha)
        {
            float startAlpha = _fadeCanvasGroup.alpha;
            float time = 0;

            if (targetAlpha == 1)
            {
                _fadeCanvasGroup.gameObject.SetActive(true);
            }

            while (time < _fadeDuration)
            {
                time += Time.deltaTime;
                _fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeDuration);
                yield return null;
            }

            _fadeCanvasGroup.alpha = targetAlpha;

            if (targetAlpha == 0)
            {
                _fadeCanvasGroup.gameObject.SetActive(false);
            }
        }
        #endregion













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










        private void OnGUI()
        {
            float fps = 1.0f / _deltaTime;

            GUIStyle style = new GUIStyle
            {
                fontSize = 24,
                normal = { textColor = Color.red }
            };

            // 화면 크기를 가져와 우측 상단으로 위치 조정
            float screenWidth = Screen.width;
            float xPos = screenWidth - 125; // 화면 오른쪽에서 150px 떨어짐
            float yPos = 10; // 화면 상단에서 10px 떨어짐

            // FPS 표시
            GUI.Label(new Rect(xPos, yPos, 300, 100), $"FPS: {Mathf.Min(fps, 144):0.}", style);
        }
    }
}