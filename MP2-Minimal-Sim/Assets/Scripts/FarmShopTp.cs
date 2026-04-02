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

        // Only for testing in XR Interactive Simulator
        Vector3 targetPos = teleportTarget.position;
        targetPos += Vector3.up * (characterController.height / 2f + characterController.skinWidth);
        targetPos -= characterController.center;
        characterController.transform.position = targetPos;

        // characterController.transform.position = teleportTarget.position; // Teleport the player to the target position
        characterController.enabled = true; // Re-enable the character controller after teleportation
    }
}
