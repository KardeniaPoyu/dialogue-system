#if UNITY_EDITOR
using ExcelDataReader;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Text;

public static class DialogueExcelImporter
{
    [MenuItem("Tools/Dialogue/Import Excel (Fixed)")]
    public static void ImportDialogueExcelFixed()
    {
        try
        {
            // 设置编码支持中文
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string excelPath = EditorUtility.OpenFilePanel("Select Excel File", 
                $"{Application.dataPath}/Resources/DialogueExcel", 
                "xlsx,xls");

            if (string.IsNullOrEmpty(excelPath)) return;

            var config = new ExcelReaderConfiguration()
            {
                FallbackEncoding = Encoding.GetEncoding(936)
            };

            // 使用字典来合并相同ID的对话
            Dictionary<int, DialogueData> dialogueDict = new Dictionary<int, DialogueData>();

            using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream, config))
            {
                do // 遍历所有工作表
                {
                    // 读取首行获取列数
                    reader.Read();
                    int columnCount = reader.FieldCount;
                    Debug.Log($"当前工作表列数: {columnCount}");

                    while (reader.Read())
                    {
                        try
                        {
                            // 安全检查
                            if (columnCount < 7) // 至少需要7列基础数据
                            {
                                Debug.LogWarning($"跳过无效行，列数不足: {reader.Depth}");
                                continue;
                            }

                            int currentID = SafeGetInt(reader, 0);
                            
                            // 检查是否已存在该ID的对话数据
                            if (!dialogueDict.TryGetValue(currentID, out DialogueData data))
                            {
                                // 创建新的DialogueData
                                data = ScriptableObject.CreateInstance<DialogueData>();
                                data.ID = currentID;
                                data.speakerNameLeft = SafeGetString(reader, 1, "DefaultSpeaker");
                                data.speakerNameRight = SafeGetString(reader, 2, "DefaultNPC");
                                
                                // 资源加载带空检查
                                string leftPath = SafeGetString(reader, 3);
                                string rightPath = SafeGetString(reader, 4);
                                data.speakerLeft = !string.IsNullOrEmpty(leftPath) ? 
                                    Resources.Load<Sprite>(leftPath) : null;
                                data.speakerRight = !string.IsNullOrEmpty(rightPath) ? 
                                    Resources.Load<Sprite>(rightPath) : null;
                                    
                                dialogueDict.Add(currentID, data);
                            }

                            // 添加对话内容到dialogueList
                            Dialogue dialogue = new Dialogue
                            {
                                row = SafeGetString(reader, 5).ToLower() == "left" ? Row.left : Row.right,
                                speech = SafeGetString(reader, 6, "...")
                            };

                            // 可选选项（第7、8列）
                            if (columnCount >= 9)
                            {
                                string chooseId = SafeGetString(reader, 7);
                                string chooseText = SafeGetString(reader, 8);
                                
                                if (!string.IsNullOrEmpty(chooseId))
                                {
                                    dialogue.chooseList.Add(new Choose
                                    {
                                        ID = int.Parse(chooseId),
                                        chooseBtnText = chooseText
                                    });
                                }
                            }

                            data.dialogueList.Add(dialogue);
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError($"行 {reader.Depth} 解析失败: {e.Message}");
                        }
                    }
                } while (reader.NextResult());
            }

            // 保存资源
            string outputDir = $"{Application.dataPath}/Resources/DialogueData";
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // 先删除旧的资源文件
            foreach (var file in Directory.GetFiles(outputDir, "*.asset"))
            {
                File.Delete(file);
            }

            // 保存新的资源文件
            foreach (var kvp in dialogueDict)
            {
                string assetPath = $"Assets/Resources/DialogueData/{kvp.Key}_Dialogue.asset";
                AssetDatabase.CreateAsset(kvp.Value, assetPath);
                Debug.Log($"创建对话资源: ID={kvp.Key}, 对话数量={kvp.Value.dialogueList.Count}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Success", 
                $"成功导入 {dialogueDict.Count} 个对话!", "OK");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"导入失败: {e.Message}\n{e.StackTrace}");
            EditorUtility.DisplayDialog("Error", 
                $"导入失败: {e.Message}", "OK");
        }
    }

    #region 安全读取方法
    private static string SafeGetString(IExcelDataReader reader, int index, string defaultValue = "")
    {
        try 
        {
            return (reader.FieldCount > index) ? 
                reader.GetValue(index)?.ToString() ?? defaultValue : defaultValue;
        }
        catch 
        {
            return defaultValue;
        }
    }

    private static int SafeGetInt(IExcelDataReader reader, int index, int defaultValue = 0)
    {
        string strValue = SafeGetString(reader, index);
        return int.TryParse(strValue, out int result) ? result : defaultValue;
    }
    #endregion
}
#endif