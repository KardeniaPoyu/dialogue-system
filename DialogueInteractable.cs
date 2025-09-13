using UnityEngine;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    [Header("对话配置")]
    public int dialogueID;  // 对应DialogueData中的ID
    public string interactPrompt = "对话";
    
    [Header("交互设置")]
    public float interactionDistance = 2f;
    public bool facePlayerDuringDialogue = true;
    
    public string InteractionPrompt => interactPrompt;
    public bool CanInteract => true;
    
    public void OnInteract()
    {
        if (DialogueSystem.Instance != null)
        {
            // 启动对话
            DialogueSystem.Instance.StartDialogue(dialogueID);
        }
        else
        {
            Debug.LogWarning("对话系统未初始化");
        }
    }
}