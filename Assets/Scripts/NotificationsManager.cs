// EVENTS MANAGER CLASS
// Receives notifications and notifies listeners
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Adding a listener: Inside the Start() function { NotificationsManager.Instance.AddListener(*Object*, *String with event/function name*); }

// Posting a notification: NotificationsManager.Instance.PostNotification(*Object*, *String with event/function name*);

public class NotificationsManager : MonoBehaviour
{
	// START Singleton code
	
	// Public property
	public static NotificationsManager Instance {
		get {
			if (instance == null)
				instance = new NotificationsManager();
			return instance;
		}
	}
	
	// Private variables
	private static NotificationsManager instance = null;
	private Dictionary<string, List<Component>> Listeners = new Dictionary<string, List<Component>>(); // .NET dictionary object
	
	void Awake()
	{
		// Check if there's an existing instance of this object
		if (instance)
			DestroyImmediate(gameObject); // Delete duplicate
		else {
			instance = this; // Make this object the only instance
			DontDestroyOnLoad(gameObject); // Set as do not destroy
		}
	}
	
	// END Singleton code
	
	// Add a listener to the listeners list
	public void AddListener(Component Sender, string NotificationName)
	{
		// Add listener to dictionary
		if (!Listeners.ContainsKey(NotificationName))
			Listeners.Add(NotificationName, new List<Component>());
		
		// Add object to listener list for this notification
		Listeners[NotificationName].Add(Sender);
	}
	
	// Removes a listener from the listeners list
	public void RemoveListener(Component Sender, string NotificationName)
	{
		// If no key in dictionary exists, exit
		if (!Listeners.ContainsKey(NotificationName))
			return;
		
		//Cycle through listeners and indentify component, then remove it
		for (int i = Listeners[NotificationName].Count - 1; i >= 0; i--) {
			if (Listeners[NotificationName][i].GetInstanceID() == Sender.GetInstanceID())
				Listeners[NotificationName].RemoveAt(i);
		}
		
	}
	
	// Post a notification to a listener
	public void PostNotification(Component Sender, string NotificationName)
	{
		// Check if there is a key in dictionary, if not exit
		if (!Listeners.ContainsKey(NotificationName))
			return;
		
		// Else post notification to all matching listeners
		foreach (Component Listener in Listeners[NotificationName])
			Listener.SendMessage(NotificationName, Sender, SendMessageOptions.DontRequireReceiver);
	}
	
	// Clear all listeners
	public void ClearListeners()
	{
		Listeners.Clear();
	}
	
	// Remove redundant listeners
	public void RemoveRedundancies()
	{
		// Create new dictionary
		Dictionary<string, List<Component>> TmpListeners = new Dictionary<string, List<Component>>();
		
		// Cycle through all dictionary entries
		foreach (KeyValuePair<string, List<Component>> Item in Listeners) {
			// Cyle through all listener objects in list, remove null if exists
			for (int i = Item.Value.Count - 1; i >= 0; i--) {
				if (Item.Value[i] == null)
					Item.Value.RemoveAt(i);
			}
			
			// If items remain in list for this notification, thgen add this to tmp dictionary
			if (Item.Value.Count > 0)
				TmpListeners.Add(Item.Key, Item.Value);
		}
		
		// Replace listeners object with new, optimized directory
		Listeners = TmpListeners;
	}
	
}