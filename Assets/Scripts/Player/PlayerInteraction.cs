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
        Debug.DrawRay(    playerCamera.transform.position,    playerCamera.transform.forward * 20f,   Color.green);
    }


    void Interact()
    {
        Debug.Log("Kamera Pos: " + playerCamera.transform.position);
        Debug.Log("Kamera Forward: " + playerCamera.transform.forward);
        Vector3 screenPoint = new Vector3(    Screen.width * 0.5f,   Screen.height * 0.1f,    0f);
        Ray ray;

        if (playerCamera != null)
        {
            Vector3 screenCenter = new Vector3(
                Screen.width * 0.5f,
                Screen.height * 0.5f,
                0f);

            ray = playerCamera.ScreenPointToRay(screenCenter);
        }
        else
        {
            ray = new Ray(
                transform.position + Vector3.up * 1.5f,
                transform.forward);
        }

        Debug.DrawRay(
            ray.origin,
            ray.direction * interactionDistance,
            Color.red,
            2f);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            Debug.Log("Raycast hat nichts getroffen!");
            return;
        }

        Debug.Log("Getroffen: " + hit.collider.name);

        IInteractable interactable =
            hit.collider.GetComponent<IInteractable>() ??
            hit.collider.GetComponentInParent<IInteractable>() ??
            hit.collider.transform.root.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact(hit.point);
        }
        else
        {
            Debug.Log("Kein Interactable gefunden");
        }
    }
}