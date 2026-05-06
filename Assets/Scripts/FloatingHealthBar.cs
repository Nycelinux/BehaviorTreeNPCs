using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Camera cam;
    private Transform target;
    private Vector3 offset;

    private void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.Log("No Target");
            return;
        }
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (slider == null) return;
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = currentValue / maxValue;
    }

    public void SetTarget(Transform transform, Vector3 off)
    {
        target = transform;
        offset = off;
    }
}
