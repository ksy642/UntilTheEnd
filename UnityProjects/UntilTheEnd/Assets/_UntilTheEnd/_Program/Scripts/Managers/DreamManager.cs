using UnityEngine;

public class DreamManager : DontDestroySingleton<DreamManager>
{
    void Start() // 이건 돈디스트로이로 소환돼서 스타트함수가 동작안하는거 같음
    {
        Debug.Log("테스트2");
    }

    void Update()
    {
        
    }

    public void DreamTest1()
    {
        Debug.Log("테스트1");
    }
}
