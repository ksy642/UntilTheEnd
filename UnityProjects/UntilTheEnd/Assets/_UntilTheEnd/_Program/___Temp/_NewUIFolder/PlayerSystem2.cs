using UnityEngine;
using Cursor = UnityEngine.Cursor;


public class PlayerSystem2 : MonoBehaviour
{
    [Header("Cursor")]
    public bool isCursorLock = false;
    public UICursor2 uiCursor2;

    private void Start()
    {
        uiCursor2.ChangeUI(isCursorLock);
    }

    private void Update()
    {
        _UpdateCursor();
    }

    private void _UpdateCursor()
    {
        if (isCursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isCursorLock)
            {
                isCursorLock = false;
            }
            else
            {
                isCursorLock = true;
            }
        }
    }
}