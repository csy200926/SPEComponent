using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SPEUI
{

    public class SPEUIRadioButton : SPEUIBase
    {

        public GameObject selectedMark;
        public string onClickEvent;

        [HideInInspector]
        public int index = 0;

        private Button button;
        private bool isSelected = false;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            MessageInitilize();
            button = GetComponent<Button>();

            button.onClick.AddListener(delegate { CallEvents(onClickEvent); });
            button.onClick.AddListener(delegate { OnSelected(); });

            // Enable first radio button as default
            Transform parentTransform = transform.parent;
            SPEUIRadioButton[] radioButtons = parentTransform.GetComponentsInChildren<SPEUIRadioButton>();

            // Give index and default state
            int count = 0;
            foreach (SPEUIRadioButton radioButton in radioButtons)
            {
                radioButton.index = count;
                radioButton.Toggle(false);
                count++;
            }

            radioButtons[0].Toggle(true);

        }

        private void OnSelected()
        {
            Transform parentTransform = transform.parent;
            SPEUIRadioButton[] radioButtons = parentTransform.GetComponentsInChildren<SPEUIRadioButton>();

            foreach(SPEUIRadioButton radioButton in radioButtons)
            {
                radioButton.Toggle(false);
            }

            Toggle(true);
        }

        
        private void Toggle(bool selected)
        {
            isSelected = selected;

            if (selectedMark != null)
                selectedMark.SetActive(selected);
            else
                Debug.LogError("No selection mark!");
        }
    }
}