using UnityEngine;
public enum NodeState {
    Undiscovered = 2,
    Discovered = 0,
    Processed = 1,
};

public class Informer : MonoBehaviour {
    public bool IsObstacle;
    public float Distance;
    public NodeState Visited;
    // Use this for initialization
    public void Start() {
        Distance = 0;
        Visited = (NodeState) 2;
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