using System.Collections.Generic;
using UnityEngine;

namespace UntilTheEnd
{
    public class EquipmentManager : DontDestroySingleton<EquipmentManager>
    {
        public bool isInteractedObject = false;

        // 왼손엔 후레시, 오른손엔 그 외 모든것?
        private Dictionary<string, Item> _equippedItems = new Dictionary<string, Item>();


        // 장비 장착
        public void EquipItem(string slot, Item item)
        {
            if (item == null)
            {
                Debug.LogWarning($"장착할 아이템이 없습니다. slot: {slot}");
                return;
            }


            // 아이템이 장착 가능한지 확인
            if (!item.isEquipable)
            {
                Debug.LogWarning($"'{item.name}'은(는) 장착할 수 없는 아이템입니다.");
                //return;
            }


            // 기존 슬롯에 아이템이 있으면 로그를 출력
            if (_equippedItems.ContainsKey(slot))
            {
                Debug.Log($"슬롯 '{slot}'의 기존 아이템 '{_equippedItems[slot].name}'을(를) 제거하고 '{item.name}'을(를) 장착합니다.");
            }
            else
            { 
                Debug.Log($"'{item.name}'을(를) '{slot}' 슬롯에 처음으로 장착합니다.");
            }

            _equippedItems[slot] = item; // 슬롯에 새로운 아이템을 추가 또는 업데이트
            UIManager.instance.UpdateEquipmentUI(slot, item); // UI 갱신 호출
        }


        /*
        // 장비 해제
        public void UnequipItem(string slot)
        {
            if (equippedItems.ContainsKey(slot))
            {
                Debug.Log($"{slot} 슬롯에서 {equippedItems[slot].name}을(를) 제거합니다.");
                equippedItems.Remove(slot);

                // 필요 시 UI 갱신 호출
                UIManager.instance.UpdateEquipmentUI(slot, null);
            }
        }

        // 장비 데이터 가져오기
        public Item GetEquippedItem(string slot)
        {
            return equippedItems.ContainsKey(slot) ? equippedItems[slot] : null;
        }
        */
    }
}