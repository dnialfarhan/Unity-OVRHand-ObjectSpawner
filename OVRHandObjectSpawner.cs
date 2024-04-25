using UnityEngine;

public class OVRHandObjectSpawner : MonoBehaviour
{
    public GameObject prefab; // The object to spawn
    public float spawnSpeed = 2; // Adjust the spawn speed to your preference
    public float pinchThreshold = 0.7f; // Adjust this threshold to your preference

    [SerializeField] bool isRightHand; // Flag to determine if this is the right hand
    private OVRHand hand; // Reference to the OVRHand component
    private bool hasPinched = false; // Variable to track whether a pinch gesture has already been detected

    void Start()
    {
        hand = GetComponent<OVRHand>(); // Get the OVRHand component attached to this GameObject
    }

    // Update is called once per frame
    void Update()
    {
        // Check if a pinch gesture has been detected and spawn an object if it hasn't already been done
        if (!hasPinched && CheckPinchGesture())
        {
            SpawnObject();
            hasPinched = true; // Set to true once a pinch gesture is detected
        }
        else if (!CheckPinchGesture()) // Reset the flag if pinch gesture is not detected
        {
            hasPinched = false;
        }
    }

    // Check if the hand is performing a pinch gesture
    bool CheckPinchGesture()
    {
        if (hand != null)
        {
            // Check if the hand is pinching
            return hand.IsPressed();
        }
        else
        {
            Debug.LogWarning("OVRHand component not found."); // Log a warning if the OVRHand component is not found
            return false;
        }
    }

    // Spawn an object at the position of the index finger tip
    void SpawnObject()
    {
        if (prefab != null)
        {
            // Get the position of the index finger
            Vector3 spawnPosition = hand.PointerPose.position;

            // Determine the direction to spawn based on whether it's the right hand or left hand
            Vector3 spawnDirection = isRightHand ? transform.forward : -transform.forward;

            // Spawn the object at the position of the index finger
            GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
            Rigidbody spawnedObjectRB = spawnedObject.GetComponent<Rigidbody>();
            if (spawnedObjectRB != null)
            {
                // Give the spawned object an initial velocity to propel it forward
                spawnedObjectRB.velocity = spawnDirection * spawnSpeed;
            }

            // Destroy the spawned object after 5 seconds if it hasn't already been destroyed
            Destroy(spawnedObject, 5);
        }
        else
        {
            Debug.LogWarning("Prefab not assigned for spawning."); // Log a warning if the prefab is not assigned
        }
    }
}
