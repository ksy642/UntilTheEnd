using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _instanceLock = new object();

    public static T instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindFirstObjectByType<T>();

                    if (_instance == null)
                    {
                        // 리소스폴더에서 프리팹 소환할 때 쓰려고
                        var resource = Resources.Load<GameObject>(typeof(T).Name);
                        _instance = resource?.GetComponent<T>();

                        if (_instance == null)
                        {
                            GameObject go = new GameObject(typeof(T).Name);
                            _instance = go.AddComponent<T>();
                        }
                    }
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        // 중복 인스턴스 방지
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
    }
}

public class DontDestroySingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}