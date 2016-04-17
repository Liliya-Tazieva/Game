using System.Collections;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public Texture2D Map;
    public TilesManager TilesM;

    // Use this for initialization
    private void Start() {
        
    }

    IEnumerator InitializeMap() {
        for (var i = 0; i < Map.height; ++i) {
            for (var j = 0; j < Map.width; ++j) {
                var color = Map.GetPixel(i, j);
                var prefab = TilesM.GetPrefab(color);
                if (prefab != null) {
                    var position = new Vector3(i * 3.0f, 0.0f, j * 3.0f);
                    var temp = Instantiate(prefab, position, Quaternion.identity) as GameObject;
                    if (temp != null) {
                        temp.transform.parent = gameObject.transform;
                    }
                }
                if (j % 15 == 0) yield return null;
            }
        }
    }

    void Update() {
        if (Input.GetKeyDown("m")) {
            StartCoroutine("InitializeMap");
        }
    }
}