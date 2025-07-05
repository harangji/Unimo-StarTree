using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BBITEditor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(FakeDirectionalLight))]
    public class FakeDirLightButton : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            FakeDirectionalLight fakeLight = (FakeDirectionalLight)target;
            if (GUILayout.Button("Normalize Vector", GUILayout.Width(120), GUILayout.Height(20))) 
            { fakeLight.NormalizeLightVector(); }

            GUILayout.FlexibleSpace(); 
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}

