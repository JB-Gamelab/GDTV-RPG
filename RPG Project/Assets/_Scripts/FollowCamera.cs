using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] private Transform targetTransform;


    private void Update() {
        transform.position = targetTransform.position;
    }
}
