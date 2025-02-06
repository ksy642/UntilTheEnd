using UnityEngine;

public class Orc : Monster // 테스트용 빨간색
{
    public float health = 100f;

    private void Awake()
    {
        SetMonsterType(MonsterType.Orc);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Orc 맞음! 남은 체력: {health}");

        if (health <= 0)
        {
            Die();
        }
    }



    public override void Attack()
    {
        Debug.Log("오크가 공격했다!");
    }

    // Orc만의 특별한 죽는 연출이 필요하면 오버라이드 가능
    public override void Die()
    {
        base.Die();  // 부모의 Die() 로직 실행
        Debug.LogWarning("오크가 크게 포효하며 쓰러졌다!");
        this.gameObject.SetActive(false);
    }
}