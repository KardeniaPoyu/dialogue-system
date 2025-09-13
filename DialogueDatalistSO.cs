using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "对话/DialogueDataListSO")]
public class DialogueDataListSO : ScriptableObject 
{
    public List<DialogueData> dialogueDataList;

    private Dictionary<int,DialogueData> DialogueDataDic;
	public Dictionary<int,DialogueData>  dialogueDataDic=> DialogueDataDic;
	


	public void Initialization()//初始化字典，并将dialogueDataList中的dialogueData存入
	{
    DialogueDataDic = new Dictionary<int, DialogueData>();
    foreach (var data in dialogueDataList)
    {
        if (DialogueDataDic.ContainsKey(data.ID))
        {
            Debug.LogError($"重复的对话ID: {data.ID}，已跳过");
            continue;
        }
        DialogueDataDic.Add(data.ID, data);
        Debug.Log($"加载对话数据: ID={data.ID}, 对话数={data.dialogueList.Count}");
    }
    }
}

