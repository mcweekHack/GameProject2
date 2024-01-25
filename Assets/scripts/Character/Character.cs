using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float Health; 
    protected virtual void OnEnable()
    {
        Initilize();
    }
    /// //////////////////////////////////////////////////
    protected virtual void Initilize()
    {
        MaxHealth = 100f;
        Health = 100f;
    }
    public virtual void TakeDamage(float damage){ }
}
