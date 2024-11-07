using UnityEngine;

public class RewardSignaler : MonoBehaviour
{
    public JugglingRobotAgent agent;
    private void OnTriggerStay()
    {
        if (agent != null)
        {
            Debug.Log("WORKING");
            agent.RewardTouched();
        }
    }
}
