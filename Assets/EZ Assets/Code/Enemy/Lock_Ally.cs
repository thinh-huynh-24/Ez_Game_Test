using UnityEngine;

public class Lock_Ally : MonoBehaviour
{
    [SerializeField] private GameObject nearestAlly;
    [SerializeField] private GameObject targetAlly;
    [SerializeField] private GameObject newAlly;
    [SerializeField] private float lockRange = 50f;
    private float nextFindTime = 0f;
    [SerializeField] private float delay = 0.5f;


    void LateUpdate()
    {
        CallFinestAlly();
    }

    public GameObject GenewAlly()
    {
        return newAlly;
    }
    public GameObject GetTargetAlly()
    {
        return targetAlly;
    }
    public void CallFinestAlly()
    {
        if (Time.time >= nextFindTime)
        {
            newAlly = FindNearestAlly();

            if (newAlly != null && newAlly != targetAlly)
            {

                targetAlly = newAlly;
                // Debug.Log(targetAlly.name);
            }
            nextFindTime = Time.time + delay;
        }
    }

    public GameObject FindNearestAlly()
    {
        GameObject[] enemies = new GameObject[0];
        if (gameObject.CompareTag("Enemy"))
        {
            enemies = GameObject.FindGameObjectsWithTag("Ally");
        }
        else if (gameObject.CompareTag("Ally"))
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }


        float minDistance = Mathf.Infinity;
        GameObject closest = null;

        if (gameObject.CompareTag("Enemy"))
        {
            foreach (GameObject Ally in enemies)
            {
                float distance = Vector3.Distance(transform.position, Ally.transform.position);
                // Debug.Log(distance);
                if (distance < minDistance && distance <= lockRange)
                {
                    minDistance = distance;
                    closest = Ally;
                }
            }
        }
        else if (gameObject.CompareTag("Ally"))
        {
            foreach (GameObject Enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, Enemy.transform.position);
                // Debug.Log(distance);
                if (distance < minDistance && distance <= lockRange)
                {
                    minDistance = distance;
                    closest = Enemy;
                }
            }
        }



        nearestAlly = closest;

        if (nearestAlly != null)
        {
            return nearestAlly;
            // Có thể quay mặt về phía Ally chẳng hạn:
            // transform.LookAt(nearestAlly.transform);
        }
        return null; // Không tìm thấy Ally trong phạm vi
    }

    public void LookAtAlly(GameObject taget)
    {
        if (taget == null) return; // Nếu không có Ally thì không làm gì cả    
        Vector3 direction = (taget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}
