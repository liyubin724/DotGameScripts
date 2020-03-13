using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dot.Ini
{
    public class IniData
    {
        public string Key { get; set; }
        public string Comment { get; set; }
        public string[] OptionValues { get; set; }
        public string Value { get; set; }
    }

    public class IniGroup
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public Dictionary<string, IniData> dataDic = new Dictionary<string, IniData>();

#if UNITY_EDITOR
        public void AddData(string key, string value, string comment, string[] optionValues)
        {
            if(!dataDic.TryGetValue(key,out IniData data))
            {
                data = new IniData();
                data.Key = key;

                dataDic.Add(key, data);
            }
            data.Value = value;
            data.Comment = comment;
            data.OptionValues = optionValues;
        }

        public void DeleteData(string key)
        {
            if(dataDic.ContainsKey(key))
            {
                dataDic.Remove(key);
            }
        }
#endif
    }

    public class IniConfig
    {
        public static readonly string INI_HEAD_CONTENT = "**INI-FILE**";
        private Dictionary<string, IniGroup> groupDic = new Dictionary<string, IniGroup>();
        private bool isReadonly = true;

        public IniConfig(string text,bool isReadonly)
        {
            this.isReadonly = isReadonly;
            if(!string.IsNullOrEmpty(text))
            {
                Init(text);
            }
        }

        private void Init(string text)
        {
            string[] lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if(lines == null || lines.Length == 0)
            {
                return;
            }
            string headLine = lines[0].Trim();
            if(headLine!=INI_HEAD_CONTENT)
            {
                return;
            }

            IniGroup group = null;
            IniData data = null;

            string comment = null;
            string[] optionValues = null;
            foreach(var line in lines)
            {
                string trimLine = line.Trim();

                //配置文件中注释使用//,首行使用**表示文件类型，均将会被忽略
                if (trimLine.StartsWith("//") || trimLine.StartsWith("**"))
                {
                    continue;
                }

                //#号开头表示分组
                if (trimLine.StartsWith("#"))
                {
                    group = new IniGroup();
                    group.Name = trimLine.Substring(1);
                    group.Comment = comment;

                    comment = null;
                    data = null;
                    optionValues = null;

                    groupDic.Add(group.Name,group);
                    continue;
                }

                //--开头表示对字段或者分组的注释
                if (trimLine.StartsWith("--"))
                {
                    if(!isReadonly)
                    {
                        comment = trimLine.Substring(2);
                    }
                    continue;
                }
                
                if (group == null)
                {
                    continue;
                }

                //使用 []包围并以,做为分割符，表示可供选择的列表。在编辑器中将会以下拉列表展示
                if (trimLine.StartsWith("[") && trimLine.EndsWith("]"))
                {
                    if(!isReadonly)
                    {
                        optionValues = trimLine.Substring(1, trimLine.Length - 2).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    continue;
                }

                string[] datas = trimLine.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if(datas!=null && datas.Length>0)
                {
                    string key = datas[0].Trim();
                    if(!string.IsNullOrEmpty(key))
                    {
                        data = new IniData();
                        data.Key = key;
                        data.Comment = comment;
                        data.OptionValues = optionValues;

                        comment = null;
                        optionValues = null;

                        group.dataDic.Add(key, data);

                        if(datas.Length>1)
                        {
                            data.Value = datas[1].Trim();
                        }else
                        {
                            data.Value = string.Empty;
                        }
                    }
                }
            }

        }

        public string GetValue(string key,string defaultValue = null)
        {
            foreach(var gKVP in groupDic)
            {
                string value = GetValueInGroup(gKVP.Key, key);
                if(value!=null)
                {
                    return value;
                }
            }

            return defaultValue;
        }

        public string GetValueInGroup(string group,string key,string defaultValue = null)
        {
            if(groupDic.TryGetValue(group,out IniGroup g))
            {
                if(g.dataDic.TryGetValue(key,out IniData d))
                {
                    return d.Value;
                }
            }
            return defaultValue;
        }

        public bool GetBool(string key,bool defaultValue = false)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (bool.TryParse(value, out bool result))
            {
                return result;
            }
            return defaultValue;
        }

        public bool GetBoolInGroup(string group,string key,bool defaultValue = false)
        {
            string value = GetValueInGroup(group, key);
            if(string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if(bool.TryParse(value,out bool result))
            {
                return result;
            }
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public int GetIntInGroup(string group, string key, int defaultValue = 0)
        {
            string value = GetValueInGroup(group, key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0.0f)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (float.TryParse(value, out float result))
            {
                return result;
            }
            return defaultValue;
        }

        public float GetFloatInGroup(string group, string key, float defaultValue = 0.0f)
        {
            string value = GetValueInGroup(group, key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (float.TryParse(value, out float result))
            {
                return result;
            }
            return defaultValue;
        }

#if UNITY_EDITOR

        public IniConfig()
        {
            isReadonly = false;
        }

        public void AddGroup(string name,string comment)
        {
            if(isReadonly)
            {
                return;
            }
            if (!groupDic.TryGetValue(name, out IniGroup g))
            {
                g = new IniGroup() { Name = name };
                groupDic.Add(name, g);
            }
            g.Comment = comment;
        }

        public void DeleteGroup(string name)
        {
            if (isReadonly)
            {
                return;
            }
            if (groupDic.ContainsKey(name))
            {
                groupDic.Remove(name);
            }
        }

        public void AddData(string group,string key,string value,string comment,string[] optionValues)
        {
            if (isReadonly)
            {
                return;
            }
            if (groupDic.TryGetValue(group, out IniGroup g))
            {
                g.AddData(key, value, comment, optionValues);
            }
        }

        public void DeleteData(string group,string key)
        {
            if (isReadonly)
            {
                return;
            }
            if (groupDic.TryGetValue(group, out IniGroup g))
            {
                g.DeleteData(key);
            }
        }

        public void Save(string filePath)
        {
            StringBuilder configSB = new StringBuilder();
            configSB.AppendLine(INI_HEAD_CONTENT);
            configSB.AppendLine();

            foreach(var gKVP in groupDic)
            {
                IniGroup group = gKVP.Value;
                configSB.AppendLine($"#{group.Name}");
                if(!string.IsNullOrEmpty(group.Comment))
                {
                    configSB.AppendLine($"--{group.Comment}");
                }
                foreach(var dKVP in group.dataDic)
                {
                    IniData data = dKVP.Value;
                    if(!string.IsNullOrEmpty(data.Comment))
                    {
                        configSB.AppendLine($"--{data.Comment}");
                    }
                    if(data.OptionValues!=null && data.OptionValues.Length>0)
                    {
                        configSB.AppendLine($"[{string.Join(",", data.OptionValues)}]");
                    }
                    configSB.AppendLine($"{data.Key} = {data.Value}");
                }

                configSB.AppendLine();
            }

            File.WriteAllText(filePath, configSB.ToString());
        }
#endif
    }
}
