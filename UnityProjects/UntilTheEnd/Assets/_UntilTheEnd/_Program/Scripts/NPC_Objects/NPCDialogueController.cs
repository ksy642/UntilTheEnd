using UnityEngine;

namespace UntilTheEnd
{
    public class NPCDialogueController : MonoBehaviour
    {
        public string npcName;    // NPC 이름
        public string sceneName; // 현재 씬 이름
        private bool _isPlayerInRange = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(StringValues.Tag.player))
            {
                _isPlayerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(StringValues.Tag.player))
            {
                _isPlayerInRange = false;
            }
        }

        private void Update()
        {
            if (_isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
            {
                var dialogueManager = DialogueManager.instance;

                if (!dialogueManager.isTalking)
                {
                    // 대화 중이 아니면 시작
                    dialogueManager.StartDialogue(sceneName, npcName);
                }
                else
                {
                    // 대화 중이라면 현재 상태 확인
                    if (!dialogueManager.isTyping)
                    {
                        // 글자 출력이 끝났으면 다음 대화로 진행
                        dialogueManager.DisplayNextDialogue();
                    }
                    else
                    {
                        // 글자 출력 중이면 전체 문장을 즉시 보여줌
                        dialogueManager.FinishCurrentTyping();
                    }
                }
            }
        }
    }
}