using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Move : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float Rdelay = 0.3f;
    [SerializeField] private bool IsWalk_Run = false;
    [SerializeField] private float combatmovedri = 0.5f;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float combatspeed = 0.1f;
    [SerializeField] private bool IsMoving = false;
    [SerializeField] private Vector3 MoveInput;
    [SerializeField] private float delay = 4f;

    public float maxSpeed = 3f;
    public float minSpeed = 0f;
    public float slowDownRadius = 0.1f;
    [SerializeField] private float strafeChangeInterval = 2f;
    [SerializeField] private float strafeSmoothSpeed = 1.1f;
    [SerializeField] private float strafeAmount = 1f;
    [SerializeField] private float forwardAmount = 0.01f;

    private float currentSpeed = 0.1f;
    private float speedVelocity = 0f;

    private float strafeDirection = 0f;
    private float targetStrafeDirection = 0f;
    private float strafeTimer = 0f;
    Vector3 move;
    bool reset = false;
    bool isCombat = false;
    bool IsMoveOrCombat = false;
    private float Walk_RunStartTime;
    [SerializeField] private Animator animator;
    float distance;
    Vector3 toTarget;
    private bool attack;





    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();



    }

    void LateUpdate()
    {
        if (IsMoving)
        {
            // currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, Time.deltaTime * (maxSpeed / agent.acceleration));
            // agent.speed = currentSpeed;
            agent.speed += 0.2f * Time.deltaTime;
            animator.SetFloat("IsWalk_Run", agent.speed);
        }
        else if (!IsMoving && agent.speed > 0)
        {
            agent.speed += 0.2f * Time.deltaTime;
            animator.SetFloat("IsWalk_Run", agent.speed);
        }
        if (isCombat)
        {
            CombatMove();

        }
    }


    public void SetMoving(bool isMoving)
    {
        IsMoving = isMoving;
    }
    public bool getMoving()
    {
        return IsMoving;
    }

    public void SetMoveInput(Vector3 moveInput)
    {
        MoveInput = moveInput;
    }

    public Vector3 GetMoveInput()
    {
        return MoveInput;

    }

    public void Move()
    {
        distance = Vector3.Distance(transform.position, MoveInput);

        if (distance <= agent.stoppingDistance)
        {
            currentSpeed = 0f;
            agent.speed = 0f;
            animator.SetFloat("IsWalk_Run", 0f);
            IsMoving = false;
            return;
        }

        IsMoving = true;
        agent.SetDestination(MoveInput);


    }



    public void UnMove()
    {
        agent.SetDestination(agent.transform.position);

    }


    public void CombatMove()
    {
        strafeTimer += Time.deltaTime;

        if (strafeTimer >= strafeChangeInterval)
        {
            targetStrafeDirection = Random.Range(-1f, 1f);
            Vector3 distan = MoveInput - transform.position;
            if (distan.magnitude < 0.7f)
            {
                forwardAmount = Random.Range(-1f, 0f);
            }
            else
            {
                forwardAmount = Random.Range(-1f, 1f);
            }

            strafeChangeInterval = Random.Range(2f, 4f);
            strafeTimer = 0f;

            isCombat = true;
        }

        strafeDirection = Mathf.Lerp(strafeDirection, targetStrafeDirection, Time.deltaTime * strafeSmoothSpeed);
        forwardAmount = Mathf.Lerp(forwardAmount, forwardAmount, Time.deltaTime * strafeSmoothSpeed);

        Vector3 forwardMove = transform.forward * forwardAmount * combatspeed * Time.deltaTime;
        Vector3 strafeMove = transform.right * strafeDirection * combatspeed * Time.deltaTime;

        Vector3 move = forwardMove + strafeMove;

        if (move.magnitude > (transform.position + new Vector3(5, 5, 5)).magnitude)
        {
            return;
        }

        agent.Move(move);

        animator.SetFloat("IsCombatLR", strafeDirection);
    }


    public System.Collections.IEnumerator ResetCombatMove()
    {
        yield return new WaitForSeconds(0.1f);
        IsMoving = true;

    }

    public void RandomMove()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 2f;
            randomDirection.y = 0f;

            Vector3 targetPosition = transform.position + randomDirection;

            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                MoveInput = hit.position;
                Move();
            }
        }
    }


    public void SetSpawm()
    {
        animator.SetBool("IsSwaping", true);
    }


    public void SetScale(float num)
    {
        combatspeed *= num;
    }

}
