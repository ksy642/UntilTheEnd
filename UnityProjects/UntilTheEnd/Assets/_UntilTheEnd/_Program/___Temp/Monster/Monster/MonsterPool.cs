using UnityEngine;
using System.Collections.Generic;

public class MonsterPool : MonoBehaviour
{
    [System.Serializable]
    public class MonsterPrefab
    {
        public MonsterType type;
        public Monster monsterPrefab;
    }

    [SerializeField] private List<MonsterPrefab> monsterPrefabs = new List<MonsterPrefab>();
    private Dictionary<MonsterType, ObjectPool<Monster>> _monsterPools = new Dictionary<MonsterType, ObjectPool<Monster>>();

    private void Start()
    {
        foreach (var monsterData in monsterPrefabs)
        {
            if (!_monsterPools.ContainsKey(monsterData.type))
            {
                _monsterPools[monsterData.type] = new ObjectPool<Monster>(monsterData.monsterPrefab, 5, transform);
            }
        }
    }

    public Monster GetMonster(MonsterType type, Vector3 position)
    {
        if (_monsterPools.ContainsKey(type))
        {
            return _monsterPools[type].GetObject(position);
        }

        Debug.LogWarning($"[MonsterPool] {type} 몬스터 타입을 찾을 수 없습니다!");
        return null;
    }

    public void ReturnMonster(MonsterType type, Monster monster)
    {
        if (_monsterPools.ContainsKey(type))
        {
            _monsterPools[type].ReturnObject(monster);
        }
        else
        {
            Debug.LogError($"[MonsterPool] 풀에서 {type} 몬스터를 찾을 수 없습니다!");
        }
    }
}