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
        [Header("매니저들")]
        public List<GameObject> managerPrefabs;
        private List<GameObject> _spawnedManagers = new List<GameObject>();

        [Header("ESC 메뉴")]
        public bool isESCMenuOpen = false;

        [Header("플레이어 동작여부")]
        public bool playerCanMove = false;

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

                //_SetManagersActivate(false);
                UICursor.instance.CursorLock(false);
            }

            if (scene.name == "Main") // 매인으로 넘어갔을 떼
            {
                //_SetManagersActivate(true);
                UICursor.instance.CursorLock(true);
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








        // ESC 메뉴 게임 일시정지 상태 변경
        public void TimeStop(bool timeStop)
        {
            Time.timeScale = timeStop ? 0 : 1;
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