using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ReadMCQ : MonoBehaviour
{
    private GameObject EnemyMCQ;
    private GameObject gameHandler;
    public TMP_Text MCQ_Qns, Ans1, Ans2, Ans3, Ans4;
    private int qnsLocation = 0;
    List<string> currQns = new List<string>();
    public string[] AllQnsAns;
    // Start is called before the first frame update
    void Start()
    {
        
        string path = "Assets\\Resources\\MCQ_dummyQuestions.txt";
        AllQnsAns = System.IO.File.ReadAllLines(path);

        EnemyMCQ = GameObject.FindWithTag("EnemyHandler");
        EnemyMCQ.GetComponent<Enemy>().spawnEnemy(AllQnsAns);

        gameHandler = GameObject.FindWithTag("GameController");
        //Debug.Log(AllQnsAns.Length);
        //each qns should hvae 9 lines 1st line qns follow by each answer and it's True False value

        PutinIndividual(AllQnsAns);
    }
    void DisplayQNS()
    {
        //Debug.Log();
        MCQ_Qns.text = "Q." + currQns[0];
        Ans1.text = "1. " + currQns[1];
        Ans2.text = "2. " + currQns[3];
        Ans3.text = "3. " + currQns[5];
        Ans4.text = "4. " + currQns[7];
        gameHandler.GetComponent<GameHandler>().getcurrMCQ(currQns);
    }
    void PutinIndividual(string[] AllQnsAns)
    {
        //int qnsCount = AllQnsAns.Length / 9;
        //Debug.Log(AllQnsAns.Length);

        if(AllQnsAns.Length > qnsLocation)
        {
            currQns.Clear();
            for (int i = qnsLocation; i < qnsLocation + 9; i++)
            {
                currQns.Add(AllQnsAns[i]);
            }
            DisplayQNS();
        }
        else
        {
            Debug.Log("End game");
            //qns end game end count score
        }
        
    }
    public void nextQns()
    {
        qnsLocation += 9;
        PutinIndividual(AllQnsAns);
    }
}
