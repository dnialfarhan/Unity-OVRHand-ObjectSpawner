using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public GameObject prefab;
    public float spawnSpeed = 5;
    public float pinchThreshold = 0.7f; // Adjust this threshold to your preference

    [SerializeField] bool isRightHand;
    private OVRHand hand;
    private bool hasPinched = false; // Variable to track whether a pinch gesture has already been detected

    void Start()
    {
        hand = GetComponent<OVRHand>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPinched && CheckPinchGesture())
        {
            SpawnNewBall();
            hasPinched = true; // Set to true once a pinch gesture is detected
        }
        else if (!CheckPinchGesture()) // Reset the flag if pinch gesture is not detected
        {
            hasPinched = false;
        }
    }

    bool CheckPinchGesture()
    {
        if (hand != null)
        {
            // Check if the hand is pinching
            return hand.IsPressed();
        }
        else
        {
            Debug.LogWarning("OVRHand not found.");
            return false;
        }
    }

    void SpawnNewBall()
    {
        if (prefab != null)
        {
            // Get the position of the index finger
            Vector3 spawnPosition = hand.PointerPose.position;

            // Determine the direction to spawn based on whether it's the right hand or left hand
            Vector3 spawnDirection = isRightHand ? transform.forward : -transform.forward;

            // Spawn the ball at the position of the index finger
            GameObject spawnedBall = Instantiate(prefab, spawnPosition, Quaternion.identity);
            Rigidbody spawnedBallRB = spawnedBall.GetComponent<Rigidbody>();
            if (spawnedBallRB != null)
            {
                spawnedBallRB.velocity = spawnDirection * spawnSpeed;
            }

            // Destroy the spawned ball after 5 seconds if it hasn't already been destroyed
            Destroy(spawnedBall, 5);
        }
        else
        {
            Debug.LogWarning("Prefab not assigned for spawning.");
        }
    }
}
