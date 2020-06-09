using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Ammo type
    enum ammoTypes { normal };
    ammoTypes ammoType = ammoTypes.normal;

    // Typical ammo creates a blast whenever it hits the terrain
    public GameObject regularExplosionPrefab;
    public GameObject explosionFX;


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
            Instantiate(regularExplosionPrefab, transform.position, Quaternion.identity);
            Instantiate(explosionFX, transform.position, Quaternion.identity);

            Destroy(this.gameObject, 0f);

        }
    }
}
