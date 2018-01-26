using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SPEUI
{
    public class SPEUIDraggable : SPEUIBase, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter
    {
        public bool freeDragAndDrop = false;

        private Transform startParent;
        private Vector3 startPosition;
        private Vector2 offset;

        public SPEUISlot lastSlot = null;
        public SPEUISlot currentSlot = null;

        private bool canRayCast = true;


            public void SetTargetSlot(SPEUISlot _slotObject)
        {
            lastSlot = currentSlot;
            currentSlot = _slotObject;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            offset = new Vector2(transform.position.x, transform.position.y) - eventData.position;

            startPosition = transform.position;
            startParent = transform.parent;
            canRayCast = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position + offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canRayCast = true;

            // Free drag and drop
            if (freeDragAndDrop == true) return;

            // Put into slot
            if (transform.parent == startParent)
            {
                transform.position = startPosition;
            }else
            {
                transform.position = transform.parent.position;
            }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            return canRayCast;
        }
    }
}