using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class FarmShopTp : MonoBehaviour
{
    public Transform teleportTarget;
    public CharacterController characterController;
    public Transform cameraTransform;

    public void Teleport()
    {
        if (teleportTarget == null || characterController == null)
        {
            return;
        }

        Transform cam = cameraTransform != null ? cameraTransform : Camera.main != null ? Camera.main.transform : null;

        // Move the rig so the user's head (camera) ends up exactly at teleportTarget.
        if (cam != null)
        {
            Vector3 headOffset = cam.position - characterController.transform.position;
            headOffset.y = 0f;

            characterController.enabled = false;
            characterController.transform.position = teleportTarget.position - headOffset;
            characterController.enabled = true;
            return;
        }

        // Fallback for non-XR setups.
        characterController.enabled = false;
        characterController.transform.position = teleportTarget.position;
        characterController.enabled = true;
    }
}
