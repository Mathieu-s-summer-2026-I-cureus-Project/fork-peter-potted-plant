using UnityEngine;
//Mathieu's code for camera rigging, this script is attached to the camera and follows the player with a vertical offset and adjusts the orthographic size based on the player's health.
public class rigCamera : MonoBehaviour
{

    public GameObject player;
    public playerStateManager PlayerState;
    public Camera mainCamera;

    Vector3 playerPosition;
    public float verticalOffset = -15.0f;
    float cameraDistance = 12.0f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        assignObjects();
        mainCamera.orthographicSize = cameraDistance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float healthBasedOffset = (PlayerState.health / 25.0f);
        playerPosition = player.transform.position;
        mainCamera.orthographicSize = cameraDistance + healthBasedOffset;
        //mainCamera.orthographicSize = cameraDistance - (PlayerState.health / 25.0f)*-1.0f;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
    }

    void assignObjects()
    {
        if (player == null) Debug.LogError("Player GameObject not found. Please ensure there is a GameObject named 'peterHead' in the scene.");
        if (PlayerState == null) Debug.LogError("playerStateManager script not found on 'peterHead' GameObject.");
        if (mainCamera == null) Debug.LogError("Main Camera not found in the scene.");

    }
}