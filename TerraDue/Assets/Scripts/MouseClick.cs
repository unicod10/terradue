using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseClick : MonoBehaviour {

    public Button buildTower;
    public Button castAbility;
    public GameObject player;
    public GameObject[] towerSlots;
    public GameObject humanAbilityParticles;

    private enum State
    {
        Default, BuildTowerClicked, MovingToTowerSlot, AbilityClicked, FightingTarget
    };
    private State state;
    private GameObject selection;

    void Start()
    {
        buildTower.onClick.AddListener(BuildTowerOnClick);
        castAbility.onClick.AddListener(CastAbilityOnClick);
        state = State.Default;
    }

    private void Update()
    {
        if(player == null)
        {
            return;
        }
        // Check if close enough to build
        if (state == State.MovingToTowerSlot)
        {
            if (GetDistance(gameObject, selection) <= Constants.BUILD_MAXIMUM_DISTANCE)
            {
                state = State.Default;
                StopMovement();
                player.GetComponent<PlayerBehaviour>().CmdBuildTower(selection);
            }
        }
        // Check if close enough to attack
        else if (state == State.FightingTarget)
        {
            if (GetDistance(gameObject, selection) <= Constants.ATTACK_MAXIMUM_DISTANCE)
            {
                StopMovement();
                Debug.Log("Attack enemy " + selection.name);
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
                            if (isEnemy(obj))
                            {
                                state = State.FightingTarget;
                                selection = obj;
                                Debug.Log("Attack enemy " + obj.name);
                            }
                            else
                            {
                                state = State.Default;
                                MoveTo(hit.point);
                            }
                            break;
                        case State.BuildTowerClicked:
                            var slot = Array.FindAll(towerSlots, s => s.name == obj.name);
                            if (slot.Length > 0)
                            {
                                state = State.MovingToTowerSlot;
                                selection = slot[0];
                                MoveTo(obj.transform.position);
                            }
                            else
                            {
                                state = State.Default;
                            }
                            break;
                        case State.AbilityClicked:
                            if(isEnemy(obj))
                            {
                                player.GetComponent<PlayerBehaviour>().CmdCastAbility(obj);
                            }
                            state = State.Default;
                            break;
                    }
                }
            }
        }
    }

    private void BuildTowerOnClick()
    {
        StopMovement();
        state = State.BuildTowerClicked;
    }

    private void CastAbilityOnClick()
    {
        StopMovement();
        state = State.AbilityClicked;
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

    private bool isEnemy(GameObject selection)
    {
        return player.tag == "Human" && selection.tag == "Alien" || player.tag == "Alien" && selection.tag == "Human";
    }

    private bool IsHuman()
    {
        return player.tag == "Human";
    }
}
