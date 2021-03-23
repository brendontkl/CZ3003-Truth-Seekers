using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class CharactersSel : MonoBehaviour
{
    ToggleGroup toogleGroupInstance;
    static int CharactersInt = 0;
    public Button ConfirmBtn;

    private GameObject spawnPos;
    public GameObject Warrior, Magician, Knight, Thief;


    public Toggle currentSelection
    {
        get { return toogleGroupInstance.ActiveToggles().FirstOrDefault(); }
    }
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "CharacterSelection")
        {
            Button btn = ConfirmBtn.GetComponent<Button>();
            btn.onClick.AddListener(LoadNextScene);
            toogleGroupInstance = GetComponent<ToggleGroup>();
        }
        else if (sceneName == "WorldSelection")
        {
            
        }
        else
        {
            spawnPos = GameObject.FindWithTag("PlayerSpawnPoint");
            CreateChar();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void SelectedChar()
    {

        if (currentSelection.name == "Magician")
        {
            CharactersInt = 1;
        }
        else if (currentSelection.name == "Knight")
        {
            CharactersInt = 2;
        }
        else if (currentSelection.name == "Thief")
        {
            CharactersInt = 3;
        }
    }

    public void loadlevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    void LoadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "CharacterSelection")
        {
            SelectedChar();
            Debug.Log("Hello");
        }
        loadlevel("WorldSelection");
    }

    void CreateChar()
    {
        if(CharactersInt == 0)
        {
            GameObject myWarrior = Instantiate(Warrior, spawnPos.transform.position, Quaternion.identity) as GameObject;
            myWarrior.transform.parent = transform;
        }
        else if (CharactersInt == 1)
        {
            GameObject myMagician = Instantiate(Magician, spawnPos.transform.position, Quaternion.identity) as GameObject;
            myMagician.transform.parent = transform;
            //Instantiate(Magician, spawnPos.position, spawnPos.rotation);
        }
        else if (CharactersInt == 2)
        {
            GameObject myKnight = Instantiate(Knight, spawnPos.transform.position, Quaternion.identity) as GameObject;
            myKnight.transform.parent = transform;
            //Instantiate(Knight, spawnPos.position, spawnPos.rotation);
        }
        else
        {
            GameObject myThief = Instantiate(Thief, spawnPos.transform.position, Quaternion.identity) as GameObject;
            myThief.transform.parent = transform;
            //Instantiate(Thief, spawnPos.position, spawnPos.rotation);
        }
    }
}
