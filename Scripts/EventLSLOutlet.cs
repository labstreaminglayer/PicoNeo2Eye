using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class EventLSLOutlet : MonoBehaviour
{
    protected liblsl.StreamOutlet outlet;
    protected List<string> outletsName = new List<string>();
    private string[] currentSample;

    public string StreamName = "Unity.GameEvents";
    public string StreamType = "Markers";
    public string StreamId = "MarkersID-Unity1234";

    // appended data to send at the end of frame
    protected List<string> toWriteString = new List<string>();

    protected void OnEnable()
    {
        // Register as a listener for EventController publish events.
        EventController.OnExperimentObjectPublish += OnPublishDefault;

        StartCoroutine(WriteAfterImageIsRendered());
    }

    void Start()
    {
        liblsl.StreamInfo streamInfo = new liblsl.StreamInfo(StreamName, StreamType, 1, liblsl.IRREGULAR_RATE, liblsl.channel_format_t.cf_string, StreamId);
        // TODO: You should probably add XML metadata to the stream so the recipient knows how to interpret the strings.
        currentSample = new string[1];
        outlet = new liblsl.StreamOutlet(streamInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WriteAfterImageIsRendered()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            // Get the same timestamp for all markers on the same frame. 
            double ts = liblsl.local_clock();

            // write and clear buffers
            for (int i = 0; i < toWriteString.Count; i++)
            {
                Write(toWriteString[i], ts);
            }
            toWriteString.Clear();
            yield return null;
        }
    }

    // Automatic timestamping
    public void Write(string pubstring)
    {
        currentSample[0] = pubstring;
        outlet.push_sample(currentSample);
    }
    // Manual timestamping
    public void Write(string pubstring, double timeStamp)
    {
        currentSample[0] = pubstring;
        outlet.push_sample(currentSample, timeStamp);
    }

    public void AppendToWrite(string pubstring)
    {
        toWriteString.Add(pubstring);
    }

    void OnPublishDefault(string pubstring)
    {
        AppendToWrite(pubstring);
        Debug.Log(pubstring);
    }
}
