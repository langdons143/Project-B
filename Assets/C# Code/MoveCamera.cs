// Sarah Hannah Langdon
// 2026-04-10
// Advanced Unity Programming
// https://www.youtube.com/watch?v=f473C43s8nE FIRST PERSON MOVEMENT in 10 MINUTES - Unity Tutorial by Dave

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
