using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using SPEUI;
[CustomEditor(typeof(SPEUIBase),true)]
public class SPEUIBaseInspector : Editor
{
    Font font = null;
    bool showData = false;
    bool showPosition = false;
    bool showMargin = false;
    public override void OnInspectorGUI()
    {
        if (font == null) font = Resources.Load("EditorFont") as Font;
 
        SPEUIBase myTarget = (SPEUIBase)target;

        showData = EditorGUILayout.Foldout(showData, "自定义数据:");
        if (showData)
        {
            myTarget.customIntData = EditorGUILayout.IntField("数据存储(Int):", myTarget.customIntData);
            myTarget.customStrData = EditorGUILayout.TextField("数据存储(String):", myTarget.customStrData);
        }



        showPosition = EditorGUILayout.Foldout(showPosition, "对齐调整:");
        if (showPosition)
        {
            
            GUI.skin.font = font;
            string[] horizontalAlignStrs = { "居左", "居中", "居右" };
            string[] verticalAlignStrs = { "居上", "居中", "居下" };
            string[] extensionStrs = { "无拉伸", "等比宽拉伸", "等比高拉伸" };

            myTarget.horizontalAlign = (SPEUIBase.HorizontalAlign)GUILayout.SelectionGrid((int)myTarget.horizontalAlign, horizontalAlignStrs, 3);
            myTarget.verticalAlign = (SPEUIBase.VerticalAlign)GUILayout.SelectionGrid((int)myTarget.verticalAlign, verticalAlignStrs, 3);
            myTarget.extension = (SPEUIBase.Extension)GUILayout.SelectionGrid((int)myTarget.extension, extensionStrs, 3);

            GUI.skin.font = null;
        }

        showMargin = EditorGUILayout.Foldout(showMargin, "边距调整:");
        if (showMargin)
        {
 
            myTarget.margin.left = EditorGUILayout.FloatField("左边距", myTarget.margin.left);
            myTarget.margin.right = EditorGUILayout.FloatField("右边距", myTarget.margin.right);
            myTarget.margin.top = EditorGUILayout.FloatField("上边距", myTarget.margin.top);
            myTarget.margin.bottom = EditorGUILayout.FloatField("下边距", myTarget.margin.bottom);

        }

    }
}

[CustomEditor(typeof(SPEUIButton), true)]
public class SPEUIButtonInspector : SPEUIBaseInspector
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SPEUIButton myTarget = (SPEUIButton)target;

        myTarget.onClickEvent = EditorGUILayout.TextField("点击事件", myTarget.onClickEvent);
    }
}
[CustomEditor(typeof(SPEUIDraggable), true)]
public class SPEUIDraggableInspector : SPEUIBaseInspector
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SPEUIDraggable myTarget = (SPEUIDraggable)target;

        myTarget.freeDragAndDrop = EditorGUILayout.Toggle("自由拖拽", myTarget.freeDragAndDrop);
    }
}
[CustomEditor(typeof(SPEUISlot), true)]
public class SPEUISlotInspector : SPEUIBaseInspector
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SPEUISlot myTarget = (SPEUISlot)target;

        myTarget.onDropEventName = EditorGUILayout.TextField("拖放事件", myTarget.onDropEventName);
    }
}