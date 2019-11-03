using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPanel : MonoBehaviour
{
    public bool MobileOnly = true;

    private void Awake()
    {
        if (MobileOnly)
        {
#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
//          this.gameObject.SetActive(true);
#else
#if !UNITY_EDITOR
            this.gameObject.SetActive(false);
#endif
#endif
        }
    }
}
