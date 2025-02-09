using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    protected Stack<T> activePool = new Stack<T>();   // 활성화된 오브젝트 스택
    protected Stack<T> deActivePool = new Stack<T>(); // 비활성화된 오브젝트 스택

    private T _prefab;

    public ObjectPool(T prefab, int initialCount)
    {
        _prefab = prefab;
        _PreloadObjects(initialCount);
    }

    protected virtual void _PreloadObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(_prefab);
            obj.gameObject.SetActive(false);
            deActivePool.Push(obj);
        }
    }

    public virtual T GetObject(Vector3 position)
    {
        if (deActivePool.Count > 0)
        {
            T obj = deActivePool.Pop();
            obj.transform.position = position;
            obj.gameObject.SetActive(true);
            activePool.Push(obj);
            return obj;
        }
        else
        {
            Debug.LogWarning($"풀에 남은 객체가 없어서 생성할 수 없습니다: {_prefab.name}");
            return null;
        }
    }

    public virtual void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        if (activePool.Contains(obj))
        {
            activePool.Pop();
            deActivePool.Push(obj);
        }
    }
}
