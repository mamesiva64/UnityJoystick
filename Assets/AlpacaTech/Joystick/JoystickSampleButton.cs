using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickSampleButton : MonoBehaviour
{
    [SerializeField] Text text;

    int count = 0;

    public void OnButtonDown()
    {
        count++;
        text.text = count.ToString();
    }

}
