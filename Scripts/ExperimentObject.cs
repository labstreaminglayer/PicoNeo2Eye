using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using Tobii.G2OM;


[Serializable]
public class ExperimentObject : MonoBehaviour, IGazeFocusable
{
    // Properties
    [HideInInspector]
    [SerializeField]
    protected bool _hasFocus = false;
    public bool HasFocus
    {
        get { return _hasFocus; }
        set
        {
            _hasFocus = value;
            Publish();
        }

    }
    [SerializeField] 
    protected string _identity = "name";
    public string Identity
    {
        get { return _identity; }
        set
        {
            _identity = value;
            Publish();
        }
    }

    [HideInInspector]
    [SerializeField]
    protected Vector3 _position = new Vector3();
    public Vector3 Position
    {
        get { return GetComponent<Transform>().localPosition; }
        set
        {
            
            GetComponent<Transform>().localPosition = value;
            _position = value;
            Publish();
        }
    }

    [HideInInspector]
    [SerializeField]
    protected Vector3 _pointingTo = new Vector3();
    public Vector3 PointingTo
    {
        get { return _pointingTo; }
        set
        {
            Transform tf = GetComponent<Transform>();
            tf.rotation = Quaternion.identity;
            tf.LookAt(value);
            _pointingTo = value;
            Publish();
        }
    }

    private void Awake()
    {
        _position = GetComponent<Transform>().localPosition;
    }

    public void GazeFocusChanged(bool hasFocus)
    {
        HasFocus = hasFocus;
    }

    public void Publish()
    {
        string pubstring = "{\"ObjectInfo\": " + JsonUtility.ToJson(this) + "}";
        // Generate a JSON string representing current state and emit via OnPublish
        EventController.instance.SendObjectState(pubstring);
    }
}