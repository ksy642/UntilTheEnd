using UnityEngine;

public class FPS : MonoBehaviour
{
    private float _deltaTime = 0.0f;

    private void Start()
    {
        // 최대 FPS를 144로 제한
        Application.targetFrameRate = 140;
    }

    private void Update()
    {
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        float fps = 1.0f / _deltaTime;

        GUIStyle style = new GUIStyle
        {
            fontSize = 30,
            normal = { textColor = Color.red }
        };

        GUI.Label(new Rect(10, 10, 300, 100), $"FPS: {Mathf.Min(fps, 144):0.}", style);
    }
}
