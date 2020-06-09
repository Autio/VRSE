using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TankController : MonoBehaviour
{
    private GameController gc;

    public GameObject bulletObject;
    public GameObject turret;
    public GameObject turretVerticalRotator;
    [SerializeField]
    GameObject cannon;
    public GameObject mainCam;
    private List<GameObject> bullets;

    public bool activeTank = false;
    int hitpoints = 10;

    public float shotForce = 1000f;
    private float angle = 0;
    private float orientation = 0;

    public float orientationSpeedModifier = 2f;
    public float angleSpeedModifier = 10f;
    public float forceSpeedModifier = 20f;

    public bool tankCamOnly = false;


    // Info texts in UI 
    [SerializeField]
    private GameObject forceText, angleText, orientationText, ammoText;

    [SerializeField]
    private GameObject bulletSpawnArea;

    private enum ammoTypes { normal, big };
    private ammoTypes currentAmmo = ammoTypes.normal;

    private int ammoResources = 100; // Resources can be used to buy better ammo
    private int activeAmmoIndex = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        gc = GameController.instance;

        bullets = new List<GameObject>();

        // Initial turret rotation
        angle = turretVerticalRotator.transform.rotation.x;
        orientation = turret.transform.rotation.y;

        turret.transform.rotation = Quaternion.AngleAxis(orientation, transform.TransformDirection(Vector3.up));
        turret.transform.rotation *= Quaternion.AngleAxis(turret.transform.rotation.z, transform.TransformDirection(Vector3.forward));

        turretVerticalRotator.transform.rotation = Quaternion.AngleAxis(angle, transform.TransformDirection(Vector3.right));

        ammoText.GetComponent<TMP_Text>().text = "Ammo type: " + Arsenal.instance.arsenal[activeAmmoIndex].name;

    }

    // Update is called once per frame
    void Update()
    {

        if (activeTank && gc.currentGameState == GameController.gameStates.playerTurn)
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

             angle = Mathf.Clamp(angle, -160f, -90f);
    

            // Set turret position
            //Vector3 currentEulerAngles = new Vector3(angle, orientation, turret.transform.eulerAngles.z);
            //turret.transform.eulerAngles = currentEulerAngles;
            Debug.Log(orientation);
            turret.transform.rotation = Quaternion.AngleAxis(orientation, transform.TransformDirection(Vector3.up));
            turret.transform.rotation *= Quaternion.AngleAxis(turret.transform.rotation.z, transform.TransformDirection(Vector3.forward));

            turretVerticalRotator.transform.rotation = Quaternion.AngleAxis(orientation, transform.TransformDirection(Vector3.up));

            turretVerticalRotator.transform.rotation *= Quaternion.AngleAxis(angle, transform.TransformDirection(Vector3.right));

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

            // Change ammo 
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                NextAmmoType();
            }

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
        // Instantiate bullet based on the selected ammo
        AmmoModel currentAmmo = Arsenal.instance.arsenal[activeAmmoIndex];
        GameObject bullet = Instantiate(currentAmmo.ammoPrefab, bulletSpawnArea.transform.position, Quaternion.identity);

        // Add force
        // Direction of the turret
        Vector3 shotDir = -(turret.transform.position - bullet.transform.position).normalized;        
        bullet.GetComponent<Rigidbody>().AddForce(shotDir * shotForce);

        if (!tankCamOnly)
        {
            bullet.transform.Find("BulletCamera").GetComponent<Camera>().enabled = true;
        }
        // mainCam.GetComponent<Camera>().enabled = false;

        bullets.Add(bullet);

        // Add bullet properties from the ammo model 
        

        // Make the cannon recoil
        // Also freeze movement for the duration of the act 
        StartCoroutine(BlockPlayerMovement(.4f));

        Sequence seq = DOTween.Sequence();
        float startingScale = cannon.GetComponent<Transform>().localScale.z;
        seq.Append(cannon.GetComponent<Transform>().DOScale(startingScale * 1.6f, .1f));
        seq.Append(cannon.GetComponent<Transform>().DOScale(startingScale * 1f, .3f));
    
        // Make the cannon animate comically to emphasize the shot effect
        Vector3 currentPos = cannon.transform.position;
      //  seq.Append(cannon.GetComponent<Transform>().DOMove((cannon.transform.position - turretVerticalRotator.transform.position).normalized, .4f));

     //   seq.Append(cannon.GetComponent<Transform>().DOMove(currentPos, 2f));

        

    }

    private IEnumerator BlockPlayerMovement(float duration = .8f)
    {
        // Player cannot fire during this cooldown
        gc.currentGameState = GameController.gameStates.transition;
        yield return new WaitForSeconds(duration);
        // Should this not move to the next player typically? Might be better to have this sit in the GameController when enemy AI is set up
        gc.currentGameState = GameController.gameStates.playerTurn;

    }

    public void NextAmmoType()
       
    {
        activeAmmoIndex++;
        if(activeAmmoIndex > Arsenal.instance.arsenal.Count - 1)
        {
            activeAmmoIndex = 0;
        }
        ammoText.GetComponent<TMP_Text>().text = "Ammo type: "+ Arsenal.instance.arsenal[activeAmmoIndex].name;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Explosion")
        {
            // Grab the damage in the explosion

            Debug.Log("Direct hit! Tank destroyed");
            Destroy(this.gameObject, 0f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Explosion")
        {
            TakeDamage(other.GetComponent<Explosion>().damage);
            

            // TODO: Play some kind of damage effect

            Debug.Log("Direct hit! Tank destroyed");
            Destroy(this.gameObject, 0f);

        }
    }

    public void TakeDamage(int damage)
    {
        hitpoints -= damage;
        Debug.Log("Tank " + this.gameObject.name + " takes " + damage + " damage!");

        if(hitpoints <= 0)
        {
            Destroy(this.gameObject, 0f);

        }
    }

}
