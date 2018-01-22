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
        public string onClickEvent;

        private Button button;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            MessageInitilize();
            button = GetComponent<Button>();
            button.onClick.AddListener(delegate { CallEvents(onClickEvent); });
        }

        

    }
}
