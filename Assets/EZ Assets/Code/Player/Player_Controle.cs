using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player_Controle : MonoBehaviour
{
    Combat_Action playerCombat;
    Player_Gesture playerGesture;
    Player_Move playerMove;
    Lock_Enemy lockEnemy;
    GameManager gameManager;
    Camera mainCamera;



    [SerializeField] ParticleSystem die;
    private Animator animator;
    private bool isSpawm = false;

    [SerializeField] private float delayAttack = 1f;

    [SerializeField] private bool IsBlocking = false;
    [SerializeField] private bool IsCombat = false;
    [SerializeField] private bool IsMove = false;
    [SerializeField] private bool IsHit = false;
    [SerializeField] private int HitNum = 0;
    [SerializeField] GameObject nearestEnemy = null;

    [SerializeField] private bool IsWin = false;
    [SerializeField] private bool IsDie = false;
    [SerializeField] private Effect effect;

    void Start()
    {
        playerMove = GetComponent<Player_Move>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        playerCombat = GetComponent<Combat_Action>();
        playerGesture = GetComponent<Player_Gesture>();
        lockEnemy = GetComponent<Lock_Enemy>();
        animator = GetComponent<Animator>();
        effect = GetComponent<Effect>();
        StartCoroutine(SetSpawm());
    }

    private IEnumerator DieWait()
    {

        yield return new WaitForSeconds(2f);
        if (!IsDie)
        {
            IsDie = true;
            effect.DieEffect(transform.position);
        }
        gameManager.DownPlayer();
    }
    void Update()
    {
        if (!isSpawm)
        {

            return;
        }
        else
        {
            if (playerCombat.GetHeal() <= 0)
            {
                // playerCombat.SetCombat(false);
                gameObject.tag = "Untagged";
                playerCombat.Die();
                StartCoroutine(DieWait());
                return;
            }
            else
            {

                nearestEnemy = lockEnemy.GetTargetEnemy();
                playerMove.SetMoveInput(playerGesture.MoveControle());
                if (!nearestEnemy)
                {
                    // mainCamera.GetComponent<Camera_Follow>().SetTarget(gameObject);
                    mainCamera.GetComponent<Camera_Follow>().ZoomOut();
                    playerCombat.SetCombat(false);
                    playerMove.MovePlayer();
                }
                else
                {
                    playerCombat.SetCombat(true);
                    lockEnemy.LookAtEnemy(nearestEnemy);
                    mainCamera.GetComponent<Camera_Follow>().ZoomIn();
                    // Vector3 midpoint = (nearestEnemy.transform.position + gameObject.transform.position) / 2f;
                    // GameObject midTarget = new GameObject("MidPointTarget");
                    // midTarget.transform.position = midpoint;
                    // mainCamera.GetComponent<Camera_Follow>().SetTarget(midTarget);


                    if (playerGesture.IsLongPress() && !playerMove.getMoving())
                    {
                        playerCombat.Blocking(true);
                        return;
                    }
                    else
                    {
                        playerCombat.Blocking(false);
                    }

                    if (playerGesture.IsDoubleTap() && !playerMove.getMoving())
                    {
                        playerCombat.Attack();
                    }
                    playerMove.CombatMove();


                }
            }
        }



    }

    private IEnumerator SetSpawm()
    {
        playerMove.SetSpawm();
        yield return new WaitForSeconds(3.5f);
        isSpawm = true;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

}
