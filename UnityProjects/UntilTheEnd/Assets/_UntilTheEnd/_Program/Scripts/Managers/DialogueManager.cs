using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public TextAsset csvFile;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueUI; // 대화상자 백그라운드
    public bool isTalking = false; // 대화 중인지 확인

    private List<Dialogue> _dialogues; // CSV에서 불러온 대화 데이터를 저장
    private Queue<Dialogue> _currentDialogue; // 현재 대화

    private void Start()
    {
        dialogueUI.SetActive(false); // 대화상자 백그라운드 비활성화
        _LoadDialogueData();
    }

    private void _LoadDialogueData()
    {
        _dialogues = new List<Dialogue>();

        if (csvFile != null)
        {
            string[] dataLines = csvFile.text.Split('\n');

            for (int i = 1; i < dataLines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(dataLines[i]))
                    continue;

                string[] rowData = dataLines[i].Split(',');  // ,를 통해 순서체크
                string sceneName = rowData[0].Trim();       // 씬 이름 
                int number = int.Parse(rowData[1]);        // 대화순서
                string npc = rowData[2].Trim();           // NPC 이름
                string answerStr = rowData[3].Trim();    // 정답처리 (주로 디폴트는 False)
                string dialog = rowData[4].Trim();      // 대화

                bool dialogueAnswer = false;
                if (answerStr.ToUpper() == "TRUE")  // "TRUE"일 경우 true로 변환
                {
                    dialogueAnswer = true;
                }

                Dialogue dialogueEntry = new Dialogue(sceneName, number, npc, dialogueAnswer, dialog);
                _dialogues.Add(dialogueEntry);
            }
        }
        else
        {
            Debug.LogError("CSV 파일이 없음");
        }
    }


    // 특정 NPC와 대화 시작
    public void StartDialogue(string sceneName, string npcName)
    {
        List<Dialogue> npcDialogues = GetDialoguesForNPC(sceneName, npcName);

        // 대화가 없다면 종료
        if (npcDialogues.Count == 0)
        {
            Debug.LogWarning($"대화가 없어서 종료, NPC: {npcName} Scene: {sceneName}");
            return;
        }

        // 대화 데이터를 번호 순으로 정렬
        npcDialogues.Sort((d1, d2) => d1.numberCSV.CompareTo(d2.numberCSV));

        isTalking = true;
        dialogueUI.SetActive(true);
        _currentDialogue = new Queue<Dialogue>(npcDialogues);

        DisplayNextDialogue();
    }


    // 다음 대화 표시
    public void DisplayNextDialogue()
    {
        if (_currentDialogue == null || _currentDialogue.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogue nextDialogue = _currentDialogue.Dequeue();
        dialogueText.text = nextDialogue.dialogueCSV;
    }


    // 대화 종료
    public void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);
    }


    // 특정 NPC와의 대화 데이터 찾기, ToLower를 통해 대소문자 안가림.
    public List<Dialogue> GetDialoguesForNPC(string sceneName, string npcName)
    {
        return _dialogues.FindAll(d => d.sceneNameCSV.Trim().ToLower() == sceneName.Trim().ToLower() && d.npcCSV.Trim().ToLower() == npcName.Trim().ToLower());
    }

}
