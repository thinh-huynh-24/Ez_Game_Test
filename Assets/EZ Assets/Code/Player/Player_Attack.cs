using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Combat_Action : MonoBehaviour
{
    [SerializeField] private float walkRL = 0.5f;
    [SerializeField] public float delay = 2f;

    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isBlocking = false;
    [SerializeField] private bool isBlock = false;
    [SerializeField] private bool isHit = false;
    [SerializeField] private bool invin = false;
    [SerializeField] private ParticleSystem block;
    [SerializeField] private ParticleSystem hit;
    private Animator animator;

    [SerializeField] private int hitDameage = 15;
    [SerializeField] private int heal = 100;
    [SerializeField] private int maxHeal = 100;
    [SerializeField] private Collider RightHandCollider;
    [SerializeField] private Collider LeftHandCollider;
    [SerializeField] private int numattack = 0;
    private float lastAttackTime = 0f;
    [SerializeField] private bool IsCombat = false;
    [SerializeField] private float push = 3f;
    [SerializeField] private Player_UI player_UI;
    [SerializeField] private Effect effect;



    void Start()
    {
        player_UI = GetComponent<Player_UI>();

        animator = GetComponent<Animator>();
        RightHandCollider.enabled = false;
        LeftHandCollider.enabled = false;
    }

    public bool GetBlocking()
    {
        return isBlocking;
    }
    public bool GetBlock()
    {
        return isBlock;
    }
    public bool GetHit()
    {
        return isHit;
    }
    private GameObject alreadyHit;
    private GameObject taget;
    void OnTriggerEnter(Collider other)
    {
        // taget = other.gameObject;
        // if (alreadyHit == taget) return;
        if (isAttacking && other.CompareTag("Enemy"))
        {
            alreadyHit = taget;
            HandleHit(other);
        }
    }
    private void HandleHit(Collider other)
    {
        RightHandCollider.enabled = false;
        LeftHandCollider.enabled = false;
        numattack = 0;
        isAttacking = false;

        other.GetComponent<Combat>()?.HitControl(numattack, hitDameage);

        other.GetComponent<Combat_Action>()?.HitControl(numattack, hitDameage);
        if (other.GetComponent<Combat>().GetHeal() <= 0)

        {
            heal += (int)(heal * 0.1f);
            player_UI.UpdateHeal(heal);

        }


    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void SetCombat(bool combat)
    {
        animator.SetBool("IsCombat", combat);
        IsCombat = combat;
    }
    public bool GetCombat()
    {
        return IsCombat;
    }
    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            numattack = Random.Range(1, 5);
            animator.SetInteger("AttackNum", numattack);

            switch (numattack)
            {
                case 1:
                    StartCoroutine(EnableHitboxForDuration(LeftHandCollider, 0.2f, 0.28f));
                    break;

                case 2:
                    StartCoroutine(EnableHitboxForDuration(RightHandCollider, 0.3f, 0.3f));
                    break;

                case 3:
                    StartCoroutine(EnableHitboxForDuration(RightHandCollider, 0.2f, 0.4f));
                    StartCoroutine(EnableHitboxForDuration(LeftHandCollider, 0.2f, 0.25f));
                    break;

                default:
                    StartCoroutine(EnableHitboxForDuration(LeftHandCollider, 0.2f, 0.46f));
                    break;
            }

            StartCoroutine(ResetAttackAnimation());
            StartCoroutine(ResetAttack1());
        }
    }

    private IEnumerator EnableHitboxForDuration(Collider collider, float duration, float started)
    {
        yield return new WaitForSeconds(started);
        alreadyHit = null;
        collider.enabled = true;
        yield return new WaitForSeconds(duration);
        collider.enabled = false;
    }

    private System.Collections.IEnumerator ResetAttack1()
    {
        yield return new WaitForSeconds(delay);

        numattack = 0;
        isAttacking = false;
        SetInvin(false);
    }
    private System.Collections.IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        RightHandCollider.enabled = false;
        LeftHandCollider.enabled = false;
        animator.SetInteger("AttackNum", 0);

    }

    public void QuickRAttck()
    {
        RightHandCollider.enabled = false;
        LeftHandCollider.enabled = false;
        numattack = 0;
        isAttacking = false;

    }
    public void SetInvin(bool inv)
    {
        invin = inv;

    }


    public void Blocking(bool blocking)
    {
        if (blocking)
        {
            animator.SetBool("IsBlocking", true);
            isBlocking = true;
            return;
        }
        else if (!blocking && isBlocking)
        {
            StartCoroutine(ResetBlocking());
        }
    }
    private System.Collections.IEnumerator ResetBlocking()
    {
        yield return new WaitForSeconds(0.1f);
        isBlocking = false;
        animator.SetBool("IsBlocking", false);
    }

    public void Block()
    {
        if (!isBlock)
        {
            isBlock = true;
            animator.SetBool("IsBlock", true);
            effect.BlockEffect(transform.position + Vector3.up / 2);
            isBlock = true;
            StartCoroutine(ResetBlock());
        }
    }
    private System.Collections.IEnumerator ResetBlock()
    {
        yield return new WaitForSeconds(0.2f);
        isBlock = false;
        animator.SetBool("IsBlock", false);
    }

    public void GotHit(int hitnum, int healnum)
    {
        if (!isHit)
        {
            effect.BloodEffect(transform.position + Vector3.up);
            if (healnum > heal)
            {
                heal = healnum;
            }
            switch (hitnum)
            {
                case 1:
                    animator.SetInteger("HitNum", 3);
                    heal -= healnum;
                    isHit = true;
                    StartCoroutine(ResetHitNum());
                    StartCoroutine(ResetInvin());
                    break;

                case 2:
                    animator.SetInteger("HitNum", 3);
                    heal -= healnum;
                    isHit = true;
                    StartCoroutine(ResetHitNum());
                    StartCoroutine(ResetInvin());
                    break;

                case 3:
                    animator.SetInteger("HitNum", 1);
                    heal -= healnum;
                    isHit = true;
                    StartCoroutine(ResetHitNum());
                    StartCoroutine(ResetInvin());
                    break;

                default:
                    animator.SetInteger("HitNum", 2);
                    heal -= healnum;
                    isHit = true;
                    StartCoroutine(ResetHitNum());
                    StartCoroutine(ResetInvin());
                    break;
            }
            player_UI.UpdateHeal(heal);
        }

    }


    private System.Collections.IEnumerator ResetHitNum()
    {
        yield return new WaitForSeconds(1f);
        isHit = false;
        animator.SetInteger("HitNum", 0);
    }

    private System.Collections.IEnumerator ResetInvin()
    {
        yield return new WaitForSeconds(0.5f);
        invin = false;

    }

    public void HitControl(int hitnum, int healnum)
    {
        if (invin)
        {
            return;
        }

        if (isBlocking)
        {
            Block();
        }

        else
        {
            if (isAttacking)
            {
                QuickRAttck();
            }
            GotHit(hitnum, healnum);
        }
    }

    public void Die()
    {

        invin = true;
        animator.SetBool("IsLose", true);
        // Debug.Log("Die!");
    }
    public int GetHeal()
    {
        return heal;
    }


}
