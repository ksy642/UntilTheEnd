using UnityEngine;

public class Test1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnClick_ToMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
