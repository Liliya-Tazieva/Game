using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CrowdManager))]
public class CrowdManagerInspector: UnityEditor.Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Instantiate agents")) {
            var crowdManager = target as CrowdManager;

            if (crowdManager != null) {
                crowdManager.InstantiateAgents();
            }
        }
    }
}