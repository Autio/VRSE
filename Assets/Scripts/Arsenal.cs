using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    public static Arsenal instance;

    // Initialize the different ammo types
    public List<AmmoModel> arsenal;
    public GameObject[] ammoPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;    // Ensures there's only one game manager only
        arsenal = new List<AmmoModel>();

        arsenal.Add(new AmmoModel("Normal", "Just your regular, run-of-the-mill potato", ammoPrefabs[0]));
        arsenal.Add(new AmmoModel("Big Potato", "A heavy hitter guaranteed to starch a foe in one go", ammoPrefabs[1], 30, 30));
        arsenal.Add(new AmmoModel("French Fries", "A bag of fries ready to spread flavour over a large area", ammoPrefabs[2], 5, 30));

    }



}
