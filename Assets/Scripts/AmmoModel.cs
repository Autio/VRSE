using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class AmmoModel 
{
    // Properties of different ammunition types
    public string name { get; protected set; }
    public string desc { get; protected set; }
    public GameObject ammoPrefab { get; protected set; }
    public int damage { get; protected set; }
    public float explosionRadius { get; protected set; }
    public int cost { get; protected set; } // How many resources does it take to use a shot of this ammo? 

    public AmmoModel(string name, string desc = "", GameObject ammoPrefab = null, int damage = 10, float explosionRadius = 20f, int cost = 10)
    {
        this.name = name;
        this.desc = desc;
        this.ammoPrefab = ammoPrefab;

        this.damage = damage;
        this.cost = cost;
    }

}
