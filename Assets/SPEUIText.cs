using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SPEUI
{
    [RequireComponent(typeof(Text))]
    public class SPEUIText : SPEUIBase
    {
        public string Text
        {
            get { return text.text; }

            set { text.text = value; }
        }

        public string localizationKey = "";
        private Text text;

        protected override void Awake()
        {
            base.Awake();

            text = GetComponent<Text>();

            LocalizeText();
        }

        private void LocalizeText()
        {
            if( localizationKey.Length > 0 )
            {
                text.text = LocalizationTableManager.GetLocalizetionText(localizationKey,LocalizationCategory.UIText);
            }
        }
 

    }

}
