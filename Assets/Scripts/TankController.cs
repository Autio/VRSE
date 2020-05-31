using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public GameObject bulletObject;
    public GameObject turret;
    public GameObject mainCam;
    private List<GameObject> bullets;

    public float shotForce = 1000f;
    
    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();   
    }

    // Update is called once per frame
    void Update()
    {
        // Turn turret

        // Tilt turret

        // Adjust power

        // Fire
        if(Input.GetKeyDown(KeyCode.F))
        {
            FireGun();
        }

        // Reset cam
        if(Input.GetKeyDown(KeyCode.R))
        {
            foreach(GameObject bullet in bullets)
            {
                bullet.transform.Find("BulletCamera").GetComponent<Camera>().enabled = false;
            }
            
            mainCam.GetComponent<Camera>().enabled = true;
        }

    }

    public void FireGun()
    {
        // Instantiate bullet 
        GameObject bullet = Instantiate(bulletObject, GameObject.Find("BulletSpawnArea").transform.position, Quaternion.identity);

        // Add force
        // Direction of the turret
        Vector3 shotDir = -(turret.transform.position - bullet.transform.position).normalized;
        Debug.Log(shotDir);
        //bullet.GetComponent<Rigidbody>().AddExplosionForce(1000f, bullet.transform.position,55f, 0);
        bullet.GetComponent<Rigidbody>().AddForce(shotDir * shotForce);
        bullet.transform.Find("BulletCamera").GetComponent<Camera>().enabled = true;
        mainCam.GetComponent<Camera>().enabled = false;

        bullets.Add(bullet);

    }
}
