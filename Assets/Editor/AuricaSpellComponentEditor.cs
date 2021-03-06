﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AuricaSpellComponent))]
[CanEditMultipleObjects]
public class AuricaSpellComponentEditor : Editor 
{
    AuricaSpellComponent component;
    bool importstr;
    string importString;
    
    void OnEnable()
    {
        component = (AuricaSpellComponent)target;
        // if (component.basicDistribution.Count == 0) component.basicDistribution = new List<float>(new float[]{0f, 0f, 0f, 0f, 0f, 0f, 0f});
    }

    public override void OnInspectorGUI()
    {
        component.c_name = EditorGUILayout.TextField("Name", component.c_name);

        EditorGUILayout.LabelField("Description");
        GUIStyle myCustomStyle = new GUIStyle(GUI.skin.GetStyle("textArea")){ wordWrap = true };
        component.description = EditorGUILayout.TextArea(component.description, myCustomStyle);

        // component.hasBasicDistribution = EditorGUILayout.Toggle("Basic distribution", component.hasBasicDistribution);
        // if (component.hasBasicDistribution) {
        //     Rect r = EditorGUILayout.BeginVertical("Basic Distribution");
        //     component.basicDistribution[0] = EditorGUILayout.DelayedFloatField("Structure", component.basicDistribution[0]);
        //     component.basicDistribution[1] = EditorGUILayout.DelayedFloatField("Essence", component.basicDistribution[1]);
        //     component.basicDistribution[2] = EditorGUILayout.DelayedFloatField("Fire", component.basicDistribution[2]);
        //     component.basicDistribution[3] = EditorGUILayout.DelayedFloatField("Water", component.basicDistribution[3]);
        //     component.basicDistribution[4] = EditorGUILayout.DelayedFloatField("Earth", component.basicDistribution[4]);
        //     component.basicDistribution[5] = EditorGUILayout.DelayedFloatField("Air", component.basicDistribution[5]);
        //     component.basicDistribution[6] = EditorGUILayout.DelayedFloatField("Nature/Alignment", component.basicDistribution[6]);
        //     EditorGUILayout.EndVertical();
            
        //     EditorGUILayout.Space();
        //     EditorGUILayout.Space();
        // }

        component.hasBasicDistribution = EditorGUILayout.Toggle("Basic distribution", component.hasBasicDistribution);
        if (component.hasBasicDistribution) {
            Rect r = EditorGUILayout.BeginVertical("Basic Distribution");
            component.basicDistribution.structure = EditorGUILayout.DelayedFloatField("Structure", component.basicDistribution.structure);
            component.basicDistribution.essence = EditorGUILayout.DelayedFloatField("Essence", component.basicDistribution.essence);
            component.basicDistribution.fire = EditorGUILayout.DelayedFloatField("Fire", component.basicDistribution.fire);
            component.basicDistribution.water = EditorGUILayout.DelayedFloatField("Water", component.basicDistribution.water);
            component.basicDistribution.earth = EditorGUILayout.DelayedFloatField("Earth", component.basicDistribution.earth);
            component.basicDistribution.air = EditorGUILayout.DelayedFloatField("Air", component.basicDistribution.air);
            component.basicDistribution.nature = EditorGUILayout.DelayedFloatField("Nature", component.basicDistribution.nature);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        component.hasAuricDistribution = EditorGUILayout.Toggle("Auric distribution", component.hasAuricDistribution);
        if (component.hasAuricDistribution) {
            Rect r = EditorGUILayout.BeginVertical("Auric Distribution");
            component.auricDistribution.structure = EditorGUILayout.DelayedFloatField("Structure", component.auricDistribution.structure);
            component.auricDistribution.essence = EditorGUILayout.DelayedFloatField("Essence", component.auricDistribution.essence);
            component.auricDistribution.fire = EditorGUILayout.DelayedFloatField("Fire", component.auricDistribution.fire);
            component.auricDistribution.water = EditorGUILayout.DelayedFloatField("Water", component.auricDistribution.water);
            component.auricDistribution.earth = EditorGUILayout.DelayedFloatField("Earth", component.auricDistribution.earth);
            component.auricDistribution.air = EditorGUILayout.DelayedFloatField("Air", component.auricDistribution.air);
            component.auricDistribution.nature = EditorGUILayout.DelayedFloatField("Nature", component.auricDistribution.nature);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        component.hasFluxDistribution = EditorGUILayout.Toggle("Flux distribution", component.hasFluxDistribution);
        if (component.hasFluxDistribution) {
            Rect r = EditorGUILayout.BeginVertical("Flux Distribution");
            component.fluxDistribution.structure = EditorGUILayout.DelayedFloatField("Structure", component.fluxDistribution.structure);
            component.fluxDistribution.essence = EditorGUILayout.DelayedFloatField("Essence", component.fluxDistribution.essence);
            component.fluxDistribution.fire = EditorGUILayout.DelayedFloatField("Fire", component.fluxDistribution.fire);
            component.fluxDistribution.water = EditorGUILayout.DelayedFloatField("Water", component.fluxDistribution.water);
            component.fluxDistribution.earth = EditorGUILayout.DelayedFloatField("Earth", component.fluxDistribution.earth);
            component.fluxDistribution.air = EditorGUILayout.DelayedFloatField("Air", component.fluxDistribution.air);
            component.fluxDistribution.nature = EditorGUILayout.DelayedFloatField("Nature", component.fluxDistribution.nature);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        importstr = EditorGUILayout.Toggle("String Import", importstr);
        if (importstr) {
            importString = EditorGUILayout.TextField("String: ", importString);
            if (GUILayout.Button("Import")) {
                string[] stringSeperator = new string[] {"\", "};
                string[] splitStr = importString.Split(stringSeperator, System.StringSplitOptions.None);
                splitStr[0] = splitStr[0].Replace("\"", "").Replace("(", "");
                splitStr[1] = splitStr[1].Replace("\"", "");
                splitStr[2] = splitStr[2].Replace("\"", "").Replace(")", "");

                component.c_name = splitStr[0];
                Debug.Log("Name: "+component.c_name);
                component.description = splitStr[1];
                Debug.Log("Description: "+component.description);

                splitStr[2] = splitStr[2].Substring(1, splitStr[2].Length - 2);
                string[] distributionSeperator = new string[] {"], ["};
                string[] splitDistributions = splitStr[2].Split(distributionSeperator, System.StringSplitOptions.None);
                int iter = 0;
                component.hasBasicDistribution = false;
                component.hasAuricDistribution = false;
                component.hasFluxDistribution = false;
                foreach (var item in splitDistributions) {
                    if (iter == 0) {
                        component.hasBasicDistribution = true;
                        component.basicDistribution = new ManaDistribution(item);
                        Debug.Log("Basic dist: "+component.basicDistribution.ToString());
                    } else if (iter == 1) {
                        component.hasAuricDistribution = true;
                        component.auricDistribution = new ManaDistribution(item);
                        Debug.Log("Auric dist: "+component.auricDistribution.ToString());
                    } else if (iter == 2) {
                        component.hasFluxDistribution = true;
                        component.fluxDistribution = new ManaDistribution(item);
                        Debug.Log("Flux dist: "+component.fluxDistribution.ToString());
                    } 
                    iter += 1;
                }
            }
            Undo.RecordObject(target, "Import values");
        }
        EditorUtility.SetDirty(target);
    }
}