using UnityEngine;

namespace UntilTheEnd
{
    public class DialogueController : MonoBehaviour, IInteractable
    {
        public string npcName;    // NPC 이름
        public string sceneName; // 현재 씬 이름
        private bool _isPlayerInRange = false;

        // 여기 함수는 플레이어 스크립트에서 SpaceBar를 누르면 동작하게 설정됨
        public void Interact()
        {
            Debug.Log(npcName + " 와 대화 시작 !!");

            DialogueManager.instance.StartDialogue(sceneName, npcName);
        }

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

         // 일단 SpaceBar로 동작하는게 너무 많아서 여기 없애
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
                    if (!dialogueManager.uiDialogue.IsTyping)
                    {
                        // 글자 출력이 끝났으면 다음 대화로 진행
                        dialogueManager.DisplayNextDialogue();
                    }
                    else
                    {
                        // 글자 출력 중이면 전체 문장을 즉시 보여줌
                        dialogueManager.uiDialogue.FinishTyping();
                    }
                }
            }
        }
    }
}