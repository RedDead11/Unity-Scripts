using UnityEngine;

public class SaveGame_v2 : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        // Load player position when the game starts
        LoadPlayerPosition();
    }

    private void Update()
    {
        // Save player position at regular intervals (e.g., every few seconds)
        if (Input.GetKeyDown(KeyCode.O)) // You can change this condition to your saving logic
        {
            SavePlayerPosition();
        }
    }

    private void SavePlayerPosition()
    {
        // Serialize player position data into JSON string
        string playerPositionJson = JsonUtility.ToJson(player.position);

        // Save serialized position data to PlayerPrefs
        PlayerPrefs.SetString("PlayerPosition", playerPositionJson);
    }

    private void LoadPlayerPosition()
    {
        // Check if player position data exists in PlayerPrefs
        if (PlayerPrefs.HasKey("PlayerPosition"))
        {
            // Retrieve serialized position data from PlayerPrefs
            string playerPositionJson = PlayerPrefs.GetString("PlayerPosition");

            // Deserialize position data from JSON string
            Vector3 playerPosition = JsonUtility.FromJson<Vector3>(playerPositionJson);

            // Set the player's position
            player.position = playerPosition;
        }
    }
}
