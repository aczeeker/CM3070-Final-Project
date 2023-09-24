using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged; // The item currently being dragged
    Vector3 startPosition; // The initial position of the dragged item
    Transform startParent; // The initial parent of the dragged item
    Transform last; // Reference to the last known parent (for resetting)

    void Start()
    {
        // Find the last known parent (where the item will return if not dropped on a valid target)
        last = GameObject.Find("InventoryCanvas").transform.Find("Inventory").Find("Last");
    }

    public Transform getStartParent()
    {
        return startParent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // When the drag begins, set the itemBeingDragged and store initial properties
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(last); // Move the item to the "last" parent for proper layering
        GetComponent<CanvasGroup>().blocksRaycasts = false; // Disable raycasting on the item being dragged
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the dragged item to follow the mouse/finger
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // When the drag ends, reset the itemBeingDragged and restore properties
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true; // Enable raycasting again

        // If the item was not dropped on a valid target, return it to its initial position and parent
        if (transform.parent == last)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }
}
