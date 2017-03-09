using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    public float velocity = 100;
    public float distanceX;
    public float velocityFinalY = 100;
    public float CorrectGunAngle;
    public Vector3 centerOfMassPos, targetPosition, horizontalRange;
    public float angularDisplacement, angularVelocity, angularAcceleration, initVelocity;
    public Vector3 pointPosition, pointVelocity, pointAccel;
    float yAdjustment;
    public bool start = false;
    public GameObject bulletPrefab;
    public bool hit;
    public float timetime = 0, positionVectorX, positionVectorY;
    GameObject bullet, target, point;
	// Use this for initialization
	void Start () {
        target = GameObject.Find("Target");
        targetPosition = target.transform.position;
        horizontalRange = target.transform.position - transform.position;
        distanceX = Mathf.Abs(transform.position.x)
            + Mathf.Abs(target.transform.position.x);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!bullet)
            {
                bullet = Instantiate(bulletPrefab);
                yAdjustment = transform.right.z * velocity;
                point = GameObject.Find("Sphere");

                start = true;
            }
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -10, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 10, 0) * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Destroy(bullet);
            timetime = 0;
            hit = false;
            start = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && target.transform.position.x > 195)
        {
            target.transform.Translate(new Vector3(-10, 0, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow) && target.transform.position.x < 395)
        {
            target.transform.Translate(new Vector3(10, 0, 0));
        }
        distanceX = Mathf.Abs(transform.position.x)
            + Mathf.Abs(target.transform.position.x);
        float time = distanceX / velocity; // divide cos(theta)
        velocityFinalY = Mathf.Sqrt(Mathf.Pow(velocity, 2f) + 2 * (9.81f)); // divide sin(theta)
        float something = (0.5f) * 9.81f * (time) / (velocity); //divide cos(theta)
        CorrectGunAngle = ((Mathf.Asin(2.0f * something)) / 2.0f) * Mathf.Rad2Deg;

        if (start == true && timetime <= 6 && !hit && bullet.transform.position.x < 395)
        {
            timetime += Time.deltaTime;
            float timeOfFlight = timetime;
            timeOfFlight = (float)System.Math.Round(timeOfFlight, 1);
            yAdjustment -= 9.81f * Time.deltaTime;
            bullet.transform.position += new Vector3(transform.right.x * velocity, 0, yAdjustment) * Time.deltaTime;

            targetPosition = target.transform.position;
            horizontalRange = target.transform.position - bullet.transform.position;
            pointPosition = point.transform.position;

            centerOfMassPos.x = transform.position.x + velocity * (Mathf.Cos(CorrectGunAngle * Mathf.Deg2Rad) * timeOfFlight);
            centerOfMassPos.z = velocity * (Mathf.Sin(CorrectGunAngle * Mathf.Deg2Rad) * timeOfFlight) + ((-9.81f * Mathf.Pow(timeOfFlight, 2) / 2));
            float velVectorX = velocity * Mathf.Cos(CorrectGunAngle * Mathf.Deg2Rad);
            float velVectorY = velocity * Mathf.Sin(CorrectGunAngle * Mathf.Deg2Rad) + -9.81f * timeOfFlight;

            angularVelocity = initVelocity + (angularAcceleration * timeOfFlight);
            angularDisplacement = (initVelocity * timeOfFlight) + ((angularAcceleration) * Mathf.Pow(timeOfFlight, 2) / 2);

            positionVectorX = 20 * Mathf.Cos((float)System.Math.Round(angularDisplacement, 1));
            positionVectorY = 20 * Mathf.Sin((float)System.Math.Round(angularDisplacement, 1));

            pointVelocity.x = velVectorX - (angularVelocity * positionVectorY);
            pointVelocity.z = velVectorY - (-angularVelocity * positionVectorX);

            pointAccel.x = (-angularAcceleration * positionVectorY) + (angularVelocity * (-angularVelocity * positionVectorX));
            pointAccel.z = -(9.81f + (-angularAcceleration * positionVectorX) + (-angularVelocity * (-angularVelocity * positionVectorY)));
            bullet.transform.Rotate(Vector3.up, (-angularVelocity * Mathf.Rad2Deg) * Time.deltaTime);
        }
    }
}
