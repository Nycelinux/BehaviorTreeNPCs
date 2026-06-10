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
        //Ray ray = playerCamera.ScreenPointToRay(new Vector3(0.5f,0.5f,0f));
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);

        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red, 2f);

        RaycastHit hit;

        bool success = Physics.Raycast(ray, out hit, interactionDistance);

        // Falls nichts getroffen  Streuung
        if (!success)
        {
            float spread = 0.02f;

            for (int i = 0; i < 6; i++)
            {
                Vector3 randomDir = ray.direction + new Vector3(
                    Random.Range(-spread, spread),
                    Random.Range(-spread, spread),
                    Random.Range(-spread, spread)
                );

                if (Physics.Raycast(ray.origin, randomDir.normalized, out hit, interactionDistance))
                {
                    success = true;
                    break;
                }
            }
        }

        if (success)
        {
            Debug.Log("=== RAYCAST TREFFER ===");
            Debug.Log("Getroffen: " + hit.collider.name);

            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(hit.point);
            }
            else
            {
                Debug.Log("Kein Interactable gefunden");
            }
        }
        else
        {
            Debug.Log("Raycast hat nichts getroffen!");
        }
    }
}