using UnityEngine;
using System.Collections;

public class BoatScript2 : MonoBehaviour
{

    public float shipMass = 10000000, thrust = 100000000, depth;
    public float drag, percentDrag, terminalVelocity;
    public float acceleration, velocity = 0, initVelocity = 0;
    public float time, tau;
    public bool start;
    public float initPosX;
    public Vector3 position;


    // Use this for initialization
    void Start()
    {
        time = 0;
        initPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        depth = shipMass / ((transform.localScale.x * transform.localScale.y)
            * (shipMass / (transform.localScale.x * transform.localScale.z)));
        position.z = -depth;
        terminalVelocity = thrust / (drag * 1000000);

        tau = shipMass / (drag * 1000000);

        

        if (start && time <= 12)
        {
            time += Time.deltaTime;
            position.x = initPosX + (terminalVelocity * time) + ((terminalVelocity - initVelocity) * tau) * (Mathf.Exp(-(time / tau)) - 1);
            velocity = terminalVelocity - (Mathf.Exp(-(time / tau)) * (terminalVelocity - initVelocity));
        }
        acceleration = (thrust - ((drag * 1000000) * velocity)) / shipMass;
        transform.position = position;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            start = !start;
        }
    }
}
