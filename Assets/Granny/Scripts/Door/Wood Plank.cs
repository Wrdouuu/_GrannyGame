using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPlank : MonoBehaviour
{
    public float fallForce = 2f;
    private bool isFalling = false;
    private static int fallenPlankCount = 0;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(isFalling)
        {
            Rigidbody rb =gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * fallForce, ForceMode.Impulse);
            isFalling = false;

            fallenPlankCount++;
        }
        if (fallenPlankCount >= 2)
        {
            //Cut scene.
            Debug.Log("Door unlocked fully");
            GameManager.instance.allLocksOpen = true;
        }
    }

    public void MakeFall()
    {
        isFalling=true;
        rb.isKinematic = false;
    }
}
