using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilesManager: MonoBehaviour {

    public List<GameObject> Prefabs;
    public List<Color> Colors;

    void Start() {
        Debug.Log("Initialize TilesManager");
    }

    public GameObject GetPrefab(Color color) {
        int index = Colors.IndexOf(color);
        return Prefabs[index];
    }
}
