using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Ammo type

    // These should be set as part of the prefab
    public GameObject explosionPrefab;
    public GameObject explosionFX;
    private GameObject myShooter;

    int damage; // How much damage does a single hit do
    float explosionRadius;

    // TODO:
    // Bounciness? Physics material? Trail?
    // Other than pure damage effects

    public void setExplosion(int damage, float explosionRadius)
    {
        this.damage = damage;
        this.explosionRadius = explosionRadius;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit something!");
        if (collision.transform.tag == "Terrain" || collision.transform.tag == "Tank")
        {
            Debug.Log("Hit terrain!");
            // Create explosion
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
            Instantiate(explosionFX, transform.position, Quaternion.identity);

            // Set the damage value of the explosion and its radius
            // If a tank is within the damage radius, deal that damage to the tank
            explosion.GetComponent<Explosion>().explosionRadius = explosionRadius;
            explosion.GetComponent<Explosion>().damage = damage;

            myShooter.GetComponent<TankController>().ReportBulletTouchdown(transform.position);

            Destroy(this.gameObject, 0f);

        }
    }

    public void AssignShooter(GameObject go)
    {
        myShooter = go;
    }
}
