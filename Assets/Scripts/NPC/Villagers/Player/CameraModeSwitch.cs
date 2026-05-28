using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeSwitch : MonoBehaviour
{
    public Transform cameraTransform;

    public Vector3 firstPersonOffset = new Vector3(0f, 1.6f, 0f);
    public Vector3 thirdPersonOffset = new Vector3(0f, 1.6f, -3f);

    private bool isFirstPerson = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
        }

        cameraTransform.localPosition = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
    }
}
