using UnityEngine;

namespace UntilTheEnd
{
    public class ItemObject : MonoBehaviour
    {
        public Item item;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(StringValues.Tag.player))
            {
                Debug.Log($"'{StringValues.Tag.player}'가 아이템에 닿았습니다.");
                EquipmentManager.instance.EquipItem("MyTest1", item);
                Debug.Log("동작완료");

                Destroy(gameObject);
            }
        }
    }
}