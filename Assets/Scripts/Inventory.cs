using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public GameObject cellContainer;

    public float range = 10f;

    public Camera cam;

    public TMP_Text dialogueText;
    public TMP_Text pickup_hint;
    public TMP_Text interact_hint;

    public int selectedItem;
    public GameObject selectedCell;

    public float timer;

    void Start()
    {
        for (int i = 0; i < cellContainer.transform.childCount; i++)
        {
            items.Add(new Item());
        }
    }

    void Update()
    {

        timer -= Time.deltaTime;

        PickupItem();

        InterctWithNPC();

        SelectItem();

    }

    void PickupItem()
    {
        Ray ray = cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.GetComponent<Item>())
            {
                pickup_hint.enabled = true;

                if (Input.GetKey(KeyCode.E))
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].id == 0)
                        {
                            items[i] = hit.collider.gameObject.GetComponent<Item>();
                            DisplayItems();
                            Destroy(hit.collider.gameObject);
                            break;
                        }
                    }
                }

            }
        }
        else pickup_hint.enabled = false;
    }

    void DisplayItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Transform cell = cellContainer.transform.GetChild(i).transform;

            if (items[i].id != 0 && cell.transform.GetChild(0).name == "Icon_000" ||
                cell.transform.GetChild(0).name == "Icon_000(Clone)")
            {
                Destroy(cell.transform.GetChild(0).gameObject);
                Instantiate(Resources.Load(items[i].pathIcon), cell);
            }
        }
    }

    void SelectItem()
    {

        if (Input.GetKey(KeyCode.Alpha1))
        {
            selectedItem = 0;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            selectedItem = 1;
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            selectedItem = 2;
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            selectedItem = 3;
        }

        selectedCell = cellContainer.transform.GetChild(selectedItem).gameObject;

        selectedCell.GetComponent<Image>().color = Color.yellow;

        for (int i = 0; i < items.Count; i++)
        {
            if (selectedCell.name != cellContainer.transform.GetChild(i).name)
            {
                cellContainer.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
        }


    }

    void InterctWithNPC()
    {
        Ray ray = cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.GetComponent<NPC>())
            {
                interact_hint.enabled = true;

                if (Input.GetKey(KeyCode.E))
                {

                    if (selectedCell.GetComponentInChildren<Item>().id == hit.collider.GetComponent<NPC>().requestItemId && selectedCell != null && !hit.collider.GetComponent<NPC>().completed && timer <= 0)
                    {
                        Destroy(selectedCell.transform.GetChild(0).gameObject);
                        
                        items[selectedItem] = new Item();
                        
                        DisplayItems();
                        
                        dialogueText.text = hit.collider.GetComponent<NPC>().completeQuestText;
                        
                        Instantiate(Resources.Load(hit.collider.GetComponent<NPC>().pathAward), selectedCell.transform);
                        
                        hit.collider.GetComponent<NPC>().completed = true;
                        
                        timer = hit.collider.GetComponent<NPC>().dialogueCooldown;
                    }
                    
                    else if (!hit.collider.GetComponent<NPC>().completed && timer <= 0)
                    {
                        if (hit.collider.GetComponent<NPC>().questText.Length > hit.collider.GetComponent<NPC>().dialoguePhase)
                        {
                            dialogueText.text = hit.collider.GetComponent<NPC>().questText[hit.collider.GetComponent<NPC>().dialoguePhase];

                            hit.collider.gameObject.GetComponent<NPC>().dialoguePhase++;

                            timer = hit.collider.GetComponent<NPC>().dialogueCooldown;
                        }
                        else
                        {
                            hit.collider.gameObject.GetComponent<NPC>().dialoguePhase = 0;

                            dialogueText.text = hit.collider.GetComponent<NPC>().questText[hit.collider.GetComponent<NPC>().dialoguePhase];

                            timer = hit.collider.GetComponent<NPC>().dialogueCooldown;
                        }
                    }
                    else if(hit.collider.GetComponent<NPC>().completed && timer <= 0) dialogueText.text = hit.collider.GetComponent<NPC>().completeQuestText;
                }
            }
        }
        else interact_hint.enabled = false;
    }
}