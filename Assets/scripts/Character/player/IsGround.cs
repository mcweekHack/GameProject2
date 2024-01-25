using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGround : MonoBehaviour
{
    [SerializeField] bool IfOnGround;
    Collider2D col;
    void Start()
    {
        col = GetComponent<Collider2D>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("platform"))
            IfOnGround = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        IfOnGround = false;
    }
    public bool IsOnGround()
    {
        return IfOnGround;
    }
    
}
