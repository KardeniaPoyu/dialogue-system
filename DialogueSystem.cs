using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI; 

public class DialogueSystem : MonoBehaviour
{
    private static DialogueSystem instance;
    public static DialogueSystem Instance => instance;
    [Tooltip("对话面板")]public DialoguePanel dialoguePanel;
    [Tooltip("对话数据列表")]public DialogueDataListSO dialogueDataListSO;
    [Tooltip("当前处于第几句对话")]public int currentSpeechCount;
    private DialogueData dialogueData;//从对话数据列表中获得单个对话数据

    [Header("立绘颜色设置")]           
    [Tooltip("说话者立绘颜色")] 
    public Color activeSpeakerColor = Color.white;

    [Tooltip("非说话者立绘颜色")] 
    public Color inactiveSpeakerColor = new Color(0.5f, 0.5f, 0.5f, 0.8f); // 半透明的灰色

    [Tooltip("是否保持非说话者立绘可见")]
    public bool keepInactiveVisible = true; // true=保持显示但变暗, false=完全隐藏

    public bool isDialogueEnd;//对话是否结束
    public static event Action OnDialogueEnd; // 对话结束事件 
    public bool IsDialogueActive => !isDialogueEnd && dialoguePanel.gameObject.activeSelf;
    
    [Header("UI 组件")]
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public Image speakerImage;
    public Button continueButton;

   [Header("打字机效果")]
    public float typingSpeed = 0.05f; 
    private Coroutine typingCoroutine;

    [Header("打字机音效")]
    public AudioClip typingSound;  // 新增的音频变量
    private Coroutine typingRoutine; // 新增的协程引用变量
    private bool isTyping = false;  // 打字状态标识
    private AudioSource audioSource; // 专用音频组件

    private void Awake() 
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        dialogueData = dialogueDataListSO.dialogueDataList[0];//从存储的对话的第一个开始

        dialogueDataListSO.Initialization();//初始化字典
        
