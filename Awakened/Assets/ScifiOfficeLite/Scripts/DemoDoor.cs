﻿using UnityEngine;

namespace ScifiOffice {
    public class DemoDoor : MonoBehaviour {
        Animator anim;

        private void Start() {
            anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.name == "Player") {
                anim.SetTrigger("Open");
            }
        }
    }
}