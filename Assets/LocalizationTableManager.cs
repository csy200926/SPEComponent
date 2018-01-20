using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SPEUI
{

    public enum LocalizationLanguage
    {
        English = 0,
        Chinese,
        CPlusPlus,
        CSharp,
    }


    public enum LocalizationCategory
    {
        UIText = 0,
        Dialog,
        Hint,
        System,
    }

    [System.Serializable]
    public class CategoryWrapper
    {
        public List<TextAsset> textList = new List<TextAsset>();
    }


    [System.Serializable]
    public class LanguageWrapper
    {
        public List<CategoryWrapper> categories = new List<CategoryWrapper>();
    }

    
    public class LocalizationTableManager : MonoBehaviour
    {
    
        public LocalizationLanguage m_currentLanguage = LocalizationLanguage.Chinese;

        public List<LanguageWrapper> m_languages = new List<LanguageWrapper>();
        //     language-->category-->elements


        private void Awake()
        {
            Reset();

            LanguageWrapper categoryAssetList = m_languages[(int)m_currentLanguage];

            for (int i = 0; i < categoryAssetList.categories.Count; i++)
            {
                LocalizationCategory category = (LocalizationCategory)i;
                LoadTable(category, categoryAssetList.categories[i].textList.ToArray());
            }

        }

        public static void Reset()
        {
            // Clear old content
            s_categoryLocalizationTableMap.Clear();

            // Initialize category dictionary
            foreach (LocalizationCategory category in System.Enum.GetValues(typeof(LocalizationCategory)))
            {
                Dictionary<string, string> localizationTable = new Dictionary<string, string>();
                s_categoryLocalizationTableMap.Add(category, localizationTable);
            }
        }

        private static Dictionary<LocalizationCategory, Dictionary<string, string>> s_categoryLocalizationTableMap = new Dictionary<LocalizationCategory, Dictionary<string, string>>();

        public static bool LoadTable(LocalizationCategory category,TextAsset[] textAssets)
        {
            Dictionary<string, string> localizationTable = s_categoryLocalizationTableMap[category];

            foreach (TextAsset textAsset in textAssets)
            {
                if (textAsset == null) continue;
                string textStream = textAsset.text;
                LoadFromStream(textStream, localizationTable);
            }

            return false;
        }

        private static void LoadFromStream(string text, Dictionary<string, string> localizationTable)
        {
            StreamReader reader = new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)));

            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                string[] splitedStr = line.Split('=');
                if (splitedStr.Length < 2) continue;

                string key = splitedStr[0];
                string content = splitedStr[1];

                localizationTable.Add(key, content);
            }

        }

        public static string GetLocalizetionText(string textKey, LocalizationCategory category)
        {
            string result = "None!";

            Dictionary<string, string> localizationTable = s_categoryLocalizationTableMap[category];
            localizationTable.TryGetValue(textKey, out result);

            return result;
        }





    }
}
