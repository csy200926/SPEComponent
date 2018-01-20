using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
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

    public class SPEUIBase : MonoBehaviour {

        public enum HorizontalAlign
        {
            Left,
            Center,
            Right
        }

        public enum VerticalAlign
        {
            Top,
            Center,
            Bottom
        }

        public enum Extension
        {
            None,
            Width,
            Height
        }

        public Margin m_margin;

        public HorizontalAlign m_horizontalAlgin;
        public VerticalAlign m_verticalAlign;
        public Extension m_extension;

        public bool update = false;

        private float parentWidth = 0.0f;
        private float parentHeight = 0.0f;

        private RectTransform m_rectTransform;

        public string Name
        {
            get { return gameObject.name; }
        }

        // Use this for initialization
        protected virtual void Awake () {
            m_rectTransform = gameObject.GetComponent<RectTransform>();
            RectTransform m_parentRectTransform = m_rectTransform.parent.GetComponent<RectTransform>();

            parentWidth = m_parentRectTransform.GetWidth();
            parentHeight = m_parentRectTransform.GetHeight();

            RefreshLayout();

        }
	
        public virtual void RefreshLayout(bool refreshChildren = false)
        {

            RectTransform m_parentRectTransform = m_rectTransform.parent.GetComponent<RectTransform>();

            float newParentWidth = m_parentRectTransform.GetWidth();
            float newParentHeight = m_parentRectTransform.GetHeight();

            float scale = 1.0f;

            switch(m_extension)
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

            float width = m_rectTransform.rect.width * scale;
            float height = m_rectTransform.rect.height * scale;

            m_rectTransform.SetSize(new Vector2(width, height));


            switch (m_horizontalAlgin)
            {
                case HorizontalAlign.Right:
                    {
                        m_rectTransform.localPosition = new Vector3(parentWidth - width * 0.5f - m_margin.right, m_rectTransform.localPosition.y, 0);
                        break;
                    }
                case HorizontalAlign.Center:
                    {
                        m_rectTransform.localPosition = new Vector3(parentWidth * 0.5f, m_rectTransform.localPosition.y, 0);
                        break;
                    }
                case HorizontalAlign.Left:
                    {
                        m_rectTransform.localPosition = new Vector3(width * 0.5f + m_margin.left, m_rectTransform.localPosition.y, 0);
                        break;
                    }
            }

            switch(m_verticalAlign)
            {

                case VerticalAlign.Top:
                    {
                        m_rectTransform.localPosition = new Vector3(m_rectTransform.localPosition.x, parentHeight - height * 0.5f - m_margin.top, 0);
                        break;
                    }
                case VerticalAlign.Center:
                    {
                        m_rectTransform.localPosition = new Vector3(m_rectTransform.localPosition.x, parentHeight * 0.5f, 0);
                        break;
                    }
                case VerticalAlign.Bottom:
                    {
                        m_rectTransform.localPosition = new Vector3(m_rectTransform.localPosition.x, height * 0.5f + m_margin.bottom, 0);
                        break;
                    }
            }
            m_rectTransform.localPosition -= new Vector3(parentWidth * 0.5f, parentHeight * 0.5f, 0.0f);

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

        private Dictionary<string, MethodInfo> m_callBackMethodMap = new Dictionary<string, MethodInfo>();
        private List<SPEUIEventHandler> m_parentEventHandlers = new List<SPEUIEventHandler>();

        protected void MessageInitilize()
        {
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
                        m_callBackMethodMap.Add(methodinfo.Name, methodinfo);
                    }
                }
            }
            m_parentEventHandlers = new List<SPEUIEventHandler>(eventHandlers);
        }

        public void CallEvents(string eventName)
        {
            MethodInfo method = null;
            if(m_callBackMethodMap.TryGetValue(eventName,out method))
            {
                for(int i = m_parentEventHandlers.Count - 1; i >= 0; i--)
                {
                    if(m_parentEventHandlers[i] == null)
                    {
                        m_parentEventHandlers.RemoveAt(i);
                        continue;
                    }
                    object[] objs = { this };
                    method.Invoke(m_parentEventHandlers[i], objs);
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