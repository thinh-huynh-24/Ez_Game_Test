using UnityEngine;

public class Setting : MonoBehaviour
{
    public static Setting Instance;

    public int mode;

    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Setting created");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Duplicate Setting destroyed");
            Destroy(gameObject);
        }
    }

    public void SetMode(int mode)
    {
        this.mode = mode;
    }

    public int GetMode()
    {
        return mode;
    }
}

