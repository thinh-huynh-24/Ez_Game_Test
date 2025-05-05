using UnityEngine;

public class CreatS : MonoBehaviour
{
    void Awake()
    {
        if (Setting.Instance == null)
        {
            GameObject settingGO = new GameObject("Setting");
            settingGO.AddComponent<Setting>();
        }
    }
}
