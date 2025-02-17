using UnityEngine;

namespace UntilTheEnd
{
    /// <summary>
    /// 아이템이 여러 개일꺼라 일단 싱글톤으로 해두긴 함...
    /// </summary>
    public class UIWorldCanvasController : Singleton<UIWorldCanvasController>
    {
        public GameObject worldCanvas;

        [Header("플레이어 대화중 !!")]
        public bool isWorldCanvasActive = false;

        private void Start()
        {
            HideUI();
        }

        // 특정 위치에서 UI 표시
        public void ShowUI(Vector3 position)
        {
            this.gameObject.transform.position = position + Vector3.up; // Y축 +1 높이
            worldCanvas.SetActive(true);
        }

        // UI 숨기기
        public void HideUI()
        {
            isWorldCanvasActive = false;
            worldCanvas.SetActive(false);
        }
    }
}
