using UnityEngine;
using UnityEngine.Networking;

public class TowersManager : NetworkBehaviour {

    public GameObject humanTowerPrefab;
    public GameObject alienTowerPrefab;

    private const int Lanes = 3;
    private const int SlotsPerLane = 4;
    enum SlotState
    {
        Empty, Human, Alien
    }
    private SlotState[,] slots;
    private bool started;

    void Start() {
        if(!isServer)
        {
            return;
        }
        slots = new SlotState[Lanes, SlotsPerLane];
        slots[0, 0] = SlotState.Empty;
        slots[0, 1] = SlotState.Empty;
        slots[0, 2] = SlotState.Empty;
        slots[0, 3] = SlotState.Empty;
        slots[1, 0] = SlotState.Empty;
        slots[1, 1] = SlotState.Empty;
        slots[1, 2] = SlotState.Empty;
        slots[1, 3] = SlotState.Empty;
        slots[2, 0] = SlotState.Empty;
        slots[2, 1] = SlotState.Empty;
        slots[2, 2] = SlotState.Empty;
        slots[2, 3] = SlotState.Empty;
        BuildTower(GameObject.Find("TowerSlot_0_0"), true);
        BuildTower(GameObject.Find("TowerSlot_0_1"), true);
        BuildTower(GameObject.Find("TowerSlot_0_3"), false);
        BuildTower(GameObject.Find("TowerSlot_0_2"), false);
        BuildTower(GameObject.Find("TowerSlot_1_0"), true);
        BuildTower(GameObject.Find("TowerSlot_1_1"), true);
        BuildTower(GameObject.Find("TowerSlot_1_3"), false);
        BuildTower(GameObject.Find("TowerSlot_1_2"), false);
        BuildTower(GameObject.Find("TowerSlot_2_0"), true);
        BuildTower(GameObject.Find("TowerSlot_2_1"), true);
        BuildTower(GameObject.Find("TowerSlot_2_3"), false);
        BuildTower(GameObject.Find("TowerSlot_2_2"), false);
    }

    public bool CanBuild(string slotName, bool human)
    {
        var lane = GetLane(slotName);
        var slot = GetSlot(slotName);

        // The slot is busy
        if(slots[lane, slot] != SlotState.Empty)
        {
            return false;
        }

        if(human)
        {
            // This is the first slot
            if(slot == 0)
            {
                return true;
            }
            // The previous slot in the lane is human
            if(slots[lane, slot - 1] == SlotState.Human)
            {
                return true;
            }
        }
        else
        {
            if(slot == 3)
            {
                return true;
            }
            if(slots[lane, slot + 1] == SlotState.Alien)
            {
                return true;
            }
        }

        return false;
    }

    public void BuildTower(GameObject slot, bool human)
    {
        // The tower wouldn't be linked
        if(!CanBuild(slot.name, human))
        {
            return;
        }
        // Load the tower prefab and create
        GameObject prefab;
        prefab = human ? humanTowerPrefab : alienTowerPrefab;
        GameObject instance = Instantiate(prefab, slot.transform);
        instance.GetComponent<TowerBehaviour>().slot = slot;
        // Register it here and spawn
        slots[GetLane(slot.name), GetSlot(slot.name)] = human ? SlotState.Human : SlotState.Alien;
        NetworkServer.Spawn(instance);
    }

    public void TowerDestroyed(string slotName)
    {
        slots[GetLane(slotName), GetSlot(slotName)] = SlotState.Empty;
    }

    public bool IsTowerVulnable(string slotName, bool human)
    {
        var lane = GetLane(slotName);
        var slot = GetSlot(slotName);

        if(human)
        {
            // Furthest slot
            if(slot == 3)
            {
                return true;
            }
            // Furthest human slot
            if(slots[lane, slot + 1] != SlotState.Human)
            {
                return true;
            }
        }
        else
        {
            if(slot == 0)
            {
                return true;
            }
            if(slots[lane, slot - 1] != SlotState.Alien)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsBaseVunlnable(bool human)
    {
        if(human)
        {
            return
                slots[0, 0] != SlotState.Human ||
                slots[1, 0] != SlotState.Human ||
                slots[2, 0] != SlotState.Human;
        }
        else
        {
            return
                slots[0, 3] != SlotState.Alien ||
                slots[1, 3] != SlotState.Alien ||
                slots[2, 3] != SlotState.Alien;
        }
    }

    private int GetLane(string slotName)
    {
        return int.Parse(slotName.Split('_')[1]);
    }

    private int GetSlot(string slotName)
    {
        return int.Parse(slotName.Split('_')[2]);
    }
}
