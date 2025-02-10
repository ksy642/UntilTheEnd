using UnityEngine;

namespace UntilTheEnd
{
    public class InteractItemState : IPlayerState
    {
        public void EnterState(TestPlayer player)
        {
            Debug.Log("아이템 줍기");
         
            // 아이템 줍는 기능
            //EquipmentManager.instance.PickUpItem(player.InteractableObject);

            // 아이템 줍기 후 대기 상태로 돌아감
            player.ChangeState(new IdleState());
        }

        public void UpdateState(TestPlayer player) { }
        public void ExitState(TestPlayer player) { }
    }
}