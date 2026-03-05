using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
public class FarmShopTp : MonoBehaviour
{
    public Transform teleportTarget;
    public CharacterController characterController;

    public void Teleport()
    {
        characterController.enabled = false; // Disable the character controller to prevent issues during teleportation
        characterController.transform.position = teleportTarget.position; // Teleport the player to the target position
        characterController.enabled = true; // Re-enable the character controller after teleportation
    }
}
