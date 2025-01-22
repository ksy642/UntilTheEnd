using UnityEngine;

public class Test1 : MonoBehaviour
{


    public void OnClick_ToMainScene()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }


    public string npcName;  // NPC 이름
    public string sceneName; // 현재 씬 이름
    public DialogueManager dialogueManager;


    void OnMouseDown() // NPC 클릭 시 대화 시작
    {
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(sceneName, npcName);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log($"씬 이름 : {sceneName}, NPC 이름 : {npcName}"); // 확인용 로그
            OnMouseDown();
        }

        if (dialogueManager != null && dialogueManager.isTalking && Input.GetKeyDown(KeyCode.Space)) // 스페이스바로 대화 진행
        {
            dialogueManager.DisplayNextDialogue();
        }
    }
}
