using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutsideRegion : MonoBehaviour, IDropHandler
{
    // This class handles the behavior when an item is dropped outside of valid drop zones.
    public void OnDrop(PointerEventData eventData)
    {
        // Destroy the item being dragged when it's dropped outside of a valid drop zone.
        Destroy(DragHandler.itemBeingDragged);
    }
}
