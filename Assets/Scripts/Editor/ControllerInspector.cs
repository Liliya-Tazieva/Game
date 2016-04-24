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
            var nodes = new List<Informer>();
            foreach (var kdTreeNode in controller.NodesTree) {
                nodes.Add(kdTreeNode.Value);
            }
            var from = nodes.Find(x => x.transform.position == controller.From);
            var to = nodes.Find(x => x.transform.position == controller.To);
            DebugInformationA_Star debugInformation;
            controller.A_star(from, to, controller.Radius, true, out debugInformation);
            controller.InitializeDebugInfo();
            controller.DebugManagerAStar.AddPath(debugInformation);
        }

        if (GUILayout.Button("Test")) {
            var controller = (Controller) target;
            var nodes = new List<Informer>();
            Debug.Log("0wer");

            foreach (var kdTreeNode in controller.NodesTree) {
                nodes.Add(kdTreeNode.Value);
            }
            Debug.Log("1wer");

            nodes = nodes.Where(arg => arg.IsObstacle != true).ToList();
            Debug.Log(string.Format("2wer" + " {0}", nodes.Count));

            var rnd = new Random();
            int index1 = rnd.Next(0, nodes.Count - 1),
                index2 = rnd.Next(0, nodes.Count - 1);

            Debug.Log("From " + nodes[index1].transform.position);
            Debug.Log("To " + nodes[index2].transform.position);

            DebugInformationA_Star debugInformation;
            controller.A_star(nodes[index1], nodes[index2], controller.Radius, true, out debugInformation);
            controller.InitializeDebugInfo();
            controller.DebugManagerAStar.AddPath(debugInformation);
        }
    }
}