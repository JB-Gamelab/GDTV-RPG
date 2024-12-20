using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {


        private Health health;

        private void Start() {
            health = GetComponent<Health>();
        }

        private void Update() {
            if (health.IsDead()) return; //If player dead stop movement and combat actions
            //If combat target, ignore skip movement logic
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat() {

            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            //Check if raycast hits a combat target
            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0)) {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                return true;
            }
            return false;
        }

        private bool InteractWithMovement() {

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit) {
                if (Input.GetMouseButton(0)) {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        //Raycast from camera to mouse cursor on Navmesh
        private static Ray GetMouseRay() {

            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
