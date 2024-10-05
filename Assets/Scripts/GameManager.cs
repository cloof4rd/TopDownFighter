using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] characters;  // Array of character prefabs for game use
    public Transform player1SpawnPoint;  // Spawn point for Player 1
    public Transform player2SpawnPoint;  // Spawn point for Player 2

    private void Start()
    {
        // Get the selected character indices from PlayerPrefs
        int selectedCharacterP1 = PlayerPrefs.GetInt("selectedCharacterP1");
        int selectedCharacterP2 = PlayerPrefs.GetInt("selectedCharacterP2");

        // Instantiate Player 1's character at the designated spawn point
        GameObject player1Character = Instantiate(characters[selectedCharacterP1], player1SpawnPoint.position, Quaternion.Euler(0, 0, 0));  // Face right
        player1Character.name = "Player1Character";  // Optional: Rename the character for clarity
        
        player1Character.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        
        // Instantiate Player 2's character at the designated spawn point
        GameObject player2Character = Instantiate(characters[selectedCharacterP2], player2SpawnPoint.position, Quaternion.Euler(0, 180, 0));  // Face left
        player2Character.name = "Player2Character";  // Optional: Rename the character for clarity
        player2Character.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
}
