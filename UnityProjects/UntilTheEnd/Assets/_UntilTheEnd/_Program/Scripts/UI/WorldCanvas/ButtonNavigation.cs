using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UntilTheEnd
{
    public class ButtonNavigation : MonoBehaviour
    {
        public List<Button> sceneButtons;
        public List<GameObject> hoverImages; // 각 버튼과 매칭되는 Hover 이미지 리스트
        private int _currentIndex = 0;

        private void Start()
        {
            //시작하자마자 첫번째 버튼 선택되어있게 설정
            if (sceneButtons.Count > 0)
            {
                _SelectButton(_currentIndex);
            }
        }

        private void Update()
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
            // 현재 버튼의 Hover 이미지를 비활성화
            _UpdateHoverImage(_currentIndex, false);

            // 현재 선택된 버튼의 선택 해제
            sceneButtons[_currentIndex].OnDeselect(null);

            // 방향에 따라 인덱스 이동
            _currentIndex += direction;

            // 리스트 범위를 벗어나지 않도록 클램프
            _currentIndex = Mathf.Clamp(_currentIndex, 0, sceneButtons.Count - 1);

            // 새로운 버튼 선택
            _SelectButton(_currentIndex);
        }

        private void _SelectButton(int index)
        {
            // 버튼에 포커스를 주기
            sceneButtons[index].Select();

            // 현재 버튼의 Hover 이미지를 활성화
            _UpdateHoverImage(index, true);
        }

        private void _UpdateHoverImage(int index, bool isActive)
        {
            if (index >= 0 && index < hoverImages.Count)
            {
                // Hover 이미지 활성화/비활성화
                hoverImages[index].SetActive(isActive);
            }
        }
    }
}