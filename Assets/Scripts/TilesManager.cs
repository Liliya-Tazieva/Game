using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour {
    public List<Color> Colors;

    public List<GameObject> Prefabs;

    private void Start() {
    }

    public GameObject GetPrefab(Color color) {
        var index = -1;
        index = Colors.IndexOf(color);
        if (index == -1) {
            return null;
        }
        return Prefabs[index];
    }
}