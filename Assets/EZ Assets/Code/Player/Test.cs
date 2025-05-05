using UnityEngine;

public class Test : MonoBehaviour
{
    GameObject target;
    [SerializeField] private Transform pointA; // Điểm bắt đầu
    [SerializeField] private Transform pointB; // Điểm kết thúc
    [SerializeField] private float speed = 2f;

    private Vector3 target1;

    void Start()
    {
        target1 = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target1, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target1) < 0.1f)
        {
            target1 = target1 == pointA.position ? pointB.position : pointA.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == target)
        {
            return;
        }
        else if (other.CompareTag("Player"))
        {
            Combat_Action taget = other.GetComponent<Combat_Action>();
            int numattack;
            numattack = Random.Range(1, 4); // Số lượng tấn công

            taget.HitControl(numattack, 10); // Gọi hàm GotHit trên Player
        }
    }
}
