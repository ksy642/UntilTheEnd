using UnityEngine;

public class Test1 : MonoBehaviour
{


    public void OnClick_ToMainScene()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }


    public string npcName = "Test1";  // NPC 이름
    public string sceneName = "MainTest"; // 현재 씬 이름
    public DialogManager dialogManager;


    void OnMouseDown() // NPC 클릭 시 대화 시작
    {
        if (dialogManager != null)
        {
            dialogManager.StartDialogue(sceneName, npcName);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log($"Starting dialogue for Scene: {sceneName}, NPC: {npcName}"); // 확인용 로그
            OnMouseDown();
        }

        if (dialogManager != null && dialogManager.isTalking && Input.GetKeyDown(KeyCode.Space)) // 스페이스바로 대화 진행
        {
            dialogManager.DisplayNextDialogue();
        }
    }
}
