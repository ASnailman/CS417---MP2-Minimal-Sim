using UnityEngine;

public class EaseIn : MonoBehaviour
{
    public float k = 8f; 
    
    private Vector3 defaultScale = Vector3.one;
    private Vector3 goalScale = Vector3.one;

    void Update()
    {
        // v = k * (goal - x) * dt 
        Vector3 velocity = k * (goalScale - transform.localScale) * Time.deltaTime;
        
        transform.localScale += velocity;

        if (goalScale != defaultScale)
        {
            goalScale = Vector3.Lerp(goalScale, defaultScale, Time.deltaTime * 5f);
        }
    }

    public void Pulse()
    {
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
    }
}