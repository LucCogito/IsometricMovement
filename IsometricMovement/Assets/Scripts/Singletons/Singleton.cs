using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindOrCreateInstance();
            }
            return _instance;
        }
    }

    private static T _instance;

    private static T FindOrCreateInstance() => FindObjectOfType<T>() ?? new GameObject(typeof(T).Name + " Singleton").AddComponent<T>();

    private void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }
}
