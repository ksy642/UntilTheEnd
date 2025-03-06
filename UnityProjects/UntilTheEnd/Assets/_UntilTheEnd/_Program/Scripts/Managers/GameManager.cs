using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntilTheEnd
{
    /// <summary>
    /// Lobby창에서 GameManager유무를 체크하고 모든 매니저들을 관리하는 역할
    /// </summary>
    public class GameManager : DontDestroySingleton<GameManager>
    {
        // UIManager에서 게임 일시정지를 감지하도록 이벤트 추가
        // 일단, 기획이 완벽하지 않아서 ESC를 눌렀을 때 멈추가 하냐 마냐가 관건인데...결정나면 수정할듯함
        // 그리고 게임매니저에서 UI매니저를 소환하는것보다 이미 생성시켜놓고 이벤트로 주는게 더 괜찮은 방법같음
        public static event Action<bool> OnESCMenuToggled;

        [Header("매니저 프리팹들")]
        public List<GameObject> managerPrefabs;

        [Header("소환된 매니저들")]
        [SerializeField] private List<GameObject> _spawnedManagers = new List<GameObject>();

        [Header("ESC 메뉴")]
        public bool isESCMenuOpen = false;

        [Header("플레이어 동작여부")]
        public bool playerCanMove = false;
        public bool isPaused = false; // 게임 일시정지 여부

        [Header("Preload, Lobby에선 업데이트문 막기위한 Bool 값")]
        public bool isLobby = false;


        // 제일 처음 로비창을 들어왔을 때 실행되는 Start함수, 그 이후 로비창으로 되돌아오면 여긴 동작안함
        private void Start()
        {
            SceneManager.sceneLoaded += _OnSceneLoaded;
        }

        // 씬이 로드될 때 실행되는 메서드
        private void _OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.LogWarning("로비씬으로 돌아왔을 때 동작하는 이벤트");

            if (scene.name == StringValues.Scene.lobby
                || scene.name == StringValues.Scene.preload)
            {
                _spawnManagers();
                _InitializeManagers();

                CursorLock(false);
                //_SetManagersActivate(false);
            }

            if (scene.name == "Main") // 매인으로 넘어갔을 떼
            {
                _SetManagersActivate(true);

                
                CursorLock(true);
            }
        }

        // 모든 매니저들을 활성화/비활성화
        private void _SetManagersActivate(bool isActive)
        {
            foreach (var manager in _spawnedManagers)
            {
                if (manager != null)
                {
                    manager.SetActive(isActive);

                    Debug.Log($"{manager.name} {(isActive ? "활성화됨" : "비활성화됨")}");
                }
            }
        }

        // 리스트에 있는 매니저 프리팹들을 전부 생성
        private void _spawnManagers() 
        {
            foreach (GameObject prefab in managerPrefabs)
            {
                if (prefab != null)
                {
                    GameObject manager = Instantiate(prefab);
                    _spawnedManagers.Add(manager);

                    Debug.Log($"{manager.name} 생성 완료!");
                }
            }
        }

        private void _InitializeManagers()
        {
            UIManager.instance.InitializeUIManager();
            DreamManager.instance.InitializeDreamManager();
        }

        public void CursorLock(bool lockCursor)
        {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }

        // ESC 메뉴 상태 변경 이벤트 호출
        public void ToggleESCMenu(bool isOpen)
        {
            isESCMenuOpen = isOpen;
            OnESCMenuToggled?.Invoke(isESCMenuOpen);

            _SetPauseState(isESCMenuOpen);
        }

        // 게임 일시정지 상태 변경
        private void _SetPauseState(bool isPaused)
        {
            this.isPaused = isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }




        public void OnClick_BackToLobby()
        {
            Debug.LogWarning("로비씬으로 되돌아갑니다!!");
            SceneController.instance.LoadScene(StringValues.Scene.lobby);
        }


        private void OnDestroy()
        {
            // GameManager가 파괴될 때 이벤트 해제 (중복 등록 방지)
            // 아마 파괴될 일은 없지만 그래도 혹시나 싶어서 해둠
            SceneManager.sceneLoaded -= _OnSceneLoaded;
        }

    }
}