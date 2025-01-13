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
                        // ���ҽ��������� ������ ��ȯ�� �� ������
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
        // �ߺ� �ν��Ͻ� ����
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