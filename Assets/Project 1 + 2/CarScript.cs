using UnityEngine;
using System.Collections;

public class CarScript : MonoBehaviour {

    public int carMass = 1000, gasTankMass = 200, driverCenterMass = 100;
    public Vector3 carPos, gasTankPos, driverPos;
    public int totalMass;
    public float MomentOfInertia;
    public Vector2 CenterOfMass;
    public float Velocity = 100f;
    public bool drag, start;
    public float SecondsTime, Acceleration = -10;
    public float speed = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.A)) {
            transform.Translate(new Vector3(10, 0, 0));
        } else if (Input.GetKeyDown(KeyCode.D)) {
            transform.Translate(new Vector3(-10, 0, 0));
        } else if (Input.GetKeyDown(KeyCode.W)) {
            transform.Translate(new Vector3(0, 0, -10));
        } else if (Input.GetKeyDown(KeyCode.S)) {
            transform.Translate(new Vector3(0, 0, 10));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            drag = !drag;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            transform.position = new Vector3(-300, 0, 350);
            SecondsTime = 0;
            start = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            speed = Velocity;
            start = true;
        }
        totalMass = carMass + gasTankMass + driverCenterMass;
        carPos = transform.position;
        gasTankPos = transform.GetChild(0).transform.position;
        driverPos = transform.GetChild(1).transform.position;
        CenterOfMass.x = ((carMass * carPos.x) + (gasTankMass * gasTankPos.x) + (driverCenterMass * driverPos.x)) / totalMass;
        CenterOfMass.y = ((carMass * carPos.z) + (gasTankMass * gasTankPos.z) + (driverCenterMass * driverPos.z)) / totalMass;

        float CarMomentOfI = (carMass * (Mathf.Pow(200, 2) + Mathf.Pow(100, 2))) / 12;
        float GasMomentOfI = (gasTankMass * (Mathf.Pow(40, 2) + Mathf.Pow(80, 2))) / 12;
        float DriverMomentOfI = (driverCenterMass * (Mathf.Pow(40, 2) + Mathf.Pow(40, 2))) / 12;

        float CarDistance = Mathf.Sqrt(Mathf.Pow((carPos.x - CenterOfMass.x), 2) + Mathf.Pow((carPos.z - CenterOfMass.y), 2));
        float GasDistance = Mathf.Sqrt(Mathf.Pow((gasTankPos.x - CenterOfMass.x), 2) + Mathf.Pow((gasTankPos.z - CenterOfMass.y), 2));
        float DriverDistance = Mathf.Sqrt(Mathf.Pow((driverPos.x - CenterOfMass.x), 2) + Mathf.Pow((driverPos.z - CenterOfMass.y), 2));

        float CarCOMInertia = (CarMomentOfI + (carMass * Mathf.Pow(CarDistance, 2)));
        float GasCOMInertia = (GasMomentOfI + (gasTankMass * Mathf.Pow(GasDistance, 2)));
        float DriverCOMInertia = (DriverMomentOfI + (driverCenterMass * Mathf.Pow(DriverDistance, 2)));

        MomentOfInertia = CarCOMInertia + GasCOMInertia + DriverCOMInertia;
        if (start == true && SecondsTime <= 8)
        {
            SecondsTime += Time.deltaTime;
            if (drag)
            {
                Acceleration = (-0.001f * Mathf.Pow(speed, 2));
            }
            speed += Acceleration * Time.deltaTime;
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
    }
}
