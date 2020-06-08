using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int shotsTaken;
    private Transform turret;
    private Transform playerTank;
    private Vector3 hitLocation;

    // Enemy AI to hit the player
    // Step 1) get the direction of the player 
    // Step 2) Randomize the angle and power slightly
    // Step 3) Track how far the shot landed
    // Step 4) Nudget values a bit closer to the player

    // Start is called before the first frame update
    void Start()
    {
        playerTank = GameObject.Find("PlayerTank").transform;
        turret = this.transform.Find("Turret");
    }

    void Shoot()
    {
        // Rotate turret to player direction 
        Debug.Log("Rotating to player");

        turret.LookAt(playerTank);
        //float angle = Vector3.Angle(this.transform.position - playerTank.position, transform.up);
        //turret.transform.rotation = Quaternion.AngleAxis(angle, transform.TransformDirection(Vector3.up));

        // turret.LookAt(playerTank, Vector3.right);

    }

    // Update is called once per frame
    void Update()
    {
        turret.LookAt(playerTank);
        if (Input.GetKeyDown(KeyCode.O))
        {
            
            Shoot();

        }
    }
}
