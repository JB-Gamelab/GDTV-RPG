using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;
using RPG.Combat;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {

        private void Update() {

            //If combat target, ignore skip movement logic
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat() {

            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            //Check if raycast hits a combat target
            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue; //if not a combat target, skip following logic and check next array entry

                if (Input.GetMouseButtonDown(0)) {
                    GetComponent<Fighter>().Attack(target);
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
                    GetComponent<Mover>().StartMoveAction(hit.point);
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
