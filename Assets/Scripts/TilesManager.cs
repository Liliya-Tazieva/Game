using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour {
    public List<Color> Colors;

    public List<GameObject> Prefabs;

    private void Start() {
        Debug.Log("Initialize TilesManager");
    }

    public GameObject GetPrefab(Color color) {
        var index = -1;
        index = Colors.IndexOf(color);
        if (index == -1) {
            index = 0;
        }
        return Prefabs[index];
    }
}