using UnityEngine;

public class BehaviourSingleton<T> : MonoBehaviour where T: Component
{
    [Header("Singleton settings")]
    [SerializeField] private SetInstanceMode setInstanceMode = SetInstanceMode.Usual;
    [SerializeField] private DublicateDeletingMode dublicateDeletingMode = DublicateDeletingMode.DestroyComponent;
    [SerializeField] private bool needLogOnSingletonLittleProblems = true;

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance != null)
                {
                    Debug.Log("Instance of " + typeof(T) + " has been called before its initialazing in Awake() and has been found with " +
                    "FindObjectOfType() method. It may cause performance drop. Try to change the script order to avoid it");
                }
                else
                {
                    Debug.LogError("Instance of " + typeof(T) + " Singleton hasn't been found. Make sure that there's one object of " +
                    typeof(T) + " on the scene before trying to access it");
                }
            }
            return _instance;
        }
    }

    public static bool IsInitiated
    {
        get
        {
            return _instance != null;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (setInstanceMode == SetInstanceMode.DontDestroyOnLoad)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            if (_instance != this)      
            {
                if (dublicateDeletingMode == DublicateDeletingMode.DestroyComponent)
                {
                    if (needLogOnSingletonLittleProblems) 
                        Debug.Log($"Destroying component *{typeof(T)}* on *{gameObject.name}* as second instance of *{typeof(T)}* Singleton!");
                    Destroy(this);
                }
                else if (dublicateDeletingMode == DublicateDeletingMode.DestroyGameObject)
                {
                    if (needLogOnSingletonLittleProblems) 
                        Debug.Log($"Destroying gameObject *{gameObject.name}* as second instance of *{typeof(T)}* Singleton!");
                    Destroy(gameObject);
                }
            }
        }
    }

    private enum SetInstanceMode
    {
        Usual,
        DontDestroyOnLoad
    }

    private enum DublicateDeletingMode
    {
        DestroyComponent,
        DestroyGameObject
    }
}