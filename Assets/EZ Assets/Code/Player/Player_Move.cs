using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class Player_Move : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float Rdelay = 0.3f;
    [SerializeField] private bool IsWalk_Run = false;
    [SerializeField] private float combatmovedri = 0.5f;
    [SerializeField] private float speed = 0f;
    [SerializeField] private float combatspeed = 0.1f;
    [SerializeField] private bool IsMoving = false;
    [SerializeField] private Vector2 MoveInput;

    Vector3 move;
    bool reset = false;
    bool isCombat = false;
    bool IsMoveOrCombat = false;
    private float Walk_RunStartTime;
    [SerializeField] private Animator animator;
    private bool isActice = false;


    private IEnumerator SetActive()
    {
        yield return new WaitForSeconds(3.5f);
        isActice = true;

    }



    void OnEnable()
    {
        animator = GetComponent<Animator>();
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();


        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(SetActive());

    }

    void Update()
    {
        if (!isActice) return;
        if (!IsMoving && speed > 0f)
        {
            MovePlayer();
        }
        if (combatspeed > 0f && !IsMoving)
        {
            isCombat = false;
            CombatMove();
        }

    }

    public void SetSpawm()
    {
        animator.SetBool("IsSwaping", true);
    }


    public void SetMoving(bool isMoving)
    {
        IsMoving = isMoving;
    }
    public bool getMoving()
    {
        return IsMoving;
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
        if (MoveInput != Vector2.zero)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    public void MovePlayer()
    {
        if (!IsMoving)
        {
            speed -= 20f * Time.deltaTime;
            if (speed < 0f)
            {
                speed = 0f;
            }
        }
        else
        {
            speed += 3f * Time.deltaTime;
            if (speed > 3f)
            {
                speed = 3f;
            }
            Vector3 move = new Vector3(MoveInput.x, 0f, MoveInput.y);
            agent.Move(move * speed * Time.deltaTime);

            if (move != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
            }
        }
        animator.SetFloat("IsWalk_Run", speed);
    }

    public void CombatMove()
    {
        move = new Vector3(MoveInput.x, 0f, MoveInput.y);
        Vector3 localMove = transform.InverseTransformDirection(move);

        if (move == Vector3.zero)
        {
            combatspeed -= 20f * Time.deltaTime;
            combatmovedri = Mathf.Lerp(combatmovedri, 0f, Time.deltaTime * 20f);
        }
        else
        {
            combatspeed += 1f * Time.deltaTime;


            combatmovedri = Mathf.Lerp(combatmovedri, localMove.x, Time.deltaTime * 20f);
            combatmovedri = Mathf.Clamp(combatmovedri, -1f, 1f);
        }

        combatspeed = Mathf.Clamp(combatspeed, 0f, 0.5f);
        agent.Move(move * combatspeed * Time.deltaTime);
        isCombat = true;
        animator.SetFloat("IsCombatLR", combatmovedri);
    }

    public void SpawmMove()
    {
        animator.SetBool("IsSwaping", true);
    }


}
