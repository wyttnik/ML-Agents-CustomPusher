using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentPusher : Agent
{
    float m_LateralSpeed = 0.15f;
    float m_ForwardSpeed = 0.5f;
    public Door doorObs;
    //public GoalTrigger goalObject;

    [HideInInspector]
    public Rigidbody agentRb;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall")) AddReward(-1f);
    }

    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;


        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        agentRb.AddForce(dirToGo, ForceMode.VelocityChange);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(doorObs.GetState());
        //sensor.AddObservation((transform.position - goalObject.transform.position).magnitude);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);

        AddReward(-1f / MaxStep);

        //if (doorObs.GetState())
        //{
        //    AddReward(1f / ((transform.position - goalObject.transform.position).magnitude + 1f));
        //}
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
    }

    public override void OnEpisodeBegin()
    {
    }

}
