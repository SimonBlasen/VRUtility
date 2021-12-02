using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControllerAngTracker : MonoBehaviour, VRControllerInit
{
    VRController vrController = null;

    public TextMeshPro textMesh;
    public Rigidbody rig;
    
    public Quaternion startXRot;
    public Quaternion endRotX;
    public float takenTime = 0f;
    public float summedAngVelX = 0f;

    public float dividedAngVel = 0f;
    public float angleErrorFac = 0f;

    public bool computing = false;

    [Space]
    public float simpleCorrFactor = 1f;

    // Start is called before the first frame update
    void Start()
    {
        VRController.RegisterInit(this, true);
        VRController.RegisterInit(this, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (vrController != null)
        {
            if (!computing && vrController.TriggerButtonDown)
            {
                takenTime = Time.time;
                startXRot = vrController.Rotation;
                summedAngVelX = 0f;
                computing = true;

                rig.transform.position = new Vector3(0f, 1f, -0.25f);
                rig.transform.rotation = Quaternion.identity;
                rig.velocity = Vector3.zero;
                rig.angularVelocity = Vector3.zero;
            }
            else if (computing && vrController.TriggerButtonDown)
            {
                takenTime = Time.time - takenTime;
                computing = false;
                endRotX = vrController.Rotation;

                rig.transform.position = new Vector3(0f, 1f, -0.25f);
                rig.transform.rotation = Quaternion.identity;
                rig.velocity = Vector3.zero;
                rig.angularVelocity = Vector3.zero;

                //dividedAngVel = summedAngVelX / takenTime;

                angleErrorFac = Quaternion.Angle(startXRot, endRotX) / summedAngVelX;
            }

            textMesh.text = "startXRot: " + startXRot.ToString("n3") + "\n"
                            + "endRotX: " + endRotX.ToString("n3") + "\n"
                            + "takenTime: " + takenTime.ToString("n3") + "\n"
                            + "summedAngVelX: " + summedAngVelX.ToString("n3") + "\n"
                            + "dividedAngVel: " + dividedAngVel.ToString("n3") + "\n"
                            + "angleErrorFac: " + angleErrorFac.ToString("n3") + "\n"
                            + "simpleCorrFactor: " + simpleCorrFactor.ToString("n3") + "\n";
        }
    }

    private void FixedUpdate()
    {

        if (computing)
        {
            summedAngVelX += vrController.AngularVelocity.x * simpleCorrFactor * Time.fixedDeltaTime;

            rig.angularVelocity = vrController.AngularVelocity * simpleCorrFactor;
            rig.transform.position = new Vector3(0f, 1f, -0.25f);

            float vecLen = vrController.AngularVelocity.magnitude;

            GraphManager.Graph.Plot("Vel X", vecLen, Color.green, new GraphManager.Matrix4x4Wrapper(transform.position, transform.rotation, transform.localScale));
        }
        else
        {
            rig.transform.position = new Vector3(0f, 1f, -0.25f);
            rig.transform.rotation = Quaternion.identity;
            rig.velocity = Vector3.zero;
            rig.angularVelocity = Vector3.zero;
        }
    }

    public void Inited(VRController vrController)
    {
        this.vrController = vrController;
    }
}
