using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public GameObject Alert, MCQ_UI;
    private GameObject enemyHandler;
    public Button Ans1, Ans2, Ans3, Ans4, cfmbtn;
    private GameObject gameHandler;
    private bool Ans1Sel = false, Ans2Sel = false, Ans3Sel = false, Ans4Sel = false;
    private float sec = 2.0f;
    private List<string> currMCQs;
    //public GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameObject.FindWithTag("GameController");
        enemyHandler = GameObject.FindWithTag("EnemyHandler");
        Alert.SetActive(false);
        Button btn = Ans1.GetComponent<Button>();
        btn.onClick.AddListener(SelectedAns1);
        Button btn2 = Ans2.GetComponent<Button>();
        btn2.onClick.AddListener(SelectedAns2);
        Button btn3 = Ans3.GetComponent<Button>();
        btn3.onClick.AddListener(SelectedAns3);
        Button btn4 = Ans4.GetComponent<Button>();
        btn4.onClick.AddListener(SelectedAns4);

        Button cfmbtn1 = cfmbtn.GetComponent<Button>();
        cfmbtn1.onClick.AddListener(confirmAns);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SelectedAns1()
    {
        //Debug.Log("You have clicked the Ans1!");
        Ans1Sel = true;
        Ans2Sel = false;
        Ans3Sel = false;
        Ans4Sel = false;
        Ans1.GetComponent<TextBG>().Selectedbtn();
        Ans2.GetComponent<TextBG>().Unselectedbtn();
        Ans3.GetComponent<TextBG>().Unselectedbtn();
        Ans4.GetComponent<TextBG>().Unselectedbtn();
    }
    void SelectedAns2()
    {
        Ans1Sel = false;
        Ans2Sel = true;
        Ans3Sel = false;
        Ans4Sel = false;
        Ans1.GetComponent<TextBG>().Unselectedbtn();
        Ans2.GetComponent<TextBG>().Selectedbtn();
        Ans3.GetComponent<TextBG>().Unselectedbtn();
        Ans4.GetComponent<TextBG>().Unselectedbtn();
    }
    void SelectedAns3()
    {
        Ans1Sel = false;
        Ans2Sel = false;
        Ans3Sel = true;
        Ans4Sel = false;
        Ans1.GetComponent<TextBG>().Unselectedbtn();
        Ans2.GetComponent<TextBG>().Unselectedbtn();
        Ans3.GetComponent<TextBG>().Selectedbtn();
        Ans4.GetComponent<TextBG>().Unselectedbtn();
    }
    void SelectedAns4()
    {
        Ans1Sel = false;
        Ans2Sel = false;
        Ans3Sel = false;
        Ans4Sel = true;
        Ans1.GetComponent<TextBG>().Unselectedbtn();
        Ans2.GetComponent<TextBG>().Unselectedbtn();
        Ans3.GetComponent<TextBG>().Unselectedbtn();
        Ans4.GetComponent<TextBG>().Selectedbtn();
    }

    void confirmAns()
    {
        if(Ans1Sel || Ans2Sel || Ans3Sel || Ans4Sel)
        {
            checkAns();
        }
        else
        {
            Alert.SetActive(true);
            StartCoroutine(LateCall());
            Debug.Log("Please select one of the answer");
        }
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(sec);
        Alert.SetActive(false);
    }

    public void getcurrMCQ(List<string> currQns)
    {
        currMCQs = currQns;
    }
    void checkAns()
    {
        if (Ans1Sel && currMCQs[2] == "T")
        {
            Debug.Log("Correct");
            enemyHandler.GetComponent<Enemy>().EnemyDeath();
        }
        else if (Ans2Sel && currMCQs[4] == "T")
        {
            Debug.Log("Correct");
            enemyHandler.GetComponent<Enemy>().EnemyDeath();
        }
        else if (Ans3Sel && currMCQs[6] == "T")
        {
            Debug.Log("Correct");
            enemyHandler.GetComponent<Enemy>().EnemyDeath();
        }
        else if (Ans4Sel && currMCQs[8] == "T")
        {
            Debug.Log("Correct");
            enemyHandler.GetComponent<Enemy>().EnemyDeath();
        }
        else
        {
            Debug.Log("Incorrect");
        }
        MCQ_UI.SetActive(false);
        StartCoroutine(DisplayCall());
    }

    IEnumerator DisplayCall()
    {
        yield return new WaitForSeconds(sec);
        gameHandler.GetComponent<ReadMCQ>().nextQns();
        MCQ_UI.SetActive(true);
    }
}
