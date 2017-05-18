using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseClick : MonoBehaviour {

    public Button buildTower;
    public Button castAbility;
    public GameObject player;
    public GameObject[] towerSlots;

    private enum SelectionMode
    {
        None, TowerSlot, MovingToSlot, Target
    };
    private SelectionMode selection;

    void Start()
    {
        buildTower.onClick.AddListener(BuildTowerOnClick);
        castAbility.onClick.AddListener(CastAbilityOnClick);
        selection = SelectionMode.None;
    }

    private void Update()
    {
        if(player == null)
        {
            return;
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
                    switch (selection)
                    {
                        // Move to position
                        case SelectionMode.None:
                            MoveTo(hit.point);
                            break;
                        // Tower slot selected
                        case SelectionMode.TowerSlot:
                            var slot = Array.FindAll(towerSlots, s => s.name == obj.name);
                            if (slot.Length > 0)
                            {
                                // TODO check on the server
                                MoveTo(obj.transform.position);
                                Debug.Log("Build tower at " + slot[0].name);
                            }
                            break;
                        // Cast target selected
                        case SelectionMode.Target:
                            if(player.tag == "Human" && obj.tag == "Alien")
                            {
                                Debug.Log("Attack alien enemy");
                            }
                            else if(player.tag == "Alien" && obj.tag == "Human")
                            {
                                Debug.Log("Attack human enemy");
                            }
                            break;
                    }
                }
                selection = SelectionMode.None;
            }
        }
    }

    void BuildTowerOnClick()
    {
        StopMovement();
        selection = SelectionMode.TowerSlot;
    }

    void CastAbilityOnClick()
    {
        StopMovement();
        selection = SelectionMode.Target;
    }

    private void MoveTo(Vector3 point)
    {
        player.GetComponent<MoveToPoint>().MoveTo(point);
    }

    private void StopMovement()
    {
        player.GetComponent<MoveToPoint>().StopMovement();
    }
}
