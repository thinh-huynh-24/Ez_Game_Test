using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controler : MonoBehaviour
{
    [SerializeField] private Enemy_Move enemy_Move;
    [SerializeField] private Lock_Ally ally;
    [SerializeField] private Combat enemy_Combat;
    [SerializeField] private GameObject target = null;
    [SerializeField] private GameObject upptarget = null;
    [SerializeField] private GameManager gameManager;
    [SerializeField] ParticleSystem die;
    [SerializeField] private bool IsDie = false;



    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isLose = false;
    [SerializeField] private bool isWin = false;
    [SerializeField] private bool isSpawm = false;
    [SerializeField] private float scale;
    [SerializeField] private float size;
    [SerializeField] private Effect effect;




    private Vector3 lastTargetPosition = Vector3.zero;

    void Awake()
    {
        effect = GetComponent<Effect>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        enemy_Move = GetComponent<Enemy_Move>();
        ally = GetComponent<Lock_Ally>();
        enemy_Combat = GetComponent<Combat>();

    }
    void OnEnable()
    {
        if (gameManager.GetMode() == 0)
        {
            scale = Random.Range(1, gameManager.GetRound() / 10 + 1);
        }
        else
        {
            scale = Random.Range(1, gameManager.GetRound() / 20 + 1);
        }

        enemy_Combat.SetScale(scale);
        gameObject.GetComponent<Transform>().localScale = new Vector3(scale, scale, scale);
        enemy_Move.SetScale(scale);


        StartCoroutine(SetSpawm());
    }

    void LateUpdate()
    {
        if (!isSpawm) return;
        else
        {
            if (enemy_Combat.GetHeal() <= 0)
            {
                if (!isLose)
                {
                    enemy_Combat.SetCombat(false);
                    gameObject.tag = "Untagged";
                    enemy_Combat.Die();
                    StartCoroutine(DieWait());
                    return;
                }
                return;
            }
            else
            {
                upptarget = ally.GenewAlly();
                target = ally.GetTargetAlly();


                if (target != null && upptarget != null)
                {

                    Vector3 targetPosition = target.transform.position;
                    float distance = Vector3.Distance(transform.position, targetPosition);

                    // Debug.Log("distance: " + distance);

                    if (lastTargetPosition != targetPosition)
                    {
                        enemy_Move.SetMoveInput(targetPosition);
                        enemy_Move.Move();
                        lastTargetPosition = targetPosition;
                    }
                    ally.LookAtAlly(target);


                    if (distance > 0.7f && distance <= 0.8f && !enemy_Combat.IsAttacking())
                    {
                        enemy_Combat.Attack();
                        target = ally.GetTargetAlly();
                        return;
                    }

                    if (distance > 2f)
                    {
                        enemy_Move.Move();
                    }

                    if (distance <= 1.5f)
                    {
                        if (!enemy_Combat.GetCombat())
                        {
                            enemy_Move.UnMove();
                            enemy_Combat.SetCombat(true);
                            return;
                        }

                        enemy_Move.CombatMove();
                        return;
                    }




                }
                else
                {
                    enemy_Combat.SetCombat(false);
                    enemy_Move.RandomMove();
                    return;
                }
            }
        }

    }

    private IEnumerator DieWait()
    {
        yield return new WaitForSeconds(5f);
        if (!IsDie)
        {
            IsDie = true;
            effect.DieEffect(transform.position);
        }
        yield return new WaitForSeconds(0.3f);
        gameManager.DownNpc(this.gameObject);
    }

    private IEnumerator SetSpawm()
    {
        enemy_Move.SetSpawm();
        yield return new WaitForSeconds(3.5f);
        isSpawm = true;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }
}
