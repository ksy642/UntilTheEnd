using System.Collections.Generic;

namespace UntilTheEnd
{
    /// <summary>
    /// 대화 순서를 관리
    /// _currentDialogue 관련 로직만 떼어냄
    /// </summary>
    public class DialogueProcessor
    {
        // 순차적으로 이야기를 하니 큐를 사용
        private Queue<DialogueData> _currentDialogueQueue;

        public bool HasNextDialogue
        {
            get
            {
                return _currentDialogueQueue != null && _currentDialogueQueue.Count > 0;
            }
        }

        // 대화 목록을 설정하고, 순서를 정렬한 후 큐에 삽입
        public void SetDialogueQueue(List<DialogueData> dialogues)
        {
            dialogues.Sort((d1, d2) => d1.numberCSV.CompareTo(d2.numberCSV)); // 정렬
            _currentDialogueQueue = new Queue<DialogueData>(dialogues);
        }

        // 다음 대사를 가져오기 (대사가 없으면 null 반환)
        public DialogueData GetNextDialogue()
        {
            return HasNextDialogue ? _currentDialogueQueue.Dequeue() : null;
        }

        // 대화 큐를 초기화 (비우기)
        public void ClearDialogue()
        {
            _currentDialogueQueue = null;
        }
    }
}
