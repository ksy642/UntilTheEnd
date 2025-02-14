using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UntilTheEnd
{
    /// <summary>
    /// 현재 대화체 매니저가 너무 많은 책임을 가지고 있다 판단...좀 스크립트를 쪼개야겠음
    /// 매니저 말 그대로 관리만하고 
    /// </summary>
    public class DialogueManager : Singleton<DialogueManager>
    {
        public UIDialogue uiDialogue;
        public TextAsset csvFile;
        public bool isTalking = false;

        private DialogueLoader _dialogueLoader;
        private DialogueProcessor _dialogueProcessor;
        
        

        private void Start()
        {
            _dialogueLoader = new DialogueLoader(csvFile);
            _dialogueProcessor = new DialogueProcessor();
            uiDialogue = FindFirstObjectByType<UIDialogue>();

            List<Dialogue> dialogues = _dialogueLoader.LoadDialogues();
            _dialogueProcessor.SetDialogueQueue(dialogues);
        }

        public void StartDialogue(string sceneName, string npcName)
        {
            List<Dialogue> npcDialogues = _dialogueLoader.LoadDialogues().FindAll(d => d.sceneNameCSV == sceneName && d.npcCSV == npcName);

            if (npcDialogues.Count == 0)
            {
                Debug.LogWarning($"대화가 없음: NPC {npcName} Scene {sceneName}");
                return;
            }

            isTalking = true;
            _dialogueProcessor.SetDialogueQueue(npcDialogues);
            uiDialogue.ShowDialogueUI();

            DisplayNextDialogue();
        }

        public void DisplayNextDialogue()
        {
            if (!_dialogueProcessor.HasNextDialogue)
            {
                EndDialogue();
                return;
            }

            Dialogue nextDialogue = _dialogueProcessor.GetNextDialogue();
            uiDialogue.DisplayDialogueText(nextDialogue.dialogueCSV);
        }

        public void EndDialogue()
        {
            isTalking = false;
            _dialogueProcessor.ClearDialogue();
            uiDialogue.HideDialogueUI();
        }
    }
}


/*
public class DialogueManager : Singleton<DialogueManager>
{
    public TextAsset csvFile;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBackGroundImage;// 대화상자 백그라운드
    public bool isTalking = false;   // 대화 중인지 확인
    public bool isTyping = false;   // 타이핑 중일 때
    public float typingSpeed = 0.05f;

    private List<Dialogue> _dialogues; // CSV에서 불러온 대화 데이터를 저장
    private Queue<Dialogue> _currentDialogue; // 현재 대화
    private Dialogue _nextDialogue;
    private Coroutine _nextDialogueCoroutine;

    private List<Dialogue> _GetDialoguesForNPC(string sceneName, string npcName)
    {
        // 특정 NPC와의 대화 데이터 찾기, ToLower를 통해 대소문자 안가림.
        return _dialogues.FindAll(d => d.sceneNameCSV.Trim().ToLower() == sceneName.Trim().ToLower() && d.npcCSV.Trim().ToLower() == npcName.Trim().ToLower());
    }

    private void Start()
    {
        dialogueText.text = ""; // 기존 텍스트 초기화
        dialogueBackGroundImage.SetActive(false); // 대화상자 백그라운드 비활성화
        _LoadDialogueData();

        //  Peek()는 대화 큐에서 다음 대화 데이터를 확인할 뿐, 제거하지 않습니다.
        //  Dequeue()는 큐에서 데이터를 가져오고 이후 큐에서 제거
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
        List<Dialogue> npcDialogues = _GetDialoguesForNPC(sceneName, npcName);

        if (npcDialogues.Count == 0) // 대화가 없다면 종료
        {
            Debug.LogWarning($"대화가 없어서 종료, NPC: {npcName} Scene: {sceneName}");
            return;
        }

        // 대화 데이터를 번호 순으로 정렬
        npcDialogues.Sort((d1, d2) => d1.numberCSV.CompareTo(d2.numberCSV));

        isTalking = true;
        dialogueBackGroundImage.SetActive(true);
        _currentDialogue = new Queue<Dialogue>(npcDialogues);

        DisplayNextDialogue();
    }

    // 다음 대화 표시
    public void DisplayNextDialogue()
    {
        if (_currentDialogue == null || _currentDialogue.Count == 0)
        {
            // 문장 끄트머리 도착했을 때 실행
            EndDialogue();
            return;
        }

        _nextDialogue = _currentDialogue.Dequeue();
        dialogueText.text = _nextDialogue.dialogueCSV;
        _nextDialogueCoroutine = StartCoroutine(_TypeDialogue(dialogueText.text));
    }

    // 문장 적히는 도중에 스페이스바 눌렀을 때 실행되는 함수
    public void FinishCurrentTyping()
    {
        if (_nextDialogueCoroutine != null)
        {
            // 현재 대화 문장을 모두 출력
            StopCoroutine(_nextDialogueCoroutine);
            dialogueText.text = _nextDialogue.dialogueCSV;//_currentDialogue.Peek().dialogueCSV;
        }

        isTyping = false;

        if (_currentDialogue == null || _currentDialogue.Count == 0)
        {
            // 마지막 문장이 출력되는 도중에 FinishCurrentTyping() 함수를 실행 시켰을 때
            StopCoroutine(_nextDialogueCoroutine);
            return;
        }
    }

    // 글자 타이핑 효과 코루틴
    private IEnumerator _TypeDialogue(string dialogue)
    {
        isTyping = true;

        dialogueText.text = ""; // 기존 텍스트 초기화

        foreach (char letter in dialogue)
        {
            dialogueText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed); // 일정 시간 대기
        }

        isTyping = false; //모든 문자가 출력되면 타이핑 상태를 종료"
    }

    // 대화 종료
    public void EndDialogue()
    {
        if (_nextDialogueCoroutine != null)
        {
            StopCoroutine(_nextDialogueCoroutine);
        }

        _currentDialogue = null;
        isTalking = false;
        dialogueBackGroundImage.SetActive(false);
        dialogueText.text = ""; // 기존 텍스트 초기화
    }
}
*/