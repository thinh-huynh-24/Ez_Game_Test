using System.Collections;
using UnityEngine;

public class DisableIn2S : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float time;

    void Start()
    {
        StartCoroutine(Gone());
    }
    private IEnumerator Gone()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
