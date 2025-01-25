using TMPro;
using UnityEngine;

/// <summary>
/// 타이틀 화면 버전 텍스트 표시
/// </summary>
public class VersionText : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = $"Ver {Application.version}";
    }
}
