using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Explosion : MonoBehaviour
{
    public float explosionRadius = 20f;
    public int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 1.6f);

        Sequence seq = DOTween.Sequence();
        seq.Append(this.GetComponent<Transform>().DOScale(explosionRadius, .2f));
        seq.Append(this.GetComponent<Transform>().DOScale(.1f, 1.5f));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
