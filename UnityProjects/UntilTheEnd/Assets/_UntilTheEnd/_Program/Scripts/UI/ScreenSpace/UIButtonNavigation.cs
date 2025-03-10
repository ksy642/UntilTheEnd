using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UntilTheEnd
{
    /// <summary>
    /// UI를 켰을 때 키보드만으로 UI를 작동시킬 수 있게 하려는 코드
    /// 따라서 특정 씬과 관게없이 모든 씬에서 다 사용할 수 있게 만드는게 목적!!
    /// </summary>
    public class UIButtonNavigation : MonoBehaviour
    {
        [SerializeField] private List<Button> _navigateButtons;
        [SerializeField] private List<GameObject> _hoverImages; // 각 버튼과 매칭되는 Hover 이미지 리스트

        private bool _isNavigationLocked = false; // 버튼 네비게이션 잠금 여부
        private int _currentIndex = 0;

        public bool IsNavigationLocked
        {
            // 읽기 전용, IsNavigationLocked 이걸 통해서 현재 값 볼 수 있음
            get
            {
                return _isNavigationLocked;
            }
        }

        public bool SetNavigationLock(bool isLocked)
        {
            // 옵션 네비게이션 활성화/비활성화 메서드
            _isNavigationLocked = isLocked;

            return _isNavigationLocked;
        }

        public Button NavigateFirstButton(int index)
        {
            // 이걸 통해서 UI 켜지면 해당 UI버튼으로 이동 포커스 줌
            if (_navigateButtons != null && _navigateButtons.Count > 0
                 && index < _navigateButtons.Count)
            {
                return _navigateButtons[index];
            }

            return _navigateButtons[0];
        }


        private void Start()
        {
            //시작하자마자 첫번째 버튼 선택되어있게 설정
            if (_navigateButtons.Count > 0)
            {
                _SelectButton(_currentIndex);
            }
        }

        private void Update()
        {
            if (_isNavigationLocked)
            {
                // 네비게이션 잠겨있으면 동작하지 않음
                return;
            }

            KeyBoardNavigation();
        }

        public void KeyBoardNavigation()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // 위로 이동
                _Navigate(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // 아래로 이동
                _Navigate(1);
            }
        }

        private void _Navigate(int direction)
        {
            int newIndex = _currentIndex;

            do
            {
                // 방향에 따라 인덱스 이동
                newIndex += direction;
            }
            while (newIndex >= 0 && newIndex < _navigateButtons.Count 
            && !_navigateButtons[newIndex].interactable);


            // 리스트 범위를 벗어나지 않는 조건문
            if (newIndex >= 0 && newIndex < _navigateButtons.Count)
            {
                _UpdateHoverImage(_currentIndex, false);           // 현재 버튼의 Hover 이미지를 비활성화
                _navigateButtons[_currentIndex].OnDeselect(null); // 현재 선택된 버튼의 선택 해제

                _currentIndex = newIndex;
                _SelectButton(_currentIndex);
            }
        }

        private void _SelectButton(int index)
        {
            // 버튼에 포커스를 주기
            _navigateButtons[index].Select();

            // 현재 버튼의 Hover 이미지를 활성화
            _UpdateHoverImage(index, true);
        }

        private void _UpdateHoverImage(int index, bool isActive)
        {
            if (index >= 0 && index < _hoverImages.Count)
            {
                // Hover 이미지 활성화/비활성화
                _hoverImages[index].SetActive(isActive);
            }
        }

        // 마우스 Hover 시 해당 버튼으로 즉시 이동하도록 설정
        public void SetHoveredButton(GameObject hoveredButton)
        {
            int index = _navigateButtons.IndexOf(hoveredButton.GetComponent<Button>());

            if (index != -1 && _navigateButtons[index].interactable)
            {
                _NavigateToHoveredButton(index);
            }
        }

        private void _NavigateToHoveredButton(int index)
        {
            _UpdateHoverImage(_currentIndex, false);
            _currentIndex = index;  // 현재 인덱스를 Hover된 버튼으로 변경
            _SelectButton(_currentIndex);
        }
    }
}