using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using SPEUI;

[CustomEditor(typeof(LocalizationTableManager))]
public class LocalizationManagerInspector : Editor
{
    private LocalizationCategory currentDisplayCategory = LocalizationCategory.UIText;
    public override void OnInspectorGUI()
    {
        LocalizationTableManager myTarget = (LocalizationTableManager)target;
        myTarget.m_currentLanguage = (LocalizationLanguage)EditorGUILayout.EnumPopup("当前语言", myTarget.m_currentLanguage);


        // #1 Language to show
        LocalizationLanguage currentLanguage = myTarget.m_currentLanguage;

        // #2 Update language list
        int languageCount = System.Enum.GetValues(typeof(LocalizationLanguage)).Length;
        int newLanguageCount = languageCount - myTarget.m_languages.Count;
        for (int i = 0; i < newLanguageCount; i++)
        {
            myTarget.m_languages.Add(new LanguageWrapper());
        }

        // #3 Select language database
        LanguageWrapper categoryAssetList = myTarget.m_languages[(int)currentLanguage];


        //----------------------------------------------------------------------------
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //----------------------------------------------------------------------------

        // #1 Category to show
        int count = 0;
        var categoryText = new string[System.Enum.GetValues(typeof(LocalizationCategory)).Length];
        foreach (LocalizationCategory category in System.Enum.GetValues(typeof(LocalizationCategory)))
        {
            categoryText[count] = category.ToString();
            count++;
        }
        currentDisplayCategory = (LocalizationCategory)GUILayout.SelectionGrid((int)currentDisplayCategory, categoryText, 2);

        // #2 Update category list
        int categoryCount = System.Enum.GetValues(typeof(LocalizationCategory)).Length;
        int newCategoryCount = categoryCount - categoryAssetList.categories.Count;
        for (int i = 0; i < newCategoryCount; i++)
        {
            categoryAssetList.categories.Add(new CategoryWrapper());
        }

        // #3 Select category database
        CategoryWrapper categoryWrapper = categoryAssetList.categories[(int)currentDisplayCategory];
        List<TextAsset> assetList = categoryWrapper.textList;


        //----------------------------------------------------------------------------
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //----------------------------------------------------------------------------

        #region add/remove textFiles
        GUILayout.BeginHorizontal();
        int newCount = Mathf.Max(0, EditorGUILayout.IntField("size", assetList.Count));
        while (newCount < assetList.Count)
            assetList.RemoveAt(assetList.Count - 1);
        while (newCount > assetList.Count)
            assetList.Add(null);
        
        if (GUILayout.Button("+"))
        {
            assetList.Add(null);
        }
        if (GUILayout.Button("-"))
        {
            assetList.RemoveAt(assetList.Count - 1);
        }
        GUILayout.EndHorizontal();
        #endregion

        for (int i = 0; i < assetList.Count; i++)
        {
            assetList[i] = (TextAsset)EditorGUILayout.ObjectField(assetList[i], typeof(TextAsset),false);
        }


        if (GUI.changed)
        {

            for (int i = 0; i < assetList.Count; i++)
            {
                if(assetList[i] == null)
                { Debug.LogError("Null text file reference!!"); }
            }

                serializedObject.Update();
            EditorUtility.SetDirty(myTarget);
            AssetDatabase.SaveAssets();
            serializedObject.ApplyModifiedProperties();
            int debug = 0;
        }
    }

}