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
                Debug.LogError("마우스 커서 Stop Middle !!");

                if (firstPersonCam != null)
                {
                    Debug.LogError("1");
                    firstPersonCam.trueMoveFalseStop = true;
                }
                Debug.LogError("2");

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
                Cursor.lockState = CursorLockMode.None;

                //Cursor.SetCursor(_interactionCursorIconTexture, new Vector2(0, 0), CursorMode.Auto);

                defaultCursorIcon.SetActive(false);
            }
        }

        private void Update()
        {

        }
    }
}