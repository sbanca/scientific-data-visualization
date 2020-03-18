using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableTop {
    public class sendCollisionUpwards : MonoBehaviour
    {
        public ColliderType type;
        BoxCollider col;

        void Start()
        {
            col = gameObject.GetComponent<BoxCollider>();
        }

        private void OnTriggerStay(Collider other)
        {
            // TODO we can meke it so that it happens only if is the hand

            gameObject.SendMessageUpwards("Trigger",type);

        }

      
    }
}