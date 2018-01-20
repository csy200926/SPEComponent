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
            get { return m_text.text; }

            set { m_text.text = value; }
        }

        public string m_localizationKey = "";
        private Text m_text;

        protected override void Awake()
        {
            base.Awake();

            m_text = GetComponent<Text>();

            LocalizeText();
        }

        private void LocalizeText()
        {
            if( m_localizationKey.Length > 0 )
            {
                m_text.text = LocalizationTableManager.GetLocalizetionText(m_localizationKey,LocalizationCategory.UIText);
            }
        }
 

    }

}
