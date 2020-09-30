using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;
using LSL;

public class GazeLSLOutlet : MonoBehaviour
{
    private liblsl.StreamOutlet outlet;
    private float[] currentSample;

    public string StreamName = "Unity.TobiiGaze";
    public string StreamType = "Gaze";
    public string StreamId = "TabiiGazeID-Unity1234";

    // Start is called before the first frame update
    void Start()
    {
        liblsl.StreamInfo streamInfo = new liblsl.StreamInfo(StreamName, StreamType, 12, 1 / Time.fixedDeltaTime, liblsl.channel_format_t.cf_float32, StreamId);
        liblsl.XMLElement chans = streamInfo.desc().append_child("channels");
        chans.append_child("channel").append_child_value("label", "Timestamp");
        chans.append_child("channel").append_child_value("label", "ConvergenceDistance");
        chans.append_child("channel").append_child_value("label", "ConvergenceDistanceIsValid");
        chans.append_child("channel").append_child_value("label", "GazeRayOriginX");
        chans.append_child("channel").append_child_value("label", "GazeRayOriginY");
        chans.append_child("channel").append_child_value("label", "GazeRayOriginZ");
        chans.append_child("channel").append_child_value("label", "GazeRayDirectionX");
        chans.append_child("channel").append_child_value("label", "GazeRayDirectionY");
        chans.append_child("channel").append_child_value("label", "GazeRayDirectionZ");
        chans.append_child("channel").append_child_value("label", "GazeRayIsValid");
        chans.append_child("channel").append_child_value("label", "IsLeftEyeBlinking");
        chans.append_child("channel").append_child_value("label", "IsRightEyeBlinking");
        outlet = new liblsl.StreamOutlet(streamInfo);
        currentSample = new float[12];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);

        currentSample[0] = eyeTrackingData.Timestamp;
        currentSample[1] = eyeTrackingData.ConvergenceDistance;
        currentSample[2] = System.Convert.ToSingle(eyeTrackingData.ConvergenceDistanceIsValid);
        currentSample[3] = eyeTrackingData.GazeRay.Origin.x;
        currentSample[4] = eyeTrackingData.GazeRay.Origin.y;
        currentSample[5] = eyeTrackingData.GazeRay.Origin.z;
        currentSample[6] = eyeTrackingData.GazeRay.Direction.x;
        currentSample[7] = eyeTrackingData.GazeRay.Direction.y;
        currentSample[8] = eyeTrackingData.GazeRay.Direction.z;
        currentSample[9] = System.Convert.ToSingle(eyeTrackingData.GazeRay.IsValid);
        currentSample[10] = System.Convert.ToSingle(eyeTrackingData.IsLeftEyeBlinking);
        currentSample[11] = System.Convert.ToSingle(eyeTrackingData.IsRightEyeBlinking);
        outlet.push_sample(currentSample);
    }
}
