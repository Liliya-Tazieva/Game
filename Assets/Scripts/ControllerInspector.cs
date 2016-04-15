using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Controller))]
public class ControllerInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

         if(GUILayout.Button("A*")) {
             var controller = (Controller)target;
             controller.A_star();
         }

         if (GUILayout.Button("Find neighbours")) {
             var controller = (Controller)target;
             controller.GetNearest();
         }
    }

    public void Show_A_Star(List<Informer> path) {
        foreach (var node in path) {
            Handles.color = Color.yellow;
            Handles.CircleCap(0,
                    node.transform.position,
                    node.transform.rotation,
                    1);
        }
    }

    public void OnSceneGUI() {
        var controller = (Controller)target;
        if (controller == null) {
            return;
        }

        Handles.color = Color.red;
        Handles.ConeCap(0,
                controller.From.transform.position,
                controller.transform.rotation,
                1);
        Handles.color = Color.blue;
        Handles.ConeCap(0,
                controller.To.transform.position,
                controller.transform.rotation,
                1);

    }
}