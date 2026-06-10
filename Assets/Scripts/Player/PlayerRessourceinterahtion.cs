using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType
{
    NPC,
    Resource,
    World
}

public class PlayerRessourceinterahtion : MonoBehaviour
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
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);

        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red, 2f);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            Debug.Log("Raycast hat nichts getroffen!");
            return;
        }

        Debug.Log("=== RAYCAST TREFFER === " + hit.collider.name);

        IInteractable interactable = null;

        // 1. direkt am Treffer
        interactable = hit.collider.GetComponent<IInteractable>();

        // 2. nach oben (Bäume)
        if (interactable == null)
        {
            interactable = hit.collider.GetComponentInParent<IInteractable>();
        }

        // 3. notfalls Root (Tree/NPC Prefabs)
        if (interactable == null)
        {
            interactable = hit.collider.transform.root.GetComponent<IInteractable>();
        }

        if (interactable != null && interactable.GetInteractType() == InteractType.Resource)
        {
            interactable.Interact(hit.point);
        }
        else
        {
            Debug.LogWarning("Kein Interactable gefunden auf: " + hit.collider.name);
        }
    }
}