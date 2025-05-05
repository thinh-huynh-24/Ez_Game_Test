using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }


    [Header("Prefabs")]
    public GameObject Ally;
    public GameObject Enemy;
    public ParticleSystem Blood;
    public ParticleSystem Die;
    public ParticleSystem Block;

    [Header("Pool Sizes")]
    public int allyPoolSize = 10;
    public int enemyPoolSize = 10;
    public int effectPoolSize = 5;

    public Queue<GameObject> AllyPool = new Queue<GameObject>();
    public Queue<GameObject> EnemyPool = new Queue<GameObject>();
    public Queue<ParticleSystem> BloodPool = new Queue<ParticleSystem>();
    public Queue<ParticleSystem> DiePool = new Queue<ParticleSystem>();
    public Queue<ParticleSystem> BlockPool = new Queue<ParticleSystem>();

    void Awake()
    {

        Instance = this;
        for (int i = 0; i < allyPoolSize; i++)
        {
            GameObject obj = Instantiate(Ally);
            obj.SetActive(false);
            AllyPool.Enqueue(obj);
        }

        for (int i = 0; i < enemyPoolSize; i++)
        {
            GameObject obj = Instantiate(Enemy);
            obj.SetActive(false);
            EnemyPool.Enqueue(obj);
        }

        for (int i = 0; i < effectPoolSize; i++)
        {
            BloodPool.Enqueue(InstantiateEffect(Blood));
            DiePool.Enqueue(InstantiateEffect(Die));
            BlockPool.Enqueue(InstantiateEffect(Block));
        }
    }

    private ParticleSystem InstantiateEffect(ParticleSystem prefab)
    {
        ParticleSystem ps = Instantiate(prefab);
        ps.gameObject.SetActive(false);
        return ps;
    }

    // ---------- Get Methods ----------
    public GameObject GetAlly()
    {
        return GetFromPool(AllyPool, Ally);
    }

    public GameObject GetEnemy()
    {
        return GetFromPool(EnemyPool, Enemy);
    }

    public ParticleSystem GetBloodEffect()
    {
        return GetFromPool(BloodPool, Blood);
    }

    public ParticleSystem GetDieEffect()
    {
        return GetFromPool(DiePool, Die);
    }

    public ParticleSystem GetBlockEffect()
    {
        return GetFromPool(BlockPool, Block);
    }

    // ---------- Return Methods ----------
    public void ReturnAlly(GameObject obj)
    {
        ReturnToPool(AllyPool, obj);
    }

    public void ReturnEnemy(GameObject obj)
    {
        ReturnToPool(EnemyPool, obj);
    }

    public void ReturnEffect(Queue<ParticleSystem> pool, ParticleSystem effect)
    {
        effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        effect.gameObject.SetActive(false);
        pool.Enqueue(effect);
    }

    // ---------- Generic Methods ----------
    public GameObject GetFromPool(Queue<GameObject> pool, GameObject prefab)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
        obj.SetActive(true);
        return obj;
    }

    public ParticleSystem GetFromPool(Queue<ParticleSystem> pool, ParticleSystem prefab)
    {
        ParticleSystem ps = pool.Count > 0 ? pool.Dequeue() : InstantiateEffect(prefab);
        ps.gameObject.SetActive(true);
        ps.Play();
        return ps;
    }

    public void ReturnToPool(Queue<GameObject> pool, GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
