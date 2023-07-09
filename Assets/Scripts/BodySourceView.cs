using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour
{
    float ga = 0.0f;

    public GameObject SaberPrefabRED;
    public GameObject SaberPrefabBLUE;

    public Material BoneMaterial;
    public GameObject BodySourceManager;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<
        Kinect.JointType,
        Kinect.JointType
    >()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    void Update()
    {
        if (BodySourceManager == null)
        {
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        Kinect.JointType[] filteredJoints =
        {
            Kinect.JointType.HandLeft,
            Kinect.JointType.WristLeft,
            Kinect.JointType.ElbowLeft,
            Kinect.JointType.HandRight,
            Kinect.JointType.WristRight,
            Kinect.JointType.ElbowRight
        }; // Arreglo de articulaciones filtradas

        foreach (Kinect.JointType jt in filteredJoints)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);

            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;

            if (jt.ToString() == "ElbowRight" || jt.ToString() == "ElbowLeft")
            {
                GameObject creater;

                if (jt.ToString() == "ElbowRight")
                {
                    creater = Instantiate(SaberPrefabRED);
                }
                else
                {
                    creater = Instantiate(SaberPrefabBLUE);
                }

                creater.transform.position = jointObj.transform.position;
                // creater.transform.rotation = jointObj.transform.rotation;
                // creater.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                // Transform firstChild  = creater.transform.GetChild(0);
                // firstChild.rotation = Quaternion.Euler(90f, 0f, 0f);

                creater.transform.GetChild(0).transform.rotation = Quaternion.Euler(
                    90.0f,
                    0.0f,
                    0.0f
                );
                creater.transform.GetChild(1).transform.rotation = Quaternion.Euler(
                    90.0f,
                    0.0f,
                    0.0f
                );
                creater.transform.GetChild(2).transform.rotation = Quaternion.Euler(
                    90.0f,
                    0.0f,
                    0.0f
                );
                // Transform firstChild = creater.GetChild(0);
                // firstChild.rotation = Quaternion.Euler(0f, 0f, 0f);

                creater.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                creater.transform.parent = jointObj.transform;
                // creater.AddComponent<Rigidbody>();
                // creater.transform.GetChild(0).AddComponent<BoxCollider>();
                // creater.transform.GetChild(0).tag = "Saber";

                // creater.transform.GetChild(0).GetComponent<Collider>().isTrigger = true;

                // creater.GetComponent<Rigidbody>().useGravity = false;
            }
        }

        return body;
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        Kinect.JointType[] filteredJoints =
        {
            Kinect.JointType.HandLeft,
            Kinect.JointType.WristLeft,
            Kinect.JointType.ElbowLeft,
            Kinect.JointType.HandRight,
            Kinect.JointType.WristRight,
            Kinect.JointType.ElbowRight
        }; // Arreglo de articulaciones filtradas

        // Variables para almacenar la posición de las manos y los codos
        Vector3 wristRightPosition = Vector3.zero,
            wristLeftPosition = Vector3.zero,
            elbowRightPosition = Vector3.zero,
            elbowLeftPosition = Vector3.zero;

        // Variables para almacenar los vectores dirección de las manos a los codos
        Vector3 rightElbowToWristVector,
            leftElbowToWristVector;

        // Buscar las posiciones de las manos y los codos en la escena
        foreach (Kinect.JointType jt in filteredJoints)
        {
            if (jt.ToString() == "WristRight")
            {
                wristRightPosition = bodyObject.transform.Find(jt.ToString()).position;
            }
            else if (jt.ToString() == "WristLeft")
            {
                wristLeftPosition = bodyObject.transform.Find(jt.ToString()).position;
            }
            else if (jt.ToString() == "ElbowRight")
            {
                elbowRightPosition = bodyObject.transform.Find(jt.ToString()).position;
            }
            else if (jt.ToString() == "ElbowLeft")
            {
                elbowLeftPosition = bodyObject.transform.Find(jt.ToString()).position;
            }
        }

        // Calcular los vectores dirección de las manos a los codos
        rightElbowToWristVector = wristRightPosition - elbowRightPosition;
        leftElbowToWristVector = wristLeftPosition - elbowLeftPosition;

        foreach (Kinect.JointType jt in filteredJoints)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }

            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if (targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.SetColors(
                    GetColorForState(sourceJoint.TrackingState),
                    GetColorForState(targetJoint.Value.TrackingState)
                );
            }
            else
            {
                lr.enabled = false;
            }

            // Debug.Log(sourceJoint.name);
            // if (jt.ToString() == "HandRight" || jt.ToString() == "HandLeft")
            if (jt.ToString() == "ElbowRight" || jt.ToString() == "ElbowLeft")
            {
                Transform transform;
                if (jt.ToString() == "ElbowRight")
                {
                    transform = bodyObject.transform
                        .Find(jt.ToString())
                        .transform.Find("LSRed(Clone)")
                        .transform;
                }
                else
                {
                    transform = bodyObject.transform
                        .Find(jt.ToString())
                        .transform.Find("LSBlue(Clone)")
                        .transform;
                }

                // transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 1f)*20));
                // ga++;
                if (transform != null)
                {
                    if (jt.ToString() == "ElbowRight")
                    {
                        Vector3 desplazamiento = rightElbowToWristVector * 10f;
                        Vector3 resultado = wristRightPosition + desplazamiento;
                        transform.LookAt(new Vector3(resultado.x, resultado.y, resultado.z));
                    }
                    else
                    {
                        Vector3 desplazamiento = leftElbowToWristVector * 10f;
                        Vector3 resultado = wristLeftPosition + desplazamiento;
                        transform.LookAt(new Vector3(resultado.x, resultado.y, resultado.z));
                    }
                }
            }

            // Obtener la posición de inicio y fin de la línea
        }
    }

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        // return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
        return new Vector3(
            joint.Position.X * 10,
            joint.Position.Y * 10,
            joint.Position.Z * -10 + 22
        );
    }
}
