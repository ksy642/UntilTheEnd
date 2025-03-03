using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadSceneScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Preload에서 시작하면 바로 Lobby씬으로 보낸다.");
        OnClick_ToLobbyScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnClick_ToLobbyScene()
    {
        SceneManager.LoadScene(StringValues.Scene.lobby);
    }
}
