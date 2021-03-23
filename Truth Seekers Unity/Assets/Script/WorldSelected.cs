using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class WorldSelected : MonoBehaviour
{
    ToggleGroup toogleGroupInstance;
    public Button ConfirmBtn;

    public Toggle currentSelection
    {
        get { return toogleGroupInstance.ActiveToggles().FirstOrDefault(); }
    }
    // Start is called before the first frame update
    void Start()
    {
        Button btn = ConfirmBtn.GetComponent<Button>();
        btn.onClick.AddListener(SelectedWorld);
        toogleGroupInstance = GetComponent<ToggleGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //select world based on toggle
    //toggle interact is enable/disabled based on users progression
    public void SelectedWorld()
    {
        loadlevel(currentSelection.name);
    }

    public void loadlevel(string level)
    {
        SceneManager.LoadScene(level);
    }

}
