using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public float Mass1, Mass2;
    public Vector3 InitialVelocityU, InitialVelocityV;
    public Vector3 FinalVelocityU, FinalVelocityV;
    public float elasticity, impulseJ;
    public Vector3 normal;
    public int collisionCount, y;
    public float p_i, p_fx, p_fy;
    public Vector2 initialParts, finalPartsX, finalPartsY;
    public Vector2 KE_i, KE_f;
    public float KE_Initial, KE_Final;
    private GameObject Ball1, Ball2;
    private bool start, done;
    private float UdotT, VdotT, UIN, VIN, normalMag;
    private Vector3 nHat, v_n, impulseVec, UFN, VFN, T;

    // Use this for initialization
    void Start () {
        Ball1 = GameObject.Find("Cube");
        Ball2 = GameObject.Find("Cube2");
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) start = !start;

        if (start)
        {
            if (collisionCount == 1 && !done)
            {
                //FinalVelocityU.x = (impulseJ / Mass1) + InitialVelocity.x * normal.x;
                //FinalVelocityU.y = (impulseJ / Mass1) + InitialVelocity.x * normal.y;
                //Ball1.transform.position += new Vector3(FinalVelocityU.x * Time.deltaTime, 0, 0);
                //Ball2.transform.position += new Vector3(FinalVelocityV.x * Time.deltaTime, 0, 0);
                normal.x = Mathf.Sqrt(Mathf.Pow((Ball1.transform.localScale.x / 2) + (Ball2.transform.localScale.z / 2), 2.0f) - Mathf.Pow(y, 2.0f));
                normal.y = y;
                nHat = normal.normalized;

                UIN = Vector3.Dot(InitialVelocityU, nHat);
                VIN = Vector3.Dot(InitialVelocityV, nHat);

                v_n.x = (UIN - VIN) * nHat.x;
                v_n.y = (UIN - VIN) * nHat.y;

                impulseJ = -(UIN - VIN) * (elasticity + 1) * ((Mass1 * Mass2) / (Mass1 + Mass2));
                impulseVec = new Vector3(-v_n.x * (elasticity + 1) * ((Mass1 * Mass2) / (Mass1 + Mass2)),
                    -v_n.y * (elasticity + 1) * ((Mass1 * Mass2) / (Mass1 + Mass2)), 0);

                UFN = new Vector3((impulseVec.x / Mass1) + UIN * nHat.x, (impulseVec.y / Mass1) + UIN * nHat.y, 0);
                VFN = new Vector3((-impulseVec.x / Mass2) + VIN * nHat.x, (-impulseVec.y / Mass2) + VIN * nHat.y, 0);

                T = Vector3.Cross(Vector3.Cross(nHat, InitialVelocityU), nHat).normalized;

                UdotT = Vector3.Dot(InitialVelocityU, T);
                VdotT = Vector3.Dot(InitialVelocityV, T);

                FinalVelocityU = UFN + (UdotT * T);
                FinalVelocityV = VFN + (VdotT * T);

                FinalVelocityU.y = -FinalVelocityU.y;
                FinalVelocityV.y = -FinalVelocityV.y;

                finalPartsX.x = Mass1 * FinalVelocityU.x;
                finalPartsX.y = Mass2 * FinalVelocityV.x;
                finalPartsY.x = Mass1 * FinalVelocityU.y;
                finalPartsY.y = Mass2 * FinalVelocityV.y;

                p_i = initialParts.x + initialParts.y;
                p_fx = finalPartsX.x + finalPartsX.y;
                p_fy = finalPartsY.x + finalPartsY.y;

                KE_i.x = Mass1 * Mathf.Pow(InitialVelocityU.x, 2.0f) / 2 + Mass1 * Mathf.Pow(InitialVelocityU.y, 2.0f) / 2;
                KE_i.y = Mass2 * Mathf.Pow(InitialVelocityV.x, 2.0f) / 2 + Mass2 * Mathf.Pow(InitialVelocityV.y, 2.0f) / 2;

                KE_f.x = Mass1 * Mathf.Pow(FinalVelocityU.x, 2.0f) / 2 + Mass1 * Mathf.Pow(FinalVelocityU.y, 2.0f) / 2;
                KE_f.y = Mass2 * Mathf.Pow(FinalVelocityV.x, 2.0f) / 2 + Mass2 * Mathf.Pow(FinalVelocityV.y, 2.0f) / 2;

                KE_Initial = KE_i.x + KE_i.y;
                KE_Final = KE_f.x + KE_f.y;
                done = true;
            }
            else if (collisionCount != 1)
            {
                Ball1.transform.position += new Vector3(InitialVelocityU.x * Time.deltaTime, 0, InitialVelocityU.y * Time.deltaTime);
                Ball2.transform.position += new Vector3(InitialVelocityV.x * Time.deltaTime, 0, InitialVelocityV.y * Time.deltaTime);
            } else if (collisionCount == 1 && done)
            {
                Ball1.transform.position += new Vector3(FinalVelocityU.x * Time.deltaTime, 0, FinalVelocityU.y * Time.deltaTime);
                Ball2.transform.position += new Vector3(FinalVelocityV.x * Time.deltaTime, 0, FinalVelocityV.y * Time.deltaTime);
            }
        }

        initialParts.x = Mass1 * InitialVelocityU.x;
        initialParts.y = Mass2 * InitialVelocityV.x;
    }
}
