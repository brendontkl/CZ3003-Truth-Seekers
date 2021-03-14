using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
 void Awake() 
 {
     DontDestroyOnLoad(transform.gameObject);
 }
}
