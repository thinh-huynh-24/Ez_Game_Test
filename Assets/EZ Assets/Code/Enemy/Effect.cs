using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private ObjectPool pool;
    void Start()
    {
        pool = ObjectPool.Instance;
    }
    public void BloodEffect(Vector3 position)
    {
        ParticleSystem fx = pool.GetBloodEffect();
        fx.transform.position = position;
        fx.Play();
        StartCoroutine(ReturnAfterSeconds(fx, 0.3f, pool.BloodPool));
    }

    public void DieEffect(Vector3 position)
    {
        ParticleSystem fx = pool.GetDieEffect();
        fx.transform.position = position;
        fx.Play();
        StartCoroutine(ReturnAfterSeconds(fx, 5f, pool.DiePool));
    }

    public void BlockEffect(Vector3 position)
    {
        ParticleSystem fx = pool.GetBlockEffect();
        fx.transform.position = position;
        fx.Play();
        StartCoroutine(ReturnAfterSeconds(fx, 1f, pool.BlockPool));
    }

    private IEnumerator ReturnAfterSeconds(ParticleSystem fx, float delay, Queue<ParticleSystem> returnPool)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnEffect(returnPool, fx);
    }

}
