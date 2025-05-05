using TMPro;
using UnityEngine;

public class Player_UI : MonoBehaviour
{
    public TextMeshProUGUI HealText;

    public void UpdateHeal(int heal)
    {
        if (heal < 0)
        {
            gameObject.SetActive(false);
            HealText.text = "0";
            return;
        }
        HealText.text = heal.ToString();
    }
}
