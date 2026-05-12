using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 10f;

    void Update()
    {
        if (DialogueUI.instance != null &&
        DialogueUI.instance.dialoguePanel.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(0.5f,0.5f,0f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Debug.Log("Getroffen: " + hit.collider.name);

            IInteractable interactable =hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log("Interactable gefunden");
                interactable.Interact();
            }
            else
            {
                Debug.Log("Kein interactable auf Objekt");
            }
        }
    }
}
