using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// 
/// https://answers.unity.com/questions/1408574/destroying-and-recreating-a-singleton.html
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static bool inactive;

    private static T m_Instance = null;
    public static T Instance
    {

        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<T>(); 
                // fallback, might not be necessary.
                if (m_Instance == null) {
                    Debug.LogFormat("{0}: Created new singleton instance", typeof(T).Name);
                    m_Instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }

                // This breaks scene reloading
                // DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }

    // NOTE: if you derive from Singleton and need to use Awake(), REMEMBER TO CALL THIS BASE METHOD TOO
    protected virtual void Awake()
    {
        if (m_Instance == null) {
            m_Instance = this as T;
            //DontDestroyOnLoad(gameObject);
        }
        else if(!ReferenceEquals(m_Instance.gameObject, gameObject)) {
            // Destroy self if there's already an instance in the scene
            Debug.LogWarningFormat("Singleton ({0}): instance ({1}) already exists in scene. Destroying self", name, m_Instance.name);
            Destroy(gameObject);
        }
    }
}