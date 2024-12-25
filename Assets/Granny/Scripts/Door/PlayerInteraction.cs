using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public Camera playerCamera;
    public GameObject player;

    void Update()
    {
        OnHoldingAxe();
        
    }

    bool IsHoldingAxe()
    {
        foreach (Transform child in player.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Axe"))
            {
                return true;
            }
        }
        return false;
    }

    void OnHoldingAxe()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
            {
                WoodPlank woodPlank = hit.collider.GetComponent<WoodPlank>();
                if (woodPlank != null)
                {
                    if (IsHoldingAxe())
                    {
                        woodPlank.MakeFall();
                    }
                    else
                    {
                        Debug.Log("You need an axe to take the planks down.");
                    }
                }
            }

        }
    }
}

