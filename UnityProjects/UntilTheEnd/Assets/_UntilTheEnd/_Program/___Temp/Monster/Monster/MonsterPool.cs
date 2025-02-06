using System.Collections.Generic;
using UnityEngine;

//  몬스터 풀 관리
public class MonsterPool : MonoBehaviour
{
    public GameObject orcPrefab;
    public GameObject goblinPrefab;
    private Dictionary<string, Queue<Monster>> pool = new Dictionary<string, Queue<Monster>>();

    private void Start()
    {
        pool["Orc"] = new Queue<Monster>();
        pool["Goblin"] = new Queue<Monster>();

        _PreloadMonsters("Orc", orcPrefab, 5);
        _PreloadMonsters("Goblin", goblinPrefab, 5);
    }

    private void _PreloadMonsters(string type, GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool[type].Enqueue(obj.GetComponent<Monster>());
        }
    }

    // Pool 에 부족하면 이미 비활성화 된 객체를 다시 재생성 시키는 방향으로 해야함.
    // 재생성은 당연히 위치도 다시 랜덤하게 소환되는 로직을 따라야함
    public Monster GetMonster(string type, Vector3 position)
    {
        if (pool[type].Count > 0)
        {
            Monster monster = pool[type].Dequeue();
            monster.Spawn(position);
            return monster;
        }
        else
        {
            //Debug.LogWarning($"{type} 풀에 몬스터가 부족해서 새로 생성합니다!");
            GameObject newMonster = Instantiate(type == "Orc" ? orcPrefab : goblinPrefab);
            return newMonster.GetComponent<Monster>();
        }
    }

    public void ReturnMonster(string type, Monster monster)
    {
        if (pool.ContainsKey(type))
        {
            monster.gameObject.SetActive(false);
            pool[type].Enqueue(monster);
        }
        else
        {
            Debug.LogError($"풀에서 {type} 몬스터를 찾을 수 없습니다!");
        }
    }
}
