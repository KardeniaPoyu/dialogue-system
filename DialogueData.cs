using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "对话/DialogueData")]
public class DialogueData : ScriptableObject
{
	public int ID;//存储在字典中的ID
	public List<Dialogue> dialogueList = new List<Dialogue>(); // 确保初始化
     
	public string speakerNameLeft;
	public string speakerNameRight;
	public Sprite speakerLeft;
	public Sprite speakerRight;


	
}





public enum Row
{
    left,   // 立绘出现在左边
    right   // 立绘出现在右边
}

[Serializable]
public class Dialogue//对话
{
	
	public Row row;//枚举数据  区别立绘出现在左边或者右边
	[TextArea(5,4)]
	public string speech;
	public List<Choose> chooseList = new List<Choose>();//对话是否有选项
	public bool isEndNode; // 新增：是否对话终点

	 // 简化为直接存储要显示的日志文本（不需要时留空）
    [Header("日志文本（完成后显示）")]
    public string logTextToAdd; 
}
[Serializable]
public class Choose//选项
{
    public int ID;//选项的ID
    public string chooseBtnText;
}
