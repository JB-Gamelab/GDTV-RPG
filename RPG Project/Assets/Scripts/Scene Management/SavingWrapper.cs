using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System.Runtime.CompilerServices;

namespace RPG.SceneManagement {
    public class SavingWrapper : MonoBehaviour {

        private const string defaultSaveFile = "save";

        [SerializeField] float fadeInTime = 0.2f;

        IEnumerator Start() {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }
        }

        public void Save() {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load() {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}
