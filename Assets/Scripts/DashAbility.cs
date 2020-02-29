using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BraidTimeWalkClone.Ability
{
    // Inherited from Ability to use cast function
    public class DashAbility : Ability
    {
        // Setting fields for ability
        [SerializeField]
        private float dashForce = 50f;
        [SerializeField]
        private float dashDuration = 0.2f;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Cast());
            }
        }

        // Overriding the cast method from Ability
        public override IEnumerator Cast()
        {
            // Adding dash to the forward direction
            rb.AddForce(transform.forward * dashForce, ForceMode.VelocityChange);

            // Wait for some seconds to stop the force
            yield return new WaitForSeconds(dashDuration);

            rb.velocity = Vector3.zero;
        }
    }
}

