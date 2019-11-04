using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AlpacaTech
{
    public class JoystickStick : MonoBehaviour
    {
        public enum MoveScope
        {
            Circle,
            Rect
        };

        [Header("Config")]
        [SerializeField] public int ID = 0;
        [SerializeField] private bool showImageAlways = false;
        [SerializeField] private float InputScope = 100.0f;

        [Header("GUI")]
        [SerializeField] private Image imageBase;
        [SerializeField] private Image imageHat;

        [Header("State")]
        private Vector2 axis = Vector2.zero;
        private bool isPress = false;

        [Header("Workarea")]
        private Vector3 startPosition = Vector3.zero;
        private int fingerId = -999;

        public Vector3 Axis
        {
            get
            {
                return axis;
            }
        }

        public bool IsPress
        {
            get
            {
                return isPress;
            }
        }

        void Awake()
        {
            ShowImage(false);
        }

        private void Start()
        {
            JoystickManager.Instance.sticks[ID] = this;
        }

        private void ShowImage(bool show)
        {
            if (!showImageAlways)
            {
                imageBase.enabled = show;
                imageHat.enabled = show;
            }
        }

        bool PositionCheck()
        {
            var pos = Input.mousePosition;
            var rect = GetComponent<RectTransform>();
            return RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition);
        }
        void OnTouchDown(int id)
        {
            if (isPress)
            {
                return;
            }
            if (!PositionCheck())
            {
                return;
            }

            ShowImage(true);
            var pos = Input.mousePosition;
            imageHat.transform.position = pos;
            imageBase.transform.position = pos;
            isPress = true;
            axis = Vector2.zero;
            startPosition = pos;
            fingerId = id;
        }

        void OnTouchPress(int id, Vector3 pos)
        {
            if (fingerId != id)
            {
                return;
            }
#if true
            //  Circle
            var delta = pos - startPosition;
            var len = delta.magnitude;
            len = Mathf.Min(len, InputScope);
            delta = delta.normalized * len;
            imageHat.transform.position = startPosition + delta;
            axis = delta / InputScope;
#else
            //  Rect
            pos.x = Mathf.Clamp(pos.x, startPosition.x - inputScope, startPosition.x + inputScope);
            pos.y = Mathf.Clamp(pos.y, startPosition.y - inputScope, startPosition.y + inputScope);

            imageHat.transform.position = pos;
            Axis.x = (pos.x - startPosition.x)/inputScope;
            Axis.y = (pos.y - startPosition.y)/inputScope;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pos"></param>
        void OnTouchUp(int id, Vector3 pos)
        {
            if (fingerId != id)
            {
                return;
            }

            isPress = false;
            axis = Vector2.zero;
            imageHat.transform.position = startPosition;
            ShowImage(false);
        }

        void Update()
        {
            var touches = Input.touches;
            axis = Vector2.zero;

            if (!isPress)
            {
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
#endif
            }

#if (UNITY_IOS || UNITY_ANDROID)
            //  Touch
            foreach (var touch in touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            OnTouchDown(touch.fingerId);
                        }
                        break;
                    case TouchPhase.Stationary:
                    case TouchPhase.Moved:
                        OnTouchPress(touch.fingerId, touch.position);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        OnTouchUp(touch.fingerId, touch.position);
                        break;
                }
            }
#endif

#if UNITY_EDITOR
            //  Mouse
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchDown(-1);
            }
            if (Input.GetMouseButton(0))
            {
                OnTouchPress(-1, Input.mousePosition);
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnTouchUp(-1, Input.mousePosition);
            }
#endif
        }

        private void OnDrawGizmosSelected()
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
            /*
            float xOffset = transform.position.x - transform.localPosition.x;
            float yOffset = transform.position.y - transform.localPosition.y;

            Gizmos.DrawCube(rect.center, rect.size);

            float x1 = xOffset + (transform.localPosition.x - (rectTransform.sizeDelta.x * rectTransform.pivot.x) * transform.lossyScale.x);
            float x2 = xOffset + (transform.localPosition.x + (rectTransform.sizeDelta.x * (1f - rectTransform.pivot.x) * transform.lossyScale.x));
            float y1 = yOffset + (transform.localPosition.y - (rectTransform.sizeDelta.y * rectTransform.pivot.y) * transform.lossyScale.y);
            float y2 = yOffset + (transform.localPosition.y + (rectTransform.sizeDelta.y * (1f - rectTransform.pivot.y) * transform.lossyScale.y));

            float max = +100000f;
            float min = -100000f;
            Gizmos.color = new Color32(255, 0, 0, 100);
            Gizmos.DrawLine(new Vector3(x1, min, 0f), new Vector3(x1, max, 0f));
            Gizmos.DrawLine(new Vector3(x2, min, 0f), new Vector3(x2, max, 0f));
            Gizmos.DrawLine(new Vector3(min, y1, 0f), new Vector3(max, y1, 0f));
            Gizmos.DrawLine(new Vector3(min, y2, 0f), new Vector3(max, y2, 0f));
            Gizmos.DrawCube(this.transform.position, new Vector3(1,1,0));
            */
        }
    }
}