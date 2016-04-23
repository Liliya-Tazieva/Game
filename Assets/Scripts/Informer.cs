using UnityEngine;

public class Informer : MonoBehaviour {
    public float Distance;
    public bool IsObstacle;
    // Use this for initialization
    public void Start() {
        Distance = 0;
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