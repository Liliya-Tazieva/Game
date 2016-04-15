using UnityEngine;

public class Informer : MonoBehaviour {
    public bool IsObstacle;
    public float Distance;
    public int Visited;
    // Use this for initialization
    public void Start() {
        Distance = 0;
        Visited = 2;
        var parent = transform.parent;
        while (parent != null) {
            var controller = parent.GetComponent<Controller>();
            if (controller != null) {
                controller.RegisterInformer(this);
                break;
            }
            parent = parent.transform.parent;
        }
    }
}