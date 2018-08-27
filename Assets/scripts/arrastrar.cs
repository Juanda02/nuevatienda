using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class arrastrar : MonoBehaviour, IDragHandler, IBeginDragHandler {

    private Vector2 offset;
    private bool drag;

	public void OnBeginDrag (PointerEventData e)
    {
        if (transform.GetComponent<RectTransform>().rect.Contains(transform.InverseTransformPoint(e.pressPosition)))
        {
            offset = e.position - new Vector2(this.transform.position.x, this.transform.position.y);
            drag = true;
        }
        else
        {
            drag = false;
        }
	}
	
	public void OnDrag (PointerEventData e)
    {
        if (drag)
        {
            this.transform.position = e.position - offset;
        }
	}
}
