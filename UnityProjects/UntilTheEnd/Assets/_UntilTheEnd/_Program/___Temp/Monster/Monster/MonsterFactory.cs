using UnityEngine;

// ✅ MonsterFactory: 몬스터를 Object Pool에서 가져옴
public class MonsterFactory
{
    private MonsterPool monsterPool;

    public MonsterFactory(MonsterPool pool)
    {
        this.monsterPool = pool;
    }

    public Monster CreateMonster(string type, Vector3 position)
    {
        return monsterPool.GetMonster(type, position);
    }
}
