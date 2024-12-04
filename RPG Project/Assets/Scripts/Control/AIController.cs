using RPG.Core;
using RPG.Combat;
using RPG.Attributes;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {
    public class AIController : MonoBehaviour {
        private const string PLAYER = "Player";

        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 3f;
        [Range(0,1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;


        private Fighter fighter;
        private Health health;
        private GameObject player;
        private Mover mover;

        private Vector3 guardPosition;

        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;

        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag(PLAYER);

            guardPosition = transform.position;
        }

        private void Update() {
            if (health.IsDead()) return;
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) {

                AttackBehaviour();
            } else if (timeSinceLastSawPlayer < suspicionTime) {
                SuspicionBehaviour();
            } else {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers() {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null) {
                if (AtWaypoint()) {
                    timeSinceArrivedAtWaypoint = 0f;

                    CycleWaypoint();
                }



                nextPosition = GetCurrentWaypoint();
            }
        
            if (timeSinceArrivedAtWaypoint > waypointDwellTime) {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint() {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint() {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionBehaviour() {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour() {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer() {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
