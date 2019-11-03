using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JoystickButtonState
{
    public bool press = false;
    public bool pressBack = false;
    public int pressCount = 0;
    public float pressTime = 0;

}
