using UnityEngine;

namespace UntilTheEnd
{
    public class IdleState : IPlayerState
    {
        public void EnterState(TestPlayer player)
        {
            Debug.Log(player.CurrentInteraction + " [IdleState] : 플레이어가 대기 상태에 들어옴.");
        }

        public void UpdateState(TestPlayer player)
        {
            if (player.CurrentInteraction.Object != null)
            {
                switch (player.CurrentInteraction.Type)
                {
                    case TestPlayer.InteractionType.Item:
                        Debug.LogWarning("[IdleState] : 아이템");
                        player.ChangeState(new InteractItemState());
                        break;
                    case TestPlayer.InteractionType.Door:
                        Debug.LogWarning("[IdleState] : 문");
                        player.ChangeState(new InteractDoorState());
                        break;
                    case TestPlayer.InteractionType.NPC:
                        Debug.LogWarning("[IdleState] : NPC");
                        player.ChangeState(new InteractNPCState());
                        break;
                }
            }
        }

        public void ExitState(TestPlayer player)
        {
            Debug.Log("대기 상태 종료 " + player.CurrentInteraction);
        }
    }
}