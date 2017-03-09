using UnityEngine;
using System.Collections;

public class GunControls : MonoBehaviour {

    public Vector3 Range;
    public GameObject bulletPrefab;
    private GameObject target, bullet;
    private GameObject mainCamera;
    public float velocity = 100, ObserveFirst, ObserveSecond, timeOfFlight;
    public float ObserverFirst, ObserverSecond, yAccel = 9.81f, yAdjustment;
    public float Wind, CW, Mass, Tau, CD, dragTime;
    public bool start, hit, wind;
    public float x, y, z;
	// Use this for initialization
	void Start () {
        target = GameObject.Find("Target");
        mainCamera = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.A))
        {
            mainCamera.transform.RotateAround(new Vector3(0,0,0), Vector3.up, 100 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            mainCamera.transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, -100 * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!bullet)
            {
                bullet = Instantiate(bulletPrefab);
                yAdjustment = transform.right.y * velocity;
                start = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            wind = !wind;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (bullet)
            {
                Destroy(bullet);
            }

            timeOfFlight = 0;
            hit = false;
            start = false;
        }

        Range = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        ObserveFirst = (Mathf.Asin(9.81f * (Mathf.Sqrt(Mathf.Pow(Range.x, 2) + Mathf.Pow(Range.z, 2)) / Mathf.Pow(velocity, 2))) * Mathf.Rad2Deg);
        ObserveSecond = (180 - ObserveFirst) / 2;
        ObserveFirst = ObserveFirst / 2;

        ObserverFirst = Mathf.Acos(Range.x / (Mathf.Sqrt(Mathf.Pow(Range.x, 2) + Mathf.Pow(Range.z, 2)))) * Mathf.Rad2Deg;
        ObserverSecond = Mathf.Asin(Range.z / (Mathf.Sqrt(Mathf.Pow(Range.x, 2) + Mathf.Pow(Range.z, 2)))) * Mathf.Rad2Deg;
        Tau = Mass / CD;
        if (start)
        {
            if (!hit)
            {
                timeOfFlight += Time.deltaTime;
                yAdjustment -= yAccel * Time.deltaTime;
                if (wind)
                {
                    x = transform.position.x + ((transform.right.x * velocity * Tau) * (1 - Mathf.Exp(-(timeOfFlight / Tau)))) 
                        + (((CW * Wind * Mathf.Cos(ObserverFirst * Mathf.Deg2Rad))/ CD) * Tau * (1 - Mathf.Exp(-(timeOfFlight/Tau))))
                        - (((CW * Wind * Mathf.Cos(ObserverFirst * Mathf.Deg2Rad)) / CD) * timeOfFlight);
                    y = transform.position.y + ((transform.right.y * velocity * Tau) * (1 - Mathf.Exp(-(timeOfFlight / Tau)))) +
                        ((9.81f * Mathf.Pow(Tau, 2)) * (1 - Mathf.Exp(-(timeOfFlight / Tau)))) -
                        (9.81f * Tau * timeOfFlight);
                    z = transform.position.z + ((transform.right.z * velocity * Tau) * (1 - Mathf.Exp(-(timeOfFlight / Tau))))
                        + (((CW * Wind * Mathf.Sin(ObserverSecond * Mathf.Deg2Rad)) / CD) * Tau * (1 - Mathf.Exp(-(timeOfFlight / Tau))))
                        - (((CW * Wind * Mathf.Sin(ObserverSecond * Mathf.Deg2Rad)) / CD) * timeOfFlight);
                    bullet.transform.position = new Vector3(x, y, z);
                } else
                {
                    bullet.transform.position += new Vector3(transform.right.x * velocity, yAdjustment, transform.right.z * velocity) * Time.deltaTime;
                }
            }
        }
    }
}
