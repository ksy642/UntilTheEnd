using UnityEngine;

/// <summary>
/// UIManager 자식으로 Canvas와 EventSystem을 넣어서 캐릭터의 전반적인 UI를 넣을 예정
/// </summary>
public class UIManager : DontDestroySingleton<UIManager>
{
    [SerializeField] private GameObject panel_ESC; // 메뉴판 UI
    [SerializeField] private bool _isMenuOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _ToggleMenu();
        }
    }

    private void _ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        panel_ESC.SetActive(_isMenuOpen);

        // 게임 일시정지 처리 (옵션)
        //Time.timeScale = _isMenuOpen ? 0 : 1;
    }
}
