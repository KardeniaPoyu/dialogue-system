using UnityEngine;
using UnityEngine.UI;
using TMPro; // 添加命名空间

public class DialoguePanel : MonoBehaviour
{
    [Tooltip("说话者左侧")]public TMP_Text speakerNameLeft; // 改为TMP_Text
    
    [Tooltip("说话者右侧")]public TMP_Text speakerNameRight; // 改为TMP_Text

    [Tooltip("说话内容")]public TMP_Text speech; // 改为TMP_Text
    [Header("------")]
    [Tooltip("说话角色的立绘或者头像左侧")]public Image rowImageLeft;

    [Tooltip("说话角色的立绘或者头像右侧")]public Image rowImageRight;

    [Header("------")]
    [Tooltip("继续下一句话按钮")]public Button continueSpeechBtn;
    [Header("------")]
    [Tooltip("生成选项预制体")]public ChooseButton chooseButtonPre;
    [Header("------")]
    [Tooltip("选择按钮的生成位置")]public Transform chooseParent;
}