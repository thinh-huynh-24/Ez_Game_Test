using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private float walkRL = 0.5f;
    [SerializeField] public float delay = 3f;
    [SerializeField] private ParticleSystem block;
    [SerializeField] private ParticleSystem hit;

    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isBlocking = false;
    [SerializeField] private bool isBlock = false;
    [SerializeField] private bool isHit = false;
    [SerializeField] private bool invin = false;
    private Animator animator;
    [SerializeField] private bool canAttack = false;



    [SerializeField] private int hitDameage;
    [SerializeField] private int heal = 100;
    [SerializeField] private int maxHeal = 100;
    [SerializeField] private Collider RightHandCollider;
    [SerializeField] private Collider LeftHandCollider;
    [SerializeField] private int numattack = 0;
    private float lastAttackTime = 0f;
    [SerializeField] private bool IsCombat = false;
    [SerializeField] private float push = 1f;
    [SerializeField] private bool setblock = false;
    [SerializeField] private float tile;
    [SerializeField] private float scalenum;

    [SerializeField] private Effect effect;


    public void SetScale(float newscalenum)
    {
        scalenum = newscalenum;

    }

    public void GethitDameage(bool strong)
    {
        if (strong)
        {
            hitDameage = (int)(20f * scalenum);
        }
        else
        {
            hitDameage = (int)(10f * scalenum);

        }

    }


    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(SetValueRandomDuration());
        StartCoroutine(SetBlockRandomDuration());

        RightHandCollider.enabled = false;
        LeftHandCollider.enabled = false;
    }
    public void SetTiLe(float value)
    {
        tile = value;

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

        if (isAttacking && other.CompareTag("Ally") && gameObject.CompareTag("Enemy"))
        {
            // alreadyHit = taget;
            HandleHit(other);
        }
        else if (isAttacking && other.CompareTag("Enemy") && gameObject.CompareTag("Ally"))
        {
            // alreadyHit = taget;
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
    }

    void ResetAttack()
    {
        alreadyHit = null;
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
    [SerializeField] private float startedtime = 0;
    [SerializeField] private float duration = 0;
    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            numattack = Random.Range(1, 5);
            animator.SetInteger("AttackNum", numattack);
            canAttack = false;

            switch (numattack)
            {
                case 1:
                    GethitDameage(true);
                    StartCoroutine(EnableHitboxForDuration(LeftHandCollider, 0.2f, 0.28f));
                    break;

                case 2:
                    GethitDameage(true);
                    StartCoroutine(EnableHitboxForDuration(RightHandCollider, 0.3f, 0.3f));
                    break;

                case 3:
                    GethitDameage(false);
                    StartCoroutine(EnableHitboxForDuration(RightHandCollider, 0.2f, 0.4f));
                    StartCoroutine(EnableHitboxForDuration(LeftHandCollider, 0.2f, 0.25f));
                    break;

                default:
                    GethitDameage(false);
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

    public bool GetInvin()
    {
        return invin;

    }

    public void Blocking()
    {
        if (setblock)
        {
            animator.SetBool("IsBlocking", true);
            isBlocking = true;
            invin = true;
            return;
        }
        else
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
        animator.SetBool("IsBlock", false);
        yield return new WaitForSeconds(0.5f);
        isBlock = false;
    }

    public void GotHit(int hitnum, int healnum)
    {
        if (isAttacking)
        {
            QuickRAttck();
        }
        if (!isHit)
        {
            effect.BloodEffect(transform.position + Vector3.up);
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
        }

    }
    private System.Collections.IEnumerator ResetHitNum()
    {
        yield return new WaitForSeconds(1f);
        isHit = false;
        invin = false;
        animator.SetInteger("HitNum", 0);
    }

    private System.Collections.IEnumerator ResetInvin()
    {
        yield return new WaitForSeconds(0.2f);


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

    public bool GetAttack()
    {
        return canAttack;
    }

    private IEnumerator SetValueRandomDuration()
    {
        while (true)
        {
            float waitBefore = Random.Range(1f, 9f) / scalenum;
            yield return new WaitForSeconds(waitBefore);

            canAttack = true;
        }
    }

    private IEnumerator SetBlockRandomDuration()
    {
        while (true)
        {
            float waitBefore = Random.Range(1f, 7f) / scalenum;
            yield return new WaitForSeconds(waitBefore);

            setblock = true;

            float activeDuration = Random.Range(2f, 4f) / scalenum;
            yield return new WaitForSeconds(activeDuration);

            setblock = false;
        }
    }

    public bool GetSetBlock()
    {
        return setblock;

    }



}
