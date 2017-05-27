using System;
using UnityEngine;
using UnityEngine.UI;

public class UserInteraction : MonoBehaviour {

    public Button buildTower;
    public Button castAbility;
    public GameObject player;
    public GameObject[] towerSlots;

    private enum State
    {
        Default, BuildTowerPressed, MovingToTowerSlot, AbilityPressed, FightingTarget
    };
    private State state;
    private GameObject selection;
    private float lastAttacked;
    private float abilityLastUsed;
    private float startedMovingSince;
    private Text statusBar;
    const string DefaultStatus = "Click an enemy or a position, or press (b) or (1)";

    void Start()
    {
        state = State.Default;
        lastAttacked = Constants.ATTACK_EACH;
        abilityLastUsed = -1;
        startedMovingSince = -1;
        statusBar = GameObject.Find("UI/Panel/StatusBar").GetComponent<Text>();
        statusBar.text = DefaultStatus;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }
        if(player.GetComponent<LifeBehaviour>().IsDead())
        {
            player.GetComponent<MoveToPoint>().Hide();
            state = State.Default;
            statusBar.text = "Dead";
            return;
        }
        // Moving to position message
        if(startedMovingSince >= 0)
        {
            if (startedMovingSince > 0.1 && DestinationReached())
            {
                statusBar.text = DefaultStatus;
                startedMovingSince = -1;
            }
            else
            {
                startedMovingSince += Time.deltaTime;
            }
        }
        // Reload ability if time
        if (abilityLastUsed >= 0)
        {
            abilityLastUsed += Time.deltaTime;
            if(abilityLastUsed >= Constants.ABILITY_COOLDOWN)
            {
                abilityLastUsed = -1;
            }
        }
        if(Input.GetKeyDown("1"))
        {
            if (abilityLastUsed < 0)
            {
                state = State.AbilityPressed;
                statusBar.text = "Click on an enemy target";
            }
            else
            {
                int remaining = (int)Mathf.Max(1, Mathf.Round(Constants.ABILITY_COOLDOWN - abilityLastUsed));
                statusBar.text = "Ability is cooling down.. " + remaining + "s left";
            }
        }
        else if(Input.GetKeyDown("b"))
        {
            state = State.BuildTowerPressed;
            statusBar.text = "Click on a free, linked slot";
        }
        // Check if close enough to build
        if (state == State.MovingToTowerSlot)
        {
            if (GetDistance(gameObject, selection) <= Constants.BUILD_MAXIMUM_DISTANCE)
            {
                state = State.Default;
                StopMovement();
                player.GetComponent<PlayerBehaviour>().CmdBuildTower(selection);
                statusBar.text = DefaultStatus;
            }
        }
        // Check if close enough to attack
        else if (state == State.FightingTarget)
        {
            if (selection == null || selection.GetComponent<LifeBehaviour>().IsDead())
            {
                selection = null;
                state = State.Default;
                statusBar.text = DefaultStatus;
                return;
            }

            if (GetDistance(player, selection) <= Constants.ATTACK_MAXIMUM_DISTANCE)
            {
                lastAttacked += Time.deltaTime;
                if(lastAttacked >= Constants.ATTACK_EACH)
                {
                    StopMovement();
                    player.GetComponent<PlayerBehaviour>().CmdAttack(selection);
                    lastAttacked = 0;
                }
            }
            else
            {
                MoveTo(selection.transform.position);
            }
        }
        if(Input.GetMouseButtonDown(0) && player.GetComponent<PlayerBehaviour>().IsAlive())
        {
            // Find the clicked object
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform != null)
                {
                    var obj = hit.transform.gameObject;
                    switch (state)
                    {
                        case State.Default:
                        case State.MovingToTowerSlot:
                        case State.FightingTarget:
                            if (IsEnemy(obj))
                            {
                                state = State.FightingTarget;
                                selection = obj;
                                statusBar.text = "Attacking enemy";
                            }
                            else
                            {
                                state = State.Default;
                                MoveTo(hit.point);
                                statusBar.text = "Moving to position";
                                startedMovingSince = 0;
                            }
                            break;
                        case State.BuildTowerPressed:
                            var slot = Array.FindAll(towerSlots, s => s.name == obj.name);
                            if (slot.Length > 0)
                            {
                                state = State.MovingToTowerSlot;
                                selection = slot[0];
                                MoveTo(obj.transform.position);
                                statusBar.text = "Building tower";
                            }
                            else
                            {
                                state = State.Default;
                                statusBar.text = DefaultStatus;
                            }
                            break;
                        case State.AbilityPressed:
                            if(IsEnemy(obj))
                            {
                                player.GetComponent<PlayerBehaviour>().CmdCastAbility(obj);
                                abilityLastUsed = 0;
                            }
                            state = State.Default;
                            statusBar.text = DefaultStatus;
                            break;
                    }
                }
            }
        }
    }

    private void MoveTo(Vector3 point)
    {
        player.GetComponent<MoveToPoint>().MoveTo(point);
    }

    private void StopMovement()
    {
        player.GetComponent<MoveToPoint>().StopMovement();
    }

    private float GetDistance(GameObject obj1, GameObject obj2)
    {
        return Vector3.Distance(obj1.transform.position, obj2.transform.position);
    }

    private bool IsEnemy(GameObject selection)
    {
        return 
            player.tag == "Human" && selection.tag == "Alien" ||
            player.tag == "Human" && selection.tag == "Monster" ||
            player.tag == "Alien" && selection.tag == "Human" ||
            player.tag == "Alien" && selection.tag == "Monster";
    }

    private bool IsHuman()
    {
        return player.tag == "Human";
    }

    private bool DestinationReached()
    {
        return !player.GetComponent<MoveToPoint>().IsMoving();
    }
}
