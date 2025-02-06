using UnityEngine;

namespace UntilTheEnd
{
    public class Weapon : MonoBehaviour
    {
        public WeaponData weaponData; // 무기 데이터 저장

        private void Start()
        {
            if (weaponData != null)
            {
                Debug.Log($" 장착된 무기: {weaponData.weaponName}, 데미지: {weaponData.damage}");
            }
        }

        public virtual void WeaponChange()
        {
            Debug.Log($"{weaponData.weaponName} 변경!");
        }

        public virtual void Attack()
        {
            Debug.Log($" {weaponData.weaponName} 공격!");
        }
    }
}
