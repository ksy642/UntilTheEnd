using UnityEngine;

namespace UntilTheEnd
{
    /// <summary>
    /// 아이템이 여러개일꺼라서 일단 싱글톤으로 해두긴함...
    /// </summary>
    public class UIItemController : Singleton<UIItemController>
    {
        private void Start()
        {
            // 시작 시 비활성화, 활성화 시켜놔도 어차피 안보이는 곳에 배치해놓긴함..
            this.gameObject.SetActive(false);
        }

        // 특정 위치에서 UI 표시
        public void ShowUI(Vector3 position)
        {
            this.gameObject.transform.position = position + Vector3.up; // Y축 +1 높이
            this.gameObject.SetActive(true);
        }

        // UI 숨기기
        public void HideUI()
        {
            this.gameObject.SetActive(false);
        }
    }
}
