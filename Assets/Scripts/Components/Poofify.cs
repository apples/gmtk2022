using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poofify : MonoBehaviour
{
    public GameObject poofPrefab;

    public void Poof()
    {
        var poofObj = Instantiate(poofPrefab);
        poofObj.transform.position = this.transform.position;

        Destroy(this.gameObject);
    }

    public void CheckHealth(Health health)
    {
        if (health.CurrentHealth <= 0)
        {
            Poof();
        }
    }
}
