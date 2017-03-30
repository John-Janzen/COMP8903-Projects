using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public float Mass1, Mass2;
    public Vector3 InitialVelocityU, InitialVelocityV;
    public Vector3 FinalVelocityU, FinalVelocityV;
    public float elasticity, impulseJ, I1, I2;
    public Vector3 normal, nHat, P, r1, r2, W1, W2, impulseVec;
    public int collisionCount, y;
    public float p_i, p_fx, p_fy;
    public float L_i, L_f;
    public Vector2 initialParts, finalPartsX, finalPartsY, AngularPartsI, AngularPartsF;
    public Vector2 KE_i, KE_WI1, KE_f, L_Base, L_Rot;
    public float KE_Initial, KE_Final;
    private GameObject Ball1, Ball2;
    private bool start, done;
    private float UdotT, VdotT, UIN, VIN, normalMag;
    private Vector3 v_n, UFN, VFN, T;
    public Vector3 test;

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
                normal.x = -1;
                normal.z = 0;
                nHat = normal.normalized;
                P.x = Ball2.transform.position.x - 20;
                P.z = transform.position.z + (Ball2.transform.position.z - transform.position.z) / 2;

                r1 = P - transform.position;
                r1.x = transform.localScale.x / 2;
                r2 = P - Ball2.transform.position;
                r2.x = -transform.localScale.x / 2;

                UIN = Vector3.Dot(InitialVelocityU, nHat);
                VIN = Vector3.Dot(InitialVelocityV, nHat);

                v_n.x = (UIN - VIN) * nHat.x;
                v_n.y = (UIN - VIN) * nHat.y;

                I1 = (Mass1 * (Mathf.Pow(transform.localScale.x, 2) + Mathf.Pow(transform.localScale.z, 2))) / 12;
                I2 = (Mass2 * (Mathf.Pow(Ball2.transform.localScale.x, 2) + Mathf.Pow(Ball2.transform.localScale.z, 2))) / 12;

                impulseJ = -(InitialVelocityU.x - InitialVelocityV.x) * (elasticity + 1) 
                    * (1 / (((1 / Mass1) + (1 / Mass2) + Vector3.Dot(nHat, Vector3.Cross(Vector3.Cross(r1, nHat) / I1, r1))) 
                                                       + Vector3.Dot(nHat, Vector3.Cross(Vector3.Cross(r2, nHat) / I2, r2))));

                impulseVec = new Vector3(-v_n.x * (elasticity + 1) * (1 / (((1 / Mass1) + (1 / Mass2) + Vector3.Dot(nHat, Vector3.Cross(Vector3.Cross(r1, nHat) / I1, r1)))
                                                       + Vector3.Dot(nHat, Vector3.Cross(Vector3.Cross(r2, nHat) / I2, r2)))),
                    -v_n.y * (elasticity + 1) * (1 / (((1 / Mass1) + (1 / Mass2) + Vector3.Dot(nHat, Vector3.Cross(Vector3.Cross(r1, nHat) / I1, r1)))
                                                       + Vector3.Dot(nHat, Vector3.Cross(Vector3.Cross(r2, nHat) / I2, r2)))), 0);

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

                W1 = (Vector3.Cross(r1, impulseVec) / I1);
                W2 = (Vector3.Cross(r2, -impulseVec) / I2);

                KE_i.x = Mass1 * Mathf.Pow(InitialVelocityU.x, 2.0f) / 2 + Mass1 * Mathf.Pow(InitialVelocityU.y, 2.0f) / 2;
                KE_i.y = Mass2 * Mathf.Pow(InitialVelocityV.x, 2.0f) / 2 + Mass2 * Mathf.Pow(InitialVelocityV.y, 2.0f) / 2;
                KE_WI1.x = (I1 * Mathf.Pow(W1.y, 2) / 2);
                KE_WI1.y = (I2 * Mathf.Pow(W2.y, 2) / 2);

                KE_f.x = (Mass1 * Mathf.Pow(FinalVelocityU.x, 2) / 2);
                KE_f.y = (Mass2 * Mathf.Pow(FinalVelocityV.x, 2) / 2);

                KE_Initial = KE_i.x + KE_i.y;
                KE_Final = KE_f.x + KE_WI1.x + KE_f.y + KE_WI1.y;

                L_i = -r1.z * Mass1 * InitialVelocityU.x + r2.z * Mass2 * InitialVelocityV.x;
                L_Base.x = -r1.z * Mass1 * FinalVelocityU.x;
                L_Base.y = r2.z * Mass2 * FinalVelocityV.x;
                L_Rot.x = -I1 * W1.y;
                L_Rot.y = I2 * W2.y;
                L_f = L_Base.x + L_Rot.x + L_Base.y + L_Rot.y;
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

                Ball1.transform.Rotate(W1 * Mathf.Rad2Deg * Time.deltaTime);
                Ball2.transform.Rotate(W2 * Mathf.Rad2Deg * Time.deltaTime);
            }
        }

        initialParts.x = Mass1 * InitialVelocityU.x;
        initialParts.y = Mass2 * InitialVelocityV.x;
    }
}
