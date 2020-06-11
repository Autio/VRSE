using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject playerTank;
    private Vector3 targetV3;
    private bool rangingShot;
    private List<Vector3> previousHits;
    private List<float[]> previousParams;
    private float orientation, angle, power;

    // Start is called before the first frame update
    void Start()
    {
        previousParams = new List<float[]>();
        previousHits = new List<Vector3>();
        rangingShot = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject GetPlayerTankTransform()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public void AdjustTarget()
    {
        Vector3 playerPosition = GetPlayerTankTransform().transform.position;
        if (rangingShot)
        {
            previousHits.Add(playerPosition);
            targetV3 = playerPosition * Random.Range(0.9f, 1.1f);
            orientation = Vector3.Angle(transform.forward, new Vector3 (targetV3.x, 0, targetV3.z));
            angle = -125f;
            power = 2000f;

            rangingShot = false;
        }
        else
        {
            targetV3 = playerPosition;
            orientation = Vector3.Angle(transform.forward, new Vector3(targetV3.x, 0, targetV3.z));
            if (Vector3.Distance(previousHits[previousHits.Count - 1], transform.position) >= Vector3.Distance(playerPosition, transform.position))
            {
                power *= 0.95f;
            }
            else
            {
                power *= 1.05f;
            }
        }

        float[] usedParams = { orientation, angle, power };
        previousParams.Add(usedParams);
    }

    public void GetTargetParameters(out float o, out float a, out float p)
    {
        o = orientation;
        a = angle;
        p = power;
    }

    public void ReportHit(Vector3 v)
    {
        previousHits.Add(v);
    }
}
