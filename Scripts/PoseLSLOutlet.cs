using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class PoseLSLOutlet : MonoBehaviour
{
    private liblsl.StreamOutlet outlet;
    private float[] currentSample;

    public string StreamName = "Unity.PicoVRHeadPose";
    public string StreamType = "MoCap";
    public string StreamId = "VRHeadPose-MoCap1234";

    // Start is called before the first frame update
    void Start()
    {
        liblsl.StreamInfo streamInfo = new liblsl.StreamInfo(StreamName, StreamType, 7, Time.fixedDeltaTime * 1000, liblsl.channel_format_t.cf_float32);
        liblsl.XMLElement chans = streamInfo.desc().append_child("channels");
        chans.append_child("channel").append_child_value("label", "PosX");
        chans.append_child("channel").append_child_value("label", "PosY");
        chans.append_child("channel").append_child_value("label", "PosZ");
        chans.append_child("channel").append_child_value("label", "RotW");
        chans.append_child("channel").append_child_value("label", "RotX");
        chans.append_child("channel").append_child_value("label", "RotY");
        chans.append_child("channel").append_child_value("label", "RotZ");
        outlet = new liblsl.StreamOutlet(streamInfo);
        currentSample = new float[7];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.transform.position;
        currentSample[0] = pos.x;
        currentSample[1] = pos.y;
        currentSample[2] = pos.z;
        Quaternion quat = gameObject.transform.rotation;
        currentSample[3] = quat.w;
        currentSample[4] = quat.x;
        currentSample[5] = quat.y;
        currentSample[6] = quat.z;
        outlet.push_sample(currentSample);
    }
}
