using UnityEngine;

public class DreamManager : DontDestroySingleton<DreamManager>
{
    void Start() // �̰� ����Ʈ���̷� ��ȯ�ż� ��ŸƮ�Լ��� ���۾��ϴ°� ����
    {
        Debug.Log("�׽�Ʈ2");
    }

    void Update()
    {
        
    }

    public void DreamTest1()
    {
        Debug.Log("�׽�Ʈ1");
    }
}
