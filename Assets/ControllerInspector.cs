using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Controller))]
public class ControllerInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
         if(GUILayout.Button("From Neibours")) {
             var controller = (Controller)target;
             controller.GetNearest();
         }
    }

    public void OnSceneGUI() {
        var controller = (Controller)target;
        if (controller == null) {
            return;
        }

        Handles.color = Color.red;
        Handles.ConeCap(0,
                controller.From,
                controller.transform.rotation,
                1);
        Handles.color = Color.blue;
        Handles.ConeCap(0,
                controller.To,
                controller.transform.rotation,
                1);

    }
}