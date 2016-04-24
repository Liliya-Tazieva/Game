using UnityEngine;

public class Informer : MonoBehaviour {
    public bool IsObstacle;
    // Use this for initialization
    public void Start() {
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