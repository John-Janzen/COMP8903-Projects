using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {
    public ProjectileScript ps;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        ps.hit = true;
    }
}
