using System.Collections.Generic;
using System;
using UnityEngine;

namespace UntilTheEnd
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/InventoryData")]
    public class ItemData : ScriptableObject
    {
        public List<Item> itemList = new List<Item>();

        // 아이템 수집
        public void ColletItem(int index)
        {
            itemList[index].isCollect = true;
        }
    }

    [Serializable]
    public class Item
    {
        public SphereCollider sphereCollider;

        public bool isCollect = false; // 수집여부
        public bool isEquipable = false;   // 장착 가능 여부
        public bool isConsumable = false; // 소모품 여부

        public Sprite iconSprite;
        public string name = "아이템";
        public string description = "간략한 설명";

        
    }
}