using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractType GetInteractType();
    void Interact(Vector3 hitPoint);

}
