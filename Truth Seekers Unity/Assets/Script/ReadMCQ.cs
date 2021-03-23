using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ReadMCQ : MonoBehaviour
{
    private GameObject EnemyMCQ;
    public Text MCQ_Qns, Ans1, Ans2, Ans3, Ans4;


    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets\\Resources\\MCQ_dummyQuestions.txt";
        string[] AllQnsAns = System.IO.File.ReadAllLines(path);

        EnemyMCQ = GameObject.FindWithTag("Enemy");

        EnemyMCQ.GetComponent<Enemy>().spawnEnemy(AllQnsAns);
        //Debug.Log(AllQnsAns.Length);
        //each qns should hvae 9 lines 1st line qns follow by each answer and it's True False value
    }
}
