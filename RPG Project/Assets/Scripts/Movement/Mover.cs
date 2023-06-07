using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour {

        private const string FORWARD_SPEED = "forwardSpeed";

        private NavMeshAgent navMeshAgent;

        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination) {
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }


        public void MoveTo(Vector3 destination) {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void Stop() {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator() {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat(FORWARD_SPEED, speed);
        }
    }
}
