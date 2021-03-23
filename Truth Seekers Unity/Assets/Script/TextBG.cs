using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBG : MonoBehaviour
{
    Image image;
    void Start()
    {
        image = GetComponent<Image>();

        Color c = image.color;
        c.a = 0.8f;
        image.color = c;
    }
}
