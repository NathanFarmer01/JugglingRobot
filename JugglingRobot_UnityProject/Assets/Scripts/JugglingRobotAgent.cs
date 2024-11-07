using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class JugglingRobotAgent : Agent
{
    public const int numArmSegments = 3;
    public GameObject targetReward;
    public GameObject[] armSegments = new GameObject[numArmSegments];
    private ConfigurableJoint[] joints = new ConfigurableJoint[numArmSegments];
    private Quaternion[] initialJointRotations = new Quaternion[numArmSegments];

    public override void Initialize()
    {
        for (int i = 0; i < numArmSegments; i++)
        {
            joints[i] = armSegments[i].GetComponent<ConfigurableJoint>();
            initialJointRotations[i] = joints[i].transform.localRotation;
        }   
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode Begin");
        for (int i = 0; i < numArmSegments; i++)
        {
            joints[i].transform.localRotation = initialJointRotations[i];
        }  

        //targetReward.transform.localPosition = new Vector3(Random.Range(-1.0f, 0.0f), 1.35f, Random.Range(.75f, 1.5f));
        //targetReward.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        //targetReward.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        for (int i = 0; i < numArmSegments; i++)
        {
            sensor.AddObservation(armSegments[i].transform.localPosition);
            sensor.AddObservation(armSegments[i].transform.localRotation);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int i = -1;
        float xLimit = joints[0].lowAngularXLimit.limit;
        float yLimit = joints[0].angularYLimit.limit;
        joints[0].targetRotation = Quaternion.Euler(actions.ContinuousActions[++i] * xLimit, actions.ContinuousActions[++i] * yLimit, 0);
        xLimit = joints[1].lowAngularXLimit.limit;
        joints[1].targetRotation = Quaternion.Euler(actions.ContinuousActions[++i] * xLimit, 0, 0);
        xLimit = joints[2].lowAngularXLimit.limit;
        yLimit = joints[2].angularYLimit.limit;
        joints[2].targetRotation = Quaternion.Euler(actions.ContinuousActions[++i] * xLimit, actions.ContinuousActions[++i] * yLimit, 0);
    }
    public void RewardTouched()
    {
        Debug.Log("Reward Touched");
        AddReward(1 / this.MaxStep);

        if (this.GetCumulativeReward() > 0.2)
        {
            Debug.Log("Ball Caught!");
        }
    }
}
