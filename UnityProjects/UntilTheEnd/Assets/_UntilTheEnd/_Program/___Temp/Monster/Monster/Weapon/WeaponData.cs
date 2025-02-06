using UnityEngine;

namespace UntilTheEnd
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;
        public float damage;
        public float fireRate;
        public float range;
        public GameObject[] weaponPrefabs;
    }
}