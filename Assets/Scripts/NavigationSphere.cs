using System;
using UnityEngine;

public class NavigationSphere : MonoBehaviour {
    public Informer From;
    public float Height;
    public Collider Collider;
    public Informer To;

    // Use this for initialization
    private void Start() {
        Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    private void Update() {
        RaycastHit hit;
        var downRay = new Ray(transform.position, -Vector3.up);

        if (Physics.Raycast(downRay, out hit, Height*2)) {
            if (Math.Abs(Height - hit.distance) > 0.01) {
                GetComponent<Transform>().Translate(0.0f, Height - hit.distance, 0.0f);
            }
            GetComponent<Transform>().Translate(0.1f, 0.0f, 0.0f);
        }
    }
}