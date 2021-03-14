using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTest : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        WWW request = new WWW("http://localhost/truthseekers/webtest.php");
        yield return request;
        Debug.Log(request.text);
    }

  
}
