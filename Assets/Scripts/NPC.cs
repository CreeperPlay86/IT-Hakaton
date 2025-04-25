using UnityEngine;

public class NPC : MonoBehaviour
{
    public int requestItemId;
    public string[] questText;
    public string completeQuestText;
    public bool completed = false;
    public string pathAward;
    public int dialoguePhase = 0;
    public float dialogueCooldown = 0;
}
