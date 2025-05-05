using System.Collections;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject allySwawpPoint;
    [SerializeField] GameObject enemySwawpPoint;
    [SerializeField] ObjectPool objectPool;

    [SerializeField] GameObject player;

    [SerializeField] private int round;
    [SerializeField] private int allyCount = 1;
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private bool isGameWin = false;
    [SerializeField] private int mode = 0;
    [SerializeField] private Setting setting;
    [SerializeField] private GameObject LOSEUI;
    [SerializeField] private GameObject WINUI;
    [SerializeField] private GameObject HEALUI;
    [SerializeField] private TextMeshProUGUI RoundText;

    bool spawming = false;

    public void SetRoundText()
    {
        RoundText.text = "Round " + round.ToString();
    }


    public int GetRound()
    {
        return round;
    }
    public void UpRound()
    {
        round++;

    }
    public void DownRound()
    {
        round--;
    }

    public void SpawmAlly(GameObject position)
    {
        GameObject ally = objectPool.GetAlly();
        ally.transform.position = position.transform.position;
        ally.transform.rotation = position.transform.rotation;
        ally.SetActive(true);
        allyCount++;
    }

    public void SpawmEnemy(GameObject position)
    {
        GameObject enemy = objectPool.GetEnemy();
        enemy.transform.position = position.transform.position;
        enemy.transform.rotation = position.transform.rotation;
        enemy.SetActive(true);
        enemyCount++;
    }
    public void SpawnPlayer()
    {
        Instantiate(player, allySwawpPoint.transform.position, allySwawpPoint.transform.rotation);
        allyCount++;

    }

    public void DownPlayer()
    {
        allyCount--;
        CheckGame();
    }


    public void DownNpc(GameObject npc)
    {
        if (npc.gameObject.tag == "Ally")
        {
            objectPool.ReturnEnemy(npc);
            allyCount--;
        }
        else
        {
            objectPool.ReturnEnemy(npc);
            enemyCount--;
        }
        CheckGame();
    }

    public void Win()
    {
        HEALUI.SetActive(false);

        WINUI.SetActive(true);

    }


    public void CheckGame()
    {

        if (round > 10)
        {
            Win();
        }
        else
        {
            if (allyCount <= 0)
            {
                GameOver();
            }
            else if (enemyCount <= 0)
            {
                UpRound();
                SetRoundText();
                SpawnRound();
            }
        }


    }

    public int GetMode()
    {
        return mode;
    }

    public void SetMode(int setmode)
    {
        mode = setmode;
    }

    public void SpawnRound()
    {
        if (mode == 2)
        {
            for (int i = 0; i < round + 2; i++)
            {
                StartCoroutine(DelaySpawnWithDelay(true, i * 2f));
            }

            for (int i = 0; i <= round * 2; i++)
            {
                StartCoroutine(DelaySpawnWithDelay(false, i * 2f));
            }
        }
        if (mode == 1)
        {
            for (int i = 0; i <= round * 2; i++)
            {
                StartCoroutine(DelaySpawnWithDelay(false, i * 2f));
            }
        }
        else if (mode == 0)
        {
            StartCoroutine(DelaySpawnWithDelay(false, 0f));
        }
    }

    private IEnumerator DelaySpawnWithDelay(bool isAlly, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isAlly)
        {
            SpawmAlly(allySwawpPoint);
        }
        else
        {
            SpawmEnemy(enemySwawpPoint);
        }
    }

    public void GameOver()
    {
        HEALUI.SetActive(false);
        LOSEUI.SetActive(true);

    }
    public void SetGameOver()
    {
        isGameOver = true;

    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1);
        SpawnRound();
    }

    [System.Obsolete]
    void Start()
    {
        SetRoundText();
        setting = FindObjectOfType<Setting>();
        mode = setting.GetMode();
        // SpawnPlayer();
        StartCoroutine(DelayStart());


    }


}
