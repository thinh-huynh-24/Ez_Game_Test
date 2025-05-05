using UnityEngine;

public class SetMode : MonoBehaviour
{

    public void SetModeTo(int num)
    {
        Setting.Instance.SetMode(num);
    }


}
