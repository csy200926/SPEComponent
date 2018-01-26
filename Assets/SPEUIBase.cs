using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SPEUI
{


    [System.Serializable]
    public struct Margin
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    public class SPEUIBase : MonoBehaviour
    {

        public enum HorizontalAlign
        {
            Left = 0,
            Center,
            Right
        }

        public enum VerticalAlign
        {
            Top = 0,
            Center,
            Bottom
        }

        public enum Extension
        {
            None = 0,
            Width,
            Height
        }

        // Storage
        public string customStrData = "";
        public int customIntData = 0;

        public Margin margin;

        public HorizontalAlign horizontalAlign;
        public VerticalAlign verticalAlign;
        public Extension extension;

        public bool update = false;

        private float parentWidth = 0.0f;
        private float parentHeight = 0.0f;

        private RectTransform rectTransform;

        public string Name
        {
            get { return gameObject.name; }
        }

        // Use this for initialization
        protected virtual void Awake () {
            rectTransform = gameObject.GetComponent<RectTransform>();
            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

            parentWidth = parentRectTransform.GetWidth();
            parentHeight = parentRectTransform.GetHeight();

            RefreshLayout();

        }
	
        public virtual void RefreshLayout(bool refreshChildren = false)
        {

            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

            float newParentWidth = parentRectTransform.GetWidth();
            float newParentHeight = parentRectTransform.GetHeight();

            float scale = 1.0f;

            switch(extension)
            {
                case Extension.None:
                    {
                        break;
                    }
                case Extension.Width:
                    {
                        scale = newParentWidth / parentWidth;
                        break;
                    }
                case Extension.Height:
                    {
                        scale = newParentHeight / parentHeight;
                        break;
                    }
            }

            parentWidth = newParentWidth;
            parentHeight = newParentHeight;

            float width = rectTransform.rect.width * scale;
            float height = rectTransform.rect.height * scale;

            rectTransform.SetSize(new Vector2(width, height));


            switch (horizontalAlign)
            {
                case HorizontalAlign.Right:
                    {
                        rectTransform.localPosition = new Vector3(parentWidth - width * 0.5f - margin.right, rectTransform.localPosition.y, 0);
                        break;
                    }
                case HorizontalAlign.Center:
                    {
                        rectTransform.localPosition = new Vector3(parentWidth * 0.5f, rectTransform.localPosition.y, 0);
                        break;
                    }
                case HorizontalAlign.Left:
                    {
                        rectTransform.localPosition = new Vector3(width * 0.5f + margin.left, rectTransform.localPosition.y, 0);
                        break;
                    }
            }

            switch(verticalAlign)
            {

                case VerticalAlign.Top:
                    {
                        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, parentHeight - height * 0.5f - margin.top, 0);
                        break;
                    }
                case VerticalAlign.Center:
                    {
                        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, parentHeight * 0.5f, 0);
                        break;
                    }
                case VerticalAlign.Bottom:
                    {
                        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, height * 0.5f + margin.bottom, 0);
                        break;
                    }
            }
            rectTransform.localPosition -= new Vector3(parentWidth * 0.5f, parentHeight * 0.5f, 0.0f);

            if(refreshChildren == true)
            {
                SPEUIBase[] childrenSPEUIBase = GetComponentsInChildren<SPEUIBase>();
                foreach (SPEUIBase child in childrenSPEUIBase)
                {
                    if(child != this)
                        child.RefreshLayout(false);
                }
            }
        }

        #region Events

        public delegate void UIEventDelegate(SPEUIBase sender);

        private Dictionary<string, MethodInfo> callBackMethodMap = new Dictionary<string, MethodInfo>();
        private List<SPEUIEventHandler> parentEventHandlers = new List<SPEUIEventHandler>();



        protected void MessageInitilize()
        {
            //TODO: 需要所有父对象的handler都接收么...
            SPEUIEventHandler[] eventHandlers = GetComponentsInParent<SPEUIEventHandler>(true);
            foreach(SPEUIEventHandler handler in eventHandlers)
            {
                // Get the public methods.
                MethodInfo[] methods = handler.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach(MethodInfo methodinfo in methods)
                {
                    var attrbutes = methodinfo.GetCustomAttributes(typeof(UIEventCall), true);
                    if(attrbutes != null && attrbutes.Length > 0)
                    {
                        callBackMethodMap.Add(methodinfo.Name, methodinfo);
                    }
                }
            }
            parentEventHandlers = new List<SPEUIEventHandler>(eventHandlers);
        }

        public void CallEvents(string eventName)
        {
            MethodInfo method = null;
            if(callBackMethodMap.TryGetValue(eventName,out method))
            {
                for(int i = parentEventHandlers.Count - 1; i >= 0; i--)
                {
                    if(parentEventHandlers[i] == null)
                    {
                        parentEventHandlers.RemoveAt(i);
                        continue;
                    }
                    object[] objs = { this };
                    method.Invoke(parentEventHandlers[i], objs);
                }
            }
        }
        #endregion

        // Update is called once per frame
        void Update () {
            if (update)
            {
                update = false;
                RefreshLayout(true);

            }

	    }


    }
}