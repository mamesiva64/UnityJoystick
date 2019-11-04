using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickTest : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        var pos = Input.mousePosition;
        {
            var rect = this.GetComponent<RectTransform>();

            if (Input.GetMouseButton(0))
            {
                Debug.Log(RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition).ToString());
            }

        }
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }

    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");

    }

    private void OnDrawGizmosSelected()
    {
        RectTransform rectTransform = transform as RectTransform;

        float xOffset = transform.position.x - transform.localPosition.x;
        float yOffset = transform.position.y - transform.localPosition.y;

        float x1 = (transform.localPosition.x - (rectTransform.sizeDelta.x * rectTransform.pivot.x) * transform.lossyScale.x) + xOffset;
        float x2 = (transform.localPosition.x + (rectTransform.sizeDelta.x * (1f - rectTransform.pivot.x) * transform.lossyScale.x)) + xOffset;
        float y1 = (transform.localPosition.y - (rectTransform.sizeDelta.y * rectTransform.pivot.y) * transform.lossyScale.y) + yOffset;
        float y2 = (transform.localPosition.y + (rectTransform.sizeDelta.y * (1f - rectTransform.pivot.y) * transform.lossyScale.y)) + yOffset;

        float max = +100000f;
        float min = -100000f;
        Gizmos.color = new Color32(255, 0, 0, 100);
        Gizmos.DrawLine(new Vector3(x1, min, 0f), new Vector3(x1, max, 0f));
        Gizmos.DrawLine(new Vector3(x2, min, 0f), new Vector3(x2, max, 0f));
        Gizmos.DrawLine(new Vector3(min, y1, 0f), new Vector3(max, y1, 0f));
        Gizmos.DrawLine(new Vector3(min, y2, 0f), new Vector3(max, y2, 0f));
    }
}
