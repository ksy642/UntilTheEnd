using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> _pool = new Queue<T>();
    private T _prefab;
    private Transform _parent;

    public ObjectPool(T prefab, int initialCount, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
        PreloadObjects(initialCount);
    }

    private void PreloadObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public T GetObject(Vector3 position)
    {
        if (_pool.Count > 0)
        {
            T obj = _pool.Dequeue();
            obj.transform.position = position;
            obj.gameObject.SetActive(true);
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
        _pool.Enqueue(obj);
    }
}
