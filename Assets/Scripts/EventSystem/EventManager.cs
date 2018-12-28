using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class EventInfo {
        public string Description;
    }
    public class TerminalPuzzleInfo : EventInfo {
        public bool completed = true;
        public Grid terminalGrid;
    }
public class EventManager : MonoBehaviour
{

    public delegate void EventResponse(EventInfo eventInfo);

    public enum EVENT_TYPE
    {
        COIN_COLLECTED,
        TERMINAL_ACTIVATED,
        TERMINAL_DEACTIVATED,
        TERMINAL_COMPLETE,
        TERMINAL_INCOMPLETE,
        SHAPE_REMOVED,

    }
    private Dictionary<EVENT_TYPE, List<EventResponse>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init ();
                }
            }

            return eventManager;
        }
    }

    void Init ()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EVENT_TYPE, List<EventResponse>> ();
        }
    }

    public static void StartListening (EVENT_TYPE eventName, EventResponse listener)
    {
        List<EventResponse> thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.Add (listener);
        }
        else
        {
            thisEvent = new List<EventResponse>();
            thisEvent.Add (listener);
            instance.eventDictionary.Add (eventName, thisEvent);
        }
    }

    public static void StopListening (EVENT_TYPE eventName, EventResponse listener)
    {
        if (eventManager == null) return;
        List<EventResponse> thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.Remove (listener);
        }
    }

    public static void TriggerEvent (EVENT_TYPE eventName, EventInfo eventInfo)
    {
        List<EventResponse> thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            foreach (EventResponse er in thisEvent)
            {
                er(eventInfo);
            }
        }
    }
}