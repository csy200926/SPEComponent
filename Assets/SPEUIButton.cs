using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SPEUI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class SPEUIButton : SPEUIBase
    {
        public string m_onClickEvent;

        private Button m_button;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            MessageInitilize();
            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(delegate { CallEvents(m_onClickEvent); });
        }

        

    }
}
