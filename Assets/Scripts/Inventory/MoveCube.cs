using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cc_inventory
{
    public class MoveCube : MonoBehaviour
    {
        public float speed = 10.0f;
        public Rigidbody rb;
        public Vector3 movement;

        // Use this for initialization
        void Start()
        {
            rb = this.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }


        void FixedUpdate()
        {
            moveCharacter(movement);
        }
        void moveCharacter(Vector3 direction)
        {
            rb.velocity = direction * speed;
        }
    }
}