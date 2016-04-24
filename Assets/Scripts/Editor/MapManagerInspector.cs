using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
public class MapManagerInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Redraw")) {
            var mapManager = target as MapManager;
            /*var it = mapManager.InitializeMap();
            it.MoveNext();*/
        }

        if (GUILayout.Button("Clear")) {
            var mapManager = target as MapManager;
            mapManager.transform
                .Cast<Transform>()
                .Select(t=>t.gameObject)
                .ToList().ForEach(DestroyImmediate);
        }
    }
}