using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    public TextAsset MapImg;
    public TilesManager TilesM;

	// Use this for initialization
	void Start () {
        Texture2D map = new Texture2D(2, 2);
	    if (!map.LoadImage(MapImg.bytes)) Debug.Log("Can't load image");
	    else {
	        for(int i = 0; i<map.height; ++i)
	            for (int j = 0; j < map.width; ++j) {
	                Color color = map.GetPixel(i, j);
                    GameObject prefab = TilesM.GetPrefab(color);
                    Vector3 position = new Vector3(i * 3.0f, 0.0f, j * 3.0f);
                    Instantiate(prefab, position, Quaternion.identity);
	            }
	    }
	}
	
}
