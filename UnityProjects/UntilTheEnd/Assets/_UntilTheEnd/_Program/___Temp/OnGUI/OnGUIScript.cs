using UnityEngine;

public class OnGUIScript : MonoBehaviour
{
    [Header("FPS")]
    private float _deltaTime = 0.0f;

    private void Update()
    {
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        float fps = 1.0f / _deltaTime;

        GUIStyle style = new GUIStyle
        {
            fontSize = 24,
            normal = { textColor = Color.red }
        };

        // 화면 크기를 가져와 우측 상단으로 위치 조정
        float screenWidth = Screen.width;
        float xPos = screenWidth - 125; // 화면 오른쪽에서 125px 떨어짐
        float yPos = 10; // 화면 상단에서 10px 떨어짐

        // FPS 표시
        GUI.Label(new Rect(xPos, yPos, 300, 100), $"FPS: {Mathf.Min(fps, 144):0.}", style);
    }
#endif
}