using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

[CustomEditor(typeof (Controller))]
public class ControllerInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("A*")) {
            var controller = (Controller) target;
            controller.A_star(controller.From, controller.To, controller.Radius, false, true);
        }

        if (GUILayout.Button("Find neighbours")) {
            var controller = (Controller) target;
            controller.GetNearest();
        }

        if (GUILayout.Button("Test")) {
            var controller = (Controller) target;
            var nodes = new List<Informer>();
            foreach (var kdTreeNode in controller.Tree) {
                nodes.Add(kdTreeNode.Value);
            }
            nodes = nodes.Where(arg => arg.IsObstacle != true).ToList();
            var rnd = new Random();
            int index1 = rnd.Next(0, nodes.Count - 1),
                index2 = rnd.Next(0, nodes.Count - 1);
            Debug.Log("From " + nodes[index1].transform.position);
            Debug.Log("To " + nodes[index2].transform.position);
            controller.A_star(nodes[index1], nodes[index2], 5, true, true);
        }
    }
}