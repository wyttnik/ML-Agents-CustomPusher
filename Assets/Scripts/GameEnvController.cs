using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class GameEnvController : MonoBehaviour
{
    public int buttonsOnEpisode = 4;
    public int boxesOnEpisode = 3;
    //public int obstacles1OnEpisode = 3;
    //public int obstacles2OnEpisode = 3;

    private Agent agent;
    public GridedDistributor buttonsDistributor;
    public GridedDistributor boxDistributor;
    public GridedDistributor agentsDistributor;
    //public GridedDistributor obstacle1Distributor;
    //public GridedDistributor obstacle2Distributor;
    public Door door;
    public MeshCollider goal;
    bool wasOpenedBefore;

    void Start()
    {
        ResetScene();
    }

    void ResetScene()
    {
        var buttons = buttonsDistributor.Respawn(buttonsOnEpisode);
        boxDistributor.Respawn(boxesOnEpisode);
        //obstacle1Distributor.Respawn(obstacles1OnEpisode);
        //obstacle2Distributor.Respawn(obstacles2OnEpisode);

        var activators = new DoorActivator[buttons.Length];
        for (var i = 0; i < buttons.Length; i++)
            activators[i] = buttons[i].GetComponent<Button>();
        door.ResetActivators(activators);
        wasOpenedBefore = false;
        goal.gameObject.SetActive(false);

        agent = agentsDistributor.Respawn(1)[0].GetComponent<Agent>();
    }

    public void OnGoalTriggered()
    {
        agent.AddReward(5f);
        agent.EndEpisode();
        ResetScene();
    }

    public void ActivateDoor()
    {
        if (!wasOpenedBefore)
        {
            agent.AddReward(1f);
            wasOpenedBefore = true;
            goal.gameObject.SetActive(true);
        }
        else
        {
            agent.AddReward(-1f);
        }
        
    }

    void FixedUpdate()
    {
    }
}
