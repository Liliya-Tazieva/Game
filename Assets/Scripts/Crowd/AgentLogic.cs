using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PathFinding;
using JetBrains.Annotations;
using Random = System.Random;
using UnityEngine;

namespace Assets.Scripts.Agents {
    public class AgentLogic : MonoBehaviour {
        public CrowdManager CrowdM;
        public Rigidbody Rigidbody;
        public Collider Collider;
        public Informer From;
        public Informer To;
        public float Speed;
        public float Height;
        public bool StopFlag;
        private bool _flag;
        [UsedImplicitly] public float HealthPoints;

        [UsedImplicitly]
        private void Start() {
            StopFlag = false;
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.isKinematic = true;
            Collider = GetComponent<Collider>();
            Collider.isTrigger = true;
            _flag = true;
        }

        public IEnumerator Go(List<Informer> path) {
                var position = From.transform.position;
                //var downRay = new Ray(transform.position, -Vector3.up);
                for (int i = 1; i < path.Count; ++i) {
                    while (position != path[i].transform.position) {
                        position = Vector3.MoveTowards(position, path[i].transform.position, Speed);
                        /*RaycastHit hit;
                        if (Physics.Raycast(downRay, out hit, Height*2)) {
                            Debug.Log("hit.distance " + hit.distance);
                            if (Math.Abs(Height - hit.distance) > 0.01) {
                                GetComponent<Transform>().Translate(0.0f, Height - hit.distance, 0.0f);
                                Debug.Log("Position y " + transform.position.y);
                            }*/
                            GetComponent<Transform>().Translate(position.x - transform.position.x,
                                0.0f, position.z - transform.position.z);
                            yield return null;
                        //}
                    }
                    From = path[i];
                }
            if (From == To) {
            }
        }

        void OnTriggerEnter(Collider other) {
            if (other.isTrigger) {
                CrowdM.InstantiateAgentsCollision();
            } 
        }

        [UsedImplicitly]
        private void Update() {
            if (!StopFlag) {
                if (From == To) {
                    _flag = true;
                    To = CrowdM.GetRandomInformer();
                } else {
                    var path = CrowdM.FindPath(From, To);
                    if (path != null && _flag) {
                        _flag = false;
                        StartCoroutine(Go(path));
                    } else if (path == null) {
                        Destroy(transform.gameObject);
                    }
                }
            }
        }

    }
}