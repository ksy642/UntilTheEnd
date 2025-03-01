using UnityEngine;

public class UICursor2 : MonoBehaviour
{
    [Header("UIs")]
    public GameObject centerCursorIcon;
    public GameObject centerCursorHoverIcon;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D cursorHoverTexture;

    [Header("Cursor")]
    [SerializeField] private bool isCursorLockMode = false;
    [SerializeField] private bool isIntercationMode = false;

    // 마우스 커서에 따른 카메라 이동 제한용
    public PlayerSystem2 playerSystem2;

    public void ChangeUI(bool isCursorLock)
    {
        isCursorLockMode = isCursorLock;
        _ChangeCursorIcon();
    }


    private void _ChangeCursorIcon()
    {

        if (isCursorLockMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Debug.LogError("3");
            centerCursorIcon.SetActive(true);
            centerCursorHoverIcon.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Debug.LogError("2번");
            centerCursorIcon.SetActive(false);
            centerCursorHoverIcon.SetActive(false);
        }
    }
}
