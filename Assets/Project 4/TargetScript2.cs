using UnityEngine;
using System.Collections;

public class TargetScript2 : MonoBehaviour {
    public GunControls gc;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        gc.hit = true;
    }
}
