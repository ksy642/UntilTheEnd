using UnityEngine;

namespace UntilTheEnd
{
    public class InteractNPCState : IPlayerState
    {
        public void EnterState(TestPlayer player)
        {
            Debug.LogWarning("NPC 대화 시작");

            // 현재 상호작용 중인 객체 가져오기
            GameObject npcObject = player.CurrentInteraction.Object;
            DialogueController dialogueController = npcObject?.GetComponent<DialogueController>(); // ?를 사용하여 안전하게 가져옴

            if (npcObject == null || dialogueController == null)
            {
                Debug.LogError("NPC 오브젝트 또는 NPC 데이터가 없습니다! 대화를 시작할 수 없음.");
                player.ChangeState(new IdleState());
                return;
            }

            // NPC의 sceneName과 npcName을 가져와서 StartDialogue() 호출
            DialogueManager.instance.StartDialogue(dialogueController.sceneName, dialogueController.npcName);
        }


        public void UpdateState(TestPlayer player)
        {
            if (DialogueManager.instance.uiDialogue.IsTyping)
            {
                // 현재 문장이 타이핑 중이면 바로 전체 출력
                DialogueManager.instance.uiDialogue.FinishTyping();
            }
            else
            {
                // 다음 문장 출력
                DialogueManager.instance.DisplayNextDialogue();
            }

            // 대화가 종료되었는지 체크
            if (!DialogueManager.instance.isTalking)
            {
                player.ChangeState(new IdleState());
            }
        }

        public void ExitState(TestPlayer player)
        {
            Debug.Log("NPC 대화 종료");
        }
    }
}
