using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public GameObject EnemyMob, EnemyBoss0, EnemyBoss1, EnemyBoss2, EnemyBoss3;
    public int enemyHP;
    public List<GameObject> enemies = new List<GameObject>();
    public Animator animator;

    private float sec = 3.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnEnemy(string[] MCQs)
    {
        int qnsCount = MCQs.Length / 9;
        int enemyMobCount = 0;

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        double x = -2.0, y = 1.5, z = 0;
        double adjustment = -2;
        
        //foreach (string s in MCQs)
        //{
        //    Debug.Log(s);
        //}
        if (sceneName == "World 1")
        {
            enemyMobCount = qnsCount - 5;
        }
        else if (sceneName == "World 2")
        {
            enemyMobCount = qnsCount - 6;
        }
        else if (sceneName == "World 3")
        {
            enemyMobCount = qnsCount - 7;
        }
        else if (sceneName == "World 4")
        {
            enemyMobCount = qnsCount - 8;
        }

        //global enemyMobCount2
        //
        //Debug.Log(qnsCount);
        //enemyMobCount += 7;
        for (int i = 0; i < enemyMobCount; i++)
        {
            adjustment += 2;
            if(adjustment == 6)
            {
                adjustment = 0;
            }
            if (i >= 3)
            {
                GameObject newGO = (GameObject)Instantiate(EnemyMob, new Vector3((float)x - 2, (float)y - (float)adjustment, (float)z), Quaternion.identity);
                enemies.Add(newGO);
            }
            else if(i <= 2)
            {
                GameObject newGO = (GameObject)Instantiate(EnemyMob, new Vector3((float)x, (float)y - (float)adjustment, (float)z), Quaternion.identity);
                enemies.Add(newGO);
            }

            if (i == 6)
            {
                enemyMobCount -= 6;
                break;
            }
        }
        //attack animation when player made wrong answer
    }

    void countEnemy()
    {

    }
    public void EnemyDeath()
    {
        Debug.Log("Dead");
        enemies[0].GetComponent<Enemy>().playDeathAni();
        StartCoroutine(DeleteCall());
    }
    IEnumerator DeleteCall()
    {
        yield return new WaitForSeconds(sec);
        GameObject Temp = enemies[0];
        enemies.RemoveAt(0);
        Destroy(Temp);

    }
    public void playDeathAni()
    {
        animator.SetBool("Die", true);
    }
    public void playAtkAni()
    {
        animator.SetBool("Attack", true);
    }
}