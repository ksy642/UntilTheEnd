using UnityEngine;

namespace UntilTheEnd
{
    public class UICursor : Singleton<UICursor>
    {
        public FirstPersonCam firstPersonCam;

        [Header("UIs")]
        public GameObject defaultCursorIcon;
        public GameObject interactionCursorIcon; // 이친구는 다른오브젝트에 닿으면 켜져야하는 애임

        [Header("Cursor")]
        [SerializeField] private Texture2D _defaultCursorTexture;          // 커서이미지1
        [SerializeField] private Texture2D _interactionCursorIconTexture; // 커서이미지2
        [SerializeField] private bool _isCursorLockMode = false;
        [SerializeField] private bool _isIntercationMode = false;

        public void ChangeUI(bool isCursorLock)
        {
            _isCursorLockMode = isCursorLock;
            _ChangeCursorIcon();
        }

        private void _ChangeCursorIcon()
        {
            if (_isCursorLockMode)
            {
                Debug.LogError("마우스 커서 Stop Middle !!");

                if (firstPersonCam != null)
                {
                    firstPersonCam.trueMoveFalseStop = true;
                }

                // 커서를 숨기고 화면 중앙에 고정
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                defaultCursorIcon.SetActive(true);
                interactionCursorIcon.SetActive(false);
            }
            else
            {
                Debug.LogError("마우스 커서 Moving !!");

                if (firstPersonCam != null)
                {
                    firstPersonCam.trueMoveFalseStop = false;
                }

                // 커서를 보이고 락 해제
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                if (SceneController.instance.SceneName() == StringValues.Scene.lobby)
                {
                    // 포인터 커서가 좌상단(기본값)을 가르킴 "뾰족한 마우스 그 뾰족한 부분"
                    Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
                }
                else if(SceneController.instance.SceneName() == StringValues.Scene.main)
                {
                    // ★★★★★★★★★ 고쳐라 나중에 ★★★★★★★★
                    // 테스트용으로 여기 ESC를 눌렀을 때 마우스이미지 2번으로 바뀔거임
                    Cursor.SetCursor(_interactionCursorIconTexture, new Vector2(0, 0), CursorMode.Auto);
                }

                defaultCursorIcon.SetActive(false);
            }
        }

        // 게임매니저에서 들고옴 당연히 여기서 관리하는게 맞음
        public void CursorLock(bool lockCursor)
        {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.Confined;
            Cursor.visible = !lockCursor;
        }
    }
}