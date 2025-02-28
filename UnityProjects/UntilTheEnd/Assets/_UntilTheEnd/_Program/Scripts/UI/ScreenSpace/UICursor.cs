using UnityEngine;
using UnityEngine.InputSystem;

namespace UntilTheEnd
{
    public class UICursor : Singleton<UICursor>
    {
        public FirstPersonCam firstPersonCam;

        [Header("UIs")]
        public GameObject defaultCursorIcon;
        public GameObject interactionCursorIcon;

        [SerializeField] private Texture2D _defaultCursorTexture;
        [SerializeField] private Texture2D _interactionCursorIconTexture;

        [Header("Cursor")]
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
                Debug.LogWarning("마우스 커서 Stop Middle !!");

                firstPersonCam.trueMoveFalseStop = true;

                // 커서를 숨기고 화면 중앙에 고정
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                defaultCursorIcon.SetActive(true);
                interactionCursorIcon.SetActive(false);
            }
            else
            {
                Debug.LogWarning("마우스 커서 Moving !!");

                firstPersonCam.trueMoveFalseStop = false;

                // 커서를 보이고 락 해제
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                defaultCursorIcon.SetActive(false);
            }
        }

        private void Update()
        {
            if (_isCursorLockMode)
            {
                //Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
                Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2f, Screen.height / 2f));
            }
        }
    }
}