using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SPEUI
{
    public class SPEUISlot : SPEUIBase, IDropHandler
    {
        protected override void Awake()
        {
            base.Awake();
            MessageInitilize();
        }
        public string onDropEventName = "OnUIDrop";
        public SPEUIDraggable draggableItem = null;
        public void OnDrop(PointerEventData data)
        {
            if (data.pointerDrag != null)
            {
                SPEUIDraggable dragableScript = data.pointerDrag.GetComponent<SPEUIDraggable>();
                if(dragableScript != null && dragableScript.freeDragAndDrop == false)
                {
                    draggableItem = dragableScript;
                    dragableScript.transform.SetParent(transform);
                    dragableScript.SetTargetSlot(this);

                    CallEvents(onDropEventName);
                }
            }
        }
    }
}