using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UntilTheEnd
{
    public class EquipmentManager : DontDestroySingleton<EquipmentManager>
    {
        public bool isInteractedObject = false;

        // 왼손엔 후레시, 오른손엔 그 외 모든것?, 장비 슬롯 데이터 관리
        private Dictionary<string, Item> _equippedItems = new Dictionary<string, Item>();
        private Dictionary<string, GameObject> _equipmentSlots = new Dictionary<string, GameObject>();

        [SerializeField] private GameObject _equipmentPanel; // 장비창 UI
        [SerializeField] private GameObject _equipmentSlotPrefab; // 슬롯 프리팹

        // 장비 데이터 가져오기
        public Item GetEquippedItem(string slot)
        {
            return _equippedItems.ContainsKey(slot) ? _equippedItems[slot] : null;
        }

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
                
                // 테스트 때문에 주석
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
            UI_UpdateEquipment(slot, item); // UI 갱신
        }


        
        // 장비 해제
        public void UnequipItem(string slot)
        {
            if (_equippedItems.ContainsKey(slot))
            {
                Debug.Log($"{slot} 슬롯에서 {_equippedItems[slot].name} 제거");
                _equippedItems.Remove(slot);

                // 필요 시 UI 갱신 호출
                UI_UpdateEquipment(slot, null);
            }
        }


        // UI 갱신 (슬롯 생성 및 아이템 아이콘 표시)
        private void UI_UpdateEquipment(string slot, Item item)
        {
            if (_equipmentSlots.ContainsKey(slot))
            {
                GameObject slotUI = _equipmentSlots[slot];

                if (item != null)
                {
                    slotUI.GetComponentInChildren<Image>().sprite = item.iconSprite;
                }
                else
                {
                    slotUI.GetComponentInChildren<Image>().sprite = null;
                }
            }
            else
            {
                // 새로운 슬롯을 생성하여 UI에 추가
                if (_equipmentSlotPrefab != null)
                {
                    GameObject newSlotUI = Instantiate(_equipmentSlotPrefab, _equipmentPanel.transform);
                    _equipmentSlots[slot] = newSlotUI;

                    if (item != null)
                    {
                        newSlotUI.GetComponentInChildren<Image>().sprite = item.iconSprite;
                    }
                }
                else
                {
                    Debug.Log("EquipmentSlotPrefab = null 인 상태");
                }
            }
        }
    }
}