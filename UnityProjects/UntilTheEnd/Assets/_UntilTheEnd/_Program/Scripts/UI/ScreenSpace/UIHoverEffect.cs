using UnityEngine;
using UnityEngine.EventSystems;

namespace UntilTheEnd
{
    public class UIHoverEffect : MonoBehaviour, IPointerEnterHandler
    {
        private UIButtonNavigation _navigation;

        private void Start()
        {
            _navigation = FindFirstObjectByType<UIButtonNavigation>();  // 네비게이션 스크립트 찾기
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_navigation != null)
            {
                _navigation.SetHoveredButton(this.gameObject);  // 마우스가 올라간 버튼 전달
            }
        }
    }
}
