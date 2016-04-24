using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Agents {
    public class NavigationSphere : MonoBehaviour {
        [UsedImplicitly]
        public Collider Collider;

        [UsedImplicitly]
        public Vector3 From;

        [UsedImplicitly]
        public float Height;

        [UsedImplicitly]
        public Vector3 To;

        [UsedImplicitly]
        private void Start() {
            Collider = GetComponent<Collider>();
        }

        [UsedImplicitly]
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
}