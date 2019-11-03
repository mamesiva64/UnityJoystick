using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlpacaTech
{
    /// <summary>
    /// HowToUse
    /// Script Execution Order 
    /// JoystickButton      -202
    /// JoystickStick       -201
    /// JoystickManager     -200
    /// 
    /// </summary>
    public class JoystickManager : MonoBehaviour
    {
        public static JoystickManager Instance;
        public static int ButtonMax = 16;

        [Header("Config")]
        [SerializeField] List<string> buttonNames = new List<string>() { "Jump", "Fire1", "Fire2", "Fire3" };

        [Header("workarea")]
        [HideInInspector] public JoystickStick[] sticks = new JoystickStick[2];
        [HideInInspector] public JoystickButton[] buttons = new JoystickButton[ButtonMax];
        [HideInInspector] public JoystickButton[] buttonSticks = new JoystickButton[4];

        [Header("state")]
        private Vector2[] axis = new Vector2[2];
        private JoystickButtonState[] buttonStates = new JoystickButtonState[ButtonMax];
        private JoystickButtonState[] buttonStickStates = new JoystickButtonState[4];

        void Awake()
        {
            Instance = this;
            buttonStates = new JoystickButtonState[ButtonMax];
            buttonStickStates = new JoystickButtonState[4];
            for (int i = 0; i < buttonStates.Length; ++i)
            {
                buttonStates[i] = new JoystickButtonState();
            }
            for (int i = 0; i < buttonStickStates.Length; ++i)
            {
                buttonStickStates[i] = new JoystickButtonState();
            }
        }

        void Update()
        {
            UpdateBackup();

            axis[0].x = Input.GetAxis("Horizontal");
            axis[0].y = Input.GetAxis("Vertical");
            foreach (var stick in sticks)
            {
                if (stick)
                {
                    if (stick.IsPress)
                    {
                        axis[stick.ID] = stick.Axis;
                    }
                }
            }
            for(int id = 0; id < 4; ++id)
            {
                UpdateCursorKey(id);
            }

            //  button
            for (int i = 0; i < buttonNames.Count; ++i)
            {
                buttonStates[i].press = Input.GetButton(buttonNames[i]);
            }
            foreach (var button in buttons)
            {
                if (button)
                {
                    buttonStates[button.ID].press = button.IsPress;
                }
            }
        }

        void UpdateCursorKey(int id)
        {
            var button = buttonSticks[id];
            if (button)
            {
                if (button.IsPress)
                {
                    switch (id)
                    {
                        case 0: axis[0].x = -1f; break;
                        case 1: axis[0].y = -1f; break;
                        case 2: axis[0].x = +1f; break;
                        case 3: axis[0].y = +1f; break;
                    }
                }
            }
        }

        void UpdateBackup()
        {
            foreach (var buttonState in buttonStates)
            {
                buttonState.pressBack = buttonState.press;
            }
            foreach (var buttonState in buttonStickStates)
            {
                buttonState.pressBack = buttonState.press;
            }
        }


        public static Vector2 GetAxis(int stickId = 0)
        {
            return Instance.axis[stickId];
            //            return Instance.sticks[stickId].Axis;
        }

        public static bool GetButtonDown(int id)
        {
            return Instance.buttonStates[id].press && !Instance.buttonStates[id].pressBack;
        }

        public static bool GetButtonUp(int id)
        {
            return !Instance.buttonStates[id].press && Instance.buttonStates[id].pressBack;
        }

        public static bool GetButton(int id)
        {
            return Instance.buttonStates[id].press;
        }

        public static bool GetButtonRepat(int id)
        {
            return false;
        }
    }
}