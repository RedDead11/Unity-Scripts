using UnityEngine;

public class SaveGame : MonoBehaviour
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
        // Save player position to PlayerPrefs
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.position.z);
    }

    private void LoadPlayerPosition()
    {
        // Load player position from PlayerPrefs
        float playerX = PlayerPrefs.GetFloat("PlayerX");
        float playerY = PlayerPrefs.GetFloat("PlayerY");
        float playerZ = PlayerPrefs.GetFloat("PlayerZ");

        // Set the player's position
        player.position = new Vector3(playerX, playerY, playerZ);
    }
}
