using UnityEngine;

namespace UntilTheEnd
{
    public class InteractDoorState : IPlayerState
    {
        public void EnterState(TestPlayer player)
        {
            Debug.Log("문 열기");
            
            // 문 여는 기능
            //DoorOpen door = player.InteractableObject.GetComponent<DoorOpen>();
            
            //if (door != null)
            //{
            //    door.OpenDoor();
            //}

            // 문을 연 후 다시 대기 상태
            player.ChangeState(new IdleState());
        }

        public void UpdateState(TestPlayer player) { }
        public void ExitState(TestPlayer player) { }
    }
}