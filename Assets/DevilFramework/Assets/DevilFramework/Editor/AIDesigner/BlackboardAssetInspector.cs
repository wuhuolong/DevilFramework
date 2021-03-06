﻿using Devil.AI;
using Devil.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevilEditor
{
    [CustomEditor(typeof(BlackboardAsset))]
    public class BlackboardAssetInspector : Editor
    {
        static bool drop = false;
        static Vector2 scroll;
        static string newName = "newVariable";
        static string newType;
        static int dropIndex;
        static long repeatTip;

        public static void OnBlackboardInspectorGUI(BlackboardAsset asset)
        {
            drop = QuickGUI.DrawHeader("Blackboard", "BT", false);
            if (drop)
            {
                QuickGUI.BeginContents(200);
                scroll = GUILayout.BeginScrollView(scroll, GUILayout.MaxHeight(200));
                for (int i = 0; i < asset.m_Properties.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    if( GUILayout.Button("", "OL Minus", GUILayout.Width(20)))
                    {
                        BlackboardAsset.KeyValue[] keys = new BlackboardAsset.KeyValue[asset.m_Properties.Length - 1];
                        if (i > 0)
                            System.Array.Copy(asset.m_Properties, 0, keys, 0, i);
                        if (i < asset.m_Properties.Length - 1)
                            System.Array.Copy(asset.m_Properties, i + 1, keys, i, keys.Length - i);
                        asset.m_Properties = keys;
                        EditorUtility.SetDirty(asset);
                        break;
                    }
                    BlackboardAsset.KeyValue key = asset.m_Properties[i];
                    GUILayout.Label(key.m_Key, GUILayout.Width(140));
                    GUILayout.Label(key.m_Value, "textfield");
                    EditorGUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
                GUILayout.Space(10);

                if (JDateTime.NowMillies - repeatTip < 2000)
                {
                    GUILayout.Label("已经存在相同的属性", "flow overlay box");
                }
                EditorGUILayout.BeginHorizontal();
                dropIndex = Mathf.Min(dropIndex, BehaviourModuleManager.GetOrNewInstance().SharedTypeNames.Length - 1);
                if (GUILayout.Button("","OL Plus", GUILayout.Width(20)))
                {
                    if (string.IsNullOrEmpty(newName) || asset.HasKey(newName))
                    {
                        repeatTip = JDateTime.NowMillies;
                    }
                    else
                    {
                        BlackboardAsset.KeyValue[] keys = new BlackboardAsset.KeyValue[asset.m_Properties.Length + 1];
                        if (keys.Length > 1)
                            System.Array.Copy(asset.m_Properties, keys, keys.Length - 1);
                        BlackboardAsset.KeyValue newk = new BlackboardAsset.KeyValue();
                        newk.m_Key = newName;
                        newk.m_Value = BehaviourModuleManager.GetOrNewInstance().SharedTypeNames[dropIndex];
                        keys[keys.Length - 1] = newk;
                        asset.m_Properties = keys;
                        EditorUtility.SetDirty(asset);
                        newName = "newVariable";
                        scroll = Vector2.up * asset.m_Properties.Length * 30;
                    }
                }
                newName = GUILayout.TextField(newName ?? "newVariable", GUILayout.Width(140));
                dropIndex = EditorGUILayout.Popup(dropIndex, BehaviourModuleManager.GetOrNewInstance().SharedTypeNames);
                EditorGUILayout.EndHorizontal();
                QuickGUI.EndContents();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            OnBlackboardInspectorGUI(target as BlackboardAsset);
        }
    }
}