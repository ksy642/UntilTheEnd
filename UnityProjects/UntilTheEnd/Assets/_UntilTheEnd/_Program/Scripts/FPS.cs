using UnityEngine;

public class FPS : MonoBehaviour
{
    private float _deltaTime = 0.0f;

    private void Update()
    {
        // 매 프레임 걸린 시간을 저장
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
    }

    public void OnGUI()
    {
        float fps = 1.0f / _deltaTime;

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.red;

        GUI.Label(new Rect(10, 10, 300, 100), $"FPS: {fps:0.}", style);
    }
}
