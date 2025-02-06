using UnityEngine;
using UntilTheEnd;

public class WeaponSpawner : MonoBehaviour
{
    public WeaponData weaponData; // 🔥 소환할 무기 데이터 (Inspector에서 연결)
    public Transform spawnPoint;  // 🔥 무기 소환 위치 (빈 오브젝트의 Transform 사용)

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // 🔥 P 키 누르면 무기 생성
        {
            SpawnWeapon();
        }
    }

    private void SpawnWeapon()
    {
        if (weaponData == null || weaponData.weaponPrefabs.Length == 0)
        {
            Debug.LogWarning("WeaponData가 없거나 프리팹이 설정되지 않았습니다!");
            return;
        }

              //  첫 번째 프리팹을 기본으로 소환
        GameObject weaponPrefab = weaponData.weaponPrefabs[0];

        //  빈 오브젝트 위치에서 무기 생성
        GameObject spawnedWeapon = Instantiate(weaponPrefab, spawnPoint.position, spawnPoint.rotation);

        Weapon weapon = spawnedWeapon.GetComponent<Weapon>();


        if (weapon != null)
        {
            weapon.weaponData = weaponData; // ✅ weaponData를 Weapon에 할당!
        }

        Debug.Log($"{weaponData.weaponName} 소환 완료!");
    }
}
