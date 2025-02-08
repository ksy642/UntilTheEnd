using System.Collections.Generic;

using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    public List<Stack<T>> activePoolList = new List<Stack<T>>();   // 활성화된 오브젝트 스택 리스트
    public List<Stack<T>> deActivePoolList = new List<Stack<T>>(); // 비활성화된 오브젝트 스택 리스트

    private T _prefab;

    public ObjectPool(T prefab, int initialCount)
    {
        _prefab = prefab;
        _PreloadObjects(initialCount);
    }

    private void _PreloadObjects(int count)
    {
        // 리스트가 비어 있으면 새로운 Stack 추가
        if (deActivePoolList.Count == 0)
        {
            deActivePoolList.Add(new Stack<T>());
        }

        if (activePoolList.Count == 0)
        {
            activePoolList.Add(new Stack<T>());
        }




        // 첫 번째 스택을 사용
        Stack<T> deActiveStack = deActivePoolList[0];

        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(_prefab);//
            obj.gameObject.SetActive(false);

            //_pool.Enqueue(obj);
            deActiveStack.Push(obj);
        }


        PrintPoolContents();
    }















    private void PrintPoolContents()
    {
        Debug.Log($"▶ activePoolList 개수: {activePoolList.Count}");
        Debug.Log($"▶ deActivePoolList 개수: {deActivePoolList.Count}");

        // 활성화된 오브젝트 스택 내용 출력
        for (int i = 0; i < activePoolList.Count; i++)
        {
            Debug.Log($"🔹 activePoolList[{i}] - 스택 크기: {activePoolList[i].Count}");

            foreach (T obj in activePoolList[i])
            {
                Debug.Log($"     ↳ {obj.name} (활성화됨)");
            }
        }

        // 비활성화된 오브젝트 스택 내용 출력
        for (int i = 0; i < deActivePoolList.Count; i++)
        {
            Debug.Log($"🔸 deActivePoolList[{i}] - 스택 크기: {deActivePoolList[i].Count}");

            foreach (T obj in deActivePoolList[i])
            {
                Debug.Log($"     ↳ {obj.name} (비활성화됨)");
            }
        }
    }

























    public T GetObject(Vector3 position)
    {
        if (deActivePoolList.Count > 0)
        {
            Debug.LogWarning($"풀에 남은 객체가 있네?! {_prefab.name}");

            Stack<T> deActiveStack = deActivePoolList[0];
            Stack<T> activeStack = activePoolList[0];

            T obj = deActiveStack.Pop(); // 비활성화 스택에서 가져옴

            obj.transform.position = position;
            obj.gameObject.SetActive(true);

            activeStack.Push(obj);

            return obj;
        }
        else
        {
            Debug.LogWarning($"풀에 남은 객체가 없어서 멈춥니다! {_prefab.name}");
            //T newObj = GameObject.Instantiate(_prefab, position, Quaternion.identity, _parent);
            //return newObj;
            return null;
        }
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);

        Stack<T> activeStack = activePoolList[0];
        Stack<T> inactiveStack = deActivePoolList[0];

        if (activeStack.Contains(obj))
        {
            activeStack.Pop();        // 활성화 리스트에서 제거
            inactiveStack.Push(obj); // 다시 비활성화 리스트에 추가
        }
    }
}
