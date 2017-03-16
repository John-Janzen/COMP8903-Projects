using UnityEngine;
using System.Collections;

public class CollisionDetect : MonoBehaviour {

    SceneManager sm;
    Renderer rend;
	// Use this for initialization
	void Start () {
        sm = GameObject.Find("Ball1").GetComponent<SceneManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        sm.collisionCount = 1;
    }
}
