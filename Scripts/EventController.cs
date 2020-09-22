///<summary>
/// This script handles all the information exchange across tasks. Since we are
/// now sub-classing, we can't rely on static instance references or pointers in the scripts
/// to hold true across all sub-classes possibilities. Using a static EventController instance
/// which is not sub-classable will take care of proper data flow and references. 
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    #region Access

    public static EventController instance = null;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion Access
    
    #region Events
    // Experiment Object Events
    public delegate void ExperimentObjectPublish(string pubString);
    public static event ExperimentObjectPublish OnExperimentObjectPublish;
    public void SendObjectState(string pubString)
    {
        OnExperimentObjectPublish?.Invoke(pubString);
    }
    #endregion Events
}
