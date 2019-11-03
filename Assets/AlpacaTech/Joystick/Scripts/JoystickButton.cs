using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AlpacaTech
{
    public class JoystickButton : MonoBehaviour
    {
        public enum Type
        {
            Button,
            Left,
            Right,
            Up,
            Down,
        }

        [SerializeField] public int ID = 0;
        [SerializeField] private Type type = Type.Button;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Color colorDefault = Color.white;
        [SerializeField] private Color colorPress = Color.gray;

        private bool isPress = false;

        public bool IsPress
        {
            get
            {
                return isPress;
            }
        }

        private void Awake()
        {
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => { OnDown(); });

            var pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => { OnUp(); });

            var triggers = GetComponent<EventTrigger>().triggers;
            triggers.Add(pointerDown);
            triggers.Add(pointerUp);
        }

        private void Start()
        {
            switch (type)
            {
                case Type.Button: JoystickManager.Instance.buttons[ID] = this; break;
                case Type.Left: JoystickManager.Instance.buttonSticks[0] = this; break;
                case Type.Up: JoystickManager.Instance.buttonSticks[1] = this; break;
                case Type.Right: JoystickManager.Instance.buttonSticks[2] = this; break;
                case Type.Down: JoystickManager.Instance.buttonSticks[3] = this; break;
            }
        }

        public void OnDown()
        {
            isPress = true;
        }

        private void OnUp()
        {
            isPress = false;
        }

        void Update()
        {
        }


        private void OnDrawGizmos()
        {
            var rectTransform = transform as RectTransform;
            var rect = new Rect();

            var center = (Vector2)(transform.position);
            center.x -= rectTransform.sizeDelta.x * rectTransform.pivot.x * transform.lossyScale.x;
            center.y -= rectTransform.sizeDelta.y * rectTransform.pivot.y * transform.lossyScale.y;
            rect.center = center;

            var size = rectTransform.rect.size;
            size.x *= rectTransform.lossyScale.x;
            size.y *= rectTransform.lossyScale.y;
            rect.size = size;

            Gizmos.color = new Color32(255, 0, 0, 100);
            Gizmos.DrawCube(rect.center, rect.size);
        }
    }
}