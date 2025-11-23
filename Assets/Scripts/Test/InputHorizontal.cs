using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHorizontal : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private WizardMovement movement;

    void Update()
    {
        if (movement.XAxis == 1)
        {
            images[1].color = Color.red;
            images[0].color = Color.white;
        }
        else if (movement.XAxis == -1)
        {
            images[0].color = Color.red;
            images[1].color = Color.white;
        }
        else
        {
            images[0].color = Color.white;
            images[1].color = Color.white;
        }
    }
}
