using UnityEngine;

namespace UntilTheEnd
{
    public class IdleState : IPlayerState
    {
        public void EnterState(TestPlayer player)
        {
            Debug.Log("▶ 플레이어: 대기 상태");
        }

        public void UpdateState(TestPlayer player)
        {
            // SpaceBar 입력 감지 후 상태 변경
            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (player.CurrentInteraction)
                {
                    case TestPlayer.InteractionType.Item:
                        player.ChangeState(new InteractItemState());
                        break;
                    case TestPlayer.InteractionType.Door:
                        player.ChangeState(new InteractDoorState());
                        break;
                    case TestPlayer.InteractionType.NPC:
                        player.ChangeState(new InteractNPCState());
                        break;
                }
            }
        }

        public void ExitState(TestPlayer player)
        {
            Debug.Log("대기 상태 종료");
        }
    }
}