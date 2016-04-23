using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TilesManager : MonoBehaviour {
    public List<Color> Colors;

    public List<GameObject> Prefabs;

    public GameObject GetPrefab(Color color) {
        var index = -1;
        index = Colors.IndexOf(color);
        if (index == -1) {
            return null;
        }
        return Prefabs[index];
    }
}