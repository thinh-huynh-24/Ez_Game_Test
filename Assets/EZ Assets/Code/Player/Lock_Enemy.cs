using UnityEngine;

public class Lock_Enemy : MonoBehaviour
{
    [SerializeField] private GameObject nearestEnemy;
    [SerializeField] private GameObject targetEnemy;
    [SerializeField] private float lockRange = 1f;
    private float nextFindTime = 0f;
    [SerializeField] private float delay = 2f;


    void Update()
    {
        CallFinestEnemy();
    }

    public GameObject GetNearestEnemy()
    {
        return nearestEnemy;
    }
    public GameObject GetTargetEnemy()
    {
        return targetEnemy;
    }
    public void CallFinestEnemy()
    {
        if (Time.time >= nextFindTime)
        {
            GameObject newEnemy = FindNearestEnemy();

            if (newEnemy != targetEnemy)
            {
                targetEnemy = newEnemy;
            }
            nextFindTime = Time.time + delay;
        }
    }

    public GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float minDistance = Mathf.Infinity;
        GameObject closest = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance && distance <= lockRange)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        nearestEnemy = closest;

        if (nearestEnemy != null)
        {
            return nearestEnemy;
            // Có thể quay mặt về phía enemy chẳng hạn:
            // transform.LookAt(nearestEnemy.transform);
        }
        return null; // Không tìm thấy enemy trong phạm vi
    }

    public void LookAtEnemy(GameObject taget)
    {
        if (taget == null) return; // Nếu không có enemy thì không làm gì cả    
        Vector3 direction = (taget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

    }

}