        // 初始化时确保两个立绘都激活
        dialoguePanel.rowImageLeft.gameObject.SetActive(true);
        dialoguePanel.rowImageRight.gameObject.SetActive(true);
		SyncDialogueInformation(currentSpeechCount);
        
	}
    
    
    public void SyncDialogueInformation(int speechCount)//同步对话信息
    {
        instance.dialoguePanel.speakerNameLeft.text = instance.dialogueData.speakerNameLeft;
        instance.dialoguePanel.speakerNameRight.text = instance.dialogueData.speakerNameRight;

        instance.dialoguePanel.rowImageLeft.sprite = instance.dialogueData.speakerLeft;
        instance.dialoguePanel.rowImageRight.sprite = instance.dialogueData.speakerRight;
        
        //dialoguePanel.speech.text = dialogueData.dialogueList[speechCount].speech;

        // 启动打字机效果
        typingCoroutine = StartCoroutine(TypeText(
            dialogueData.dialogueList[speechCount].speech
        ));
        
    }
        private IEnumerator TypeText(string text)
    {
        // 确保UI准备就绪
        yield return new WaitForEndOfFrame(); 
        
        isTyping = true;
        dialoguePanel.speech.text = "";
        
            foreach (char c in text)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            
            dialoguePanel.speech.text += c;
            
            // 播放打字音效
            if (typingSound != null && c != ' ')
            {
                audioSource.PlayOneShot(typingSound);
            }
            
            // 根据标点符号暂停
            float delay = ",.?!".Contains(c) ? typingSpeed * 5 : typingSpeed;
            yield return new WaitForSecondsRealtime(delay);
        }
            
        
        isTyping = false;
    }

    public static void UpdateDialogueSpeech()//更新对话，对话存储在列表中，点击继续将对应的下标+1
    {
        
        if (instance == null || instance.isDialogueEnd)
        {
            Debug.LogError("DialogueSystem 实例未初始化或对话结束！");
            return;// 如果对话已结束，不再更新
        }
        Debug.Log($"当前对话索引: {instance.currentSpeechCount}, 总对话数: {instance.dialogueData.dialogueList.Count}");
       
        // 先检查当前节点是否是结束节点
        var currentDialogue = instance.dialogueData.dialogueList[instance.currentSpeechCount];
        if (currentDialogue.isEndNode)
        {
            instance.EndDialogue();
            return;
        }

         // 检查当前对话是否需要添加日志
        instance.CheckForLogAddition();

        instance.currentSpeechCount++;//下标+1

        // 检查是否到达终点
        if (instance.currentSpeechCount >= instance.dialogueData.dialogueList.Count)
        {
           instance.currentSpeechCount = 0; 
        }
        

        Debug.Log($"更新后的对话内容: {instance.dialogueData.dialogueList[instance.currentSpeechCount].speech}");
        
        instance.SyncDialogueInformation(instance.currentSpeechCount);   
        instance.InstanceChooseBtn(instance.dialoguePanel.chooseParent);        
        
        // 更新立绘显示状态
        instance.UpdateSpeakerVisualState();
    }
    /// <summary>
    /// 更新立绘视觉状态（新添加的方法）
    /// 实现说话者正常显示，非说话者变暗的效果
    /// </summary>
    private void UpdateSpeakerVisualState()
    {
        Row currentRow = dialogueData.dialogueList[currentSpeechCount].row;
        
        if (keepInactiveVisible)
        {
            // 模式1：保持两个立绘都可见，通过颜色区分
            dialoguePanel.rowImageLeft.gameObject.SetActive(true);
            dialoguePanel.rowImageRight.gameObject.SetActive(true);
            
            // 设置颜色：说话者正常，非说话者变暗
            dialoguePanel.rowImageLeft.color = (currentRow == Row.left) ? activeSpeakerColor : inactiveSpeakerColor;
            dialoguePanel.rowImageRight.color = (currentRow == Row.right) ? activeSpeakerColor : inactiveSpeakerColor;
        }
        else
        {
            // 模式2：原始模式，只显示说话者立绘
            dialoguePanel.rowImageLeft.gameObject.SetActive(currentRow == Row.left);
            dialoguePanel.rowImageRight.gameObject.SetActive(currentRow == Row.right);
            
            // 确保显示的立绘颜色正常
            if (currentRow == Row.left)
                dialoguePanel.rowImageLeft.color = activeSpeakerColor;
            else
                dialoguePanel.rowImageRight.color = activeSpeakerColor;
        }
    }
    
    public void InstanceChooseBtn(Transform transform)//生成选项
    {
        ChooseButton chooseButton;
        if (dialoguePanel.chooseButtonPre != null) // 判空逻辑
        {
            for (int i = 0; i < dialogueData.dialogueList[currentSpeechCount].chooseList.Count; i++)
            {
                chooseButton = Instantiate(dialoguePanel.chooseButtonPre, transform);
                chooseButton.chooseText.text = dialogueData.dialogueList[currentSpeechCount].chooseList[i].chooseBtnText;

                chooseButton.ID = dialogueData.dialogueList[currentSpeechCount].chooseList[i].ID;
				//将选项按钮中的ID同步于当前对话中的选项的ID
                dialoguePanel.continueSpeechBtn.gameObject.SetActive(false);//生成选项时关闭继续按钮
            }
        } 
        else    
        {
            Debug.LogError("chooseButtonPre 为空");
        }
    }
    //原理：循环当前对话内的chooseList中是否有选项，如果有就将其生成出来
    //将其放在更新对话中执行，传入的transform参数为按钮生成的位置
    
    // 同步更新立绘状态（可选）
    
    public void ChooseClickUpdateDialogueSpeech(int alpa)//点击时，更新对话  需要在生成按钮时就将按钮对应的List传入
    {
        dialogueData = dialogueDataListSO.dialogueDataDic[alpa];//根据对应ID选择字典中的对话数据 

        currentSpeechCount = 0; // 重置对话计数器
        isDialogueEnd = false; // 重置结束标志

        // 根据选项ID应用效果
         switch(alpa)
        {
            case 1: // 正义选项
                ApplyChoiceEffects(10, 0, 0);
                break;
            case 2: // 邪恶选项
                ApplyChoiceEffects(-10, 20, 0);
                break;
        }

        dialoguePanel.continueSpeechBtn.gameObject.SetActive(true);//点击选项后开启继续按钮
        currentSpeechCount = 0; // 重置对话进度
        ClearChoices();
        SyncDialogueInformation(currentSpeechCount); // 立即更新显示
    }

    public void ClearChoices()// 销毁之前的选择按钮
    {
        ChooseButton[] chooseBtns = dialoguePanel.chooseParent.GetComponentsInChildren<ChooseButton>();
        foreach (ChooseButton choice in chooseBtns)
        {
            Destroy(choice.gameObject);
        }
    }


    //调用对话系统以开始对话
        public void StartDialogue(int dialogueID)
    {
        if (dialogueDataListSO.dialogueDataDic.TryGetValue(dialogueID, out var newData))
        {
            
            dialogueData = newData;

            // 暂停游戏逻辑（2D俯视角需要）
            Time.timeScale = 0;
            
            // 初始化对话状态
            currentSpeechCount = 0;
            isDialogueEnd = false;
            dialoguePanel.gameObject.SetActive(true);
            
            // 安全验证（新增）
            if (dialogueData.dialogueList == null || dialogueData.dialogueList.Count == 0)
            {
                Debug.LogError("对话数据为空！");
                EndDialogue();
                return;
            }
            
            SyncDialogueInformation(currentSpeechCount);// 显示第一句对话
            UpdateSpeakerVisualState();// 更新立绘状态
            
            // 检查是否是结束节点（防止单句结束的对话）
            if (dialogueData.dialogueList[currentSpeechCount].isEndNode)
            {
                EndDialogue();
            }
        
            else
            {
                Debug.LogError($"对话ID {dialogueID} 不存在");
            }
        }
    }
    
    
    public void EndDialogue()//结束对话
    {   
        // 恢复游戏逻辑
        Time.timeScale = 1; 
        isDialogueEnd = true; // 标记对话结束
        currentSpeechCount = 0; // 重置对话计数器
        ClearChoices(); // 清除所有选项按钮
        dialoguePanel.gameObject.SetActive(false);// 关闭对话面板
        OnDialogueEnd?.Invoke(); // 触发事件
    }
    
    private void CheckForLogAddition()
    {
        string logText = dialogueData.dialogueList[currentSpeechCount].logTextToAdd;

        if (!string.IsNullOrEmpty(logText))
        {
            LogManager.Instance?.AddLog(logText);
        }
    }

            
    public void ApplyChoiceEffects(int moralityChange, int coinsChange, int healthChange)
    {
        PlayerStats.Instance.Morality += moralityChange;
        PlayerStats.Instance.Coins += coinsChange;
        PlayerStats.Instance.Health += healthChange;
    }
}
/*

立绘亮暗功能使用说明：
在 Unity Inspector 中：

调整 activeSpeakerColor 和 inactiveSpeakerColor 获得理想的亮暗效果
勾选/取消 keepInactiveSpeakerVisible 来切换显示模式

Excel 配置：

Row 列仍然使用 "left" 或 "right"

系统会自动处理立绘的亮暗变化

*/