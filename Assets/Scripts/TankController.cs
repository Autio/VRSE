using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using TMPro;

public class TankController : MonoBehaviour
{
    public GameObject bulletObject;
    public GameObject turret;
    public GameObject mainCam;
    private List<GameObject> bullets;

    public bool activeTank = false;

    public float shotForce = 1000f;
    private float angle = 0;
    private float orientation = 0;

    public float orientationSpeedModifier = 2f;
    public float angleSpeedModifier = 10f;
    public float forceSpeedModifier = 20f;

    public bool tankCamOnly = false;


    // Info texts in UI 
    [SerializeField]
    private GameObject forceText, angleText, orientationText;

    [SerializeField]
    private GameObject bulletSpawnArea;

    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();

        // Initial turret rotation
        angle = turret.transform.rotation.x;
        orientation = turret.transform.rotation.y;

        turret.transform.rotation = Quaternion.AngleAxis(orientation, transform.TransformDirection(Vector3.up));
        turret.transform.rotation *= Quaternion.AngleAxis(angle, transform.TransformDirection(Vector3.right));
        turret.transform.rotation *= Quaternion.AngleAxis(turret.transform.rotation.z, transform.TransformDirection(Vector3.forward));
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTank)
        {
            forceText.GetComponent<TMP_Text>().text = shotForce.ToString();
            angleText.GetComponent<TMP_Text>().text = angle.ToString();
            orientationText.GetComponent<TMP_Text>().text = orientation.ToString();

            // Turn turret
            if (Input.GetKey(KeyCode.A))
            {
                orientation -= Time.deltaTime * orientationSpeedModifier;
            }

            if (Input.GetKey(KeyCode.D))
            {
                orientation += Time.deltaTime * orientationSpeedModifier;
            }



            // Tilt turret
            // Turn turret
            if (Input.GetKey(KeyCode.S))
            {
                angle += Time.deltaTime * angleSpeedModifier;
            }

            if (Input.GetKey(KeyCode.W))
            {
                angle -= Time.deltaTime * angleSpeedModifier;
            }

            // Keep angle reasonable

                angle = Mathf.Clamp(angle, 30f,90f);
    

            // Set turret position
            //Vector3 currentEulerAngles = new Vector3(angle, orientation, turret.transform.eulerAngles.z);
            //turret.transform.eulerAngles = currentEulerAngles;
            Debug.Log(orientation);
            turret.transform.rotation = Quaternion.AngleAxis(orientation, transform.TransformDirection(Vector3.up));
            turret.transform.rotation *= Quaternion.AngleAxis(angle, transform.TransformDirection(Vector3.right));
            turret.transform.rotation *= Quaternion.AngleAxis(turret.transform.rotation.z, transform.TransformDirection(Vector3.forward));

            // Adjust power
            if (Input.GetKey(KeyCode.E))
            {
                shotForce += Time.deltaTime * forceSpeedModifier;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                shotForce -= Time.deltaTime * forceSpeedModifier;
            }

            shotForce = Mathf.Clamp(shotForce, 1000f, 4000f);


            // Fire
            if (Input.GetKeyDown(KeyCode.F))
            {
                FireGun();
            }

            // Reset cam
            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (GameObject bullet in bullets)
                {
                    bullet.transform.Find("BulletCamera").GetComponent<Camera>().enabled = false;
                }

                mainCam.GetComponent<Camera>().enabled = true;
            }
        }
    }

    public void FireGun()
    {
        // Instantiate bullet 
        GameObject bullet = Instantiate(bulletObject, bulletSpawnArea.transform.position, Quaternion.identity);

        // Add force
        // Direction of the turret
        Vector3 shotDir = -(turret.transform.position - bullet.transform.position).normalized;
        Debug.Log(shotDir);
        
        bullet.GetComponent<Rigidbody>().AddForce(shotDir * shotForce);
        if (!tankCamOnly)
        {
            bullet.transform.Find("BulletCamera").GetComponent<Camera>().enabled = true;
        }
        // mainCam.GetComponent<Camera>().enabled = false;

        bullets.Add(bullet);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Explosion")
        {
            Debug.Log("Direct hit! Tank destroyed");
            Destroy(this.gameObject, 0f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Explosion")
        {
            Debug.Log("Direct hit! Tank destroyed");
            Destroy(this.gameObject, 0f);

        }
    }
}
