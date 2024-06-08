using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLifetime : MonoBehaviour
{
    public float lifetime = 5.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
