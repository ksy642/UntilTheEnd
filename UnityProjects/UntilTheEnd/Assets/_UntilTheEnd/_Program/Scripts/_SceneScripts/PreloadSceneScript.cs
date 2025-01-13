using UnityEditor.Search;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadSceneScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
