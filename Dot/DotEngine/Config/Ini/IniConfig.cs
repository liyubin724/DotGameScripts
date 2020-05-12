using Dot.Core.Generic;
using Dot.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dot.Config.Ini
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

        private ListDictionary<string, IniData> datas = new ListDictionary<string, IniData>();

        public string[] Keys
        {
            get
            {
                return datas.Keys;
            }
        }

        public bool Contains(string key)
        {
            return datas.ContainsKey(key);
        }

        public string GetValue(string key)
        {
            IniData data = datas[key];
            if(data !=null)
            {
                return data.Value;
            }

            return null;
        }

        public IniData GetData(string key)
        {
            return datas[key];
        }

        public void AddData(string key, string value, string comment, string[] optionValues)
        {
            if(!datas.TryGetValue(key,out IniData data))
            {
                data = new IniData();
                data.Key = key;
                datas.Add(key, data);
            }
            data.Value = value;
            data.Comment = comment;
            data.OptionValues = optionValues;
        }

        public void RemoveData(string key)
        {
            datas.Remove(key);
        }
    }

    public class IniConfig
    {
        public static readonly string INI_HEAD_CONTENT = "**INI-FILE**";

        public bool IsReadonly { get; set; } = true;
        
        private ListDictionary<string, IniGroup> groups = new ListDictionary<string, IniGroup>();

        public void ParseText(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return;
            }

            string[] lines = text.SplitToNotEmptyLines();
            if (lines == null || lines.Length <= 1)
            {
                return;
            }
            string headLine = lines[0].Trim();
            if(headLine!=INI_HEAD_CONTENT)
            {
                return;
            }

            IniGroup group = null;
            string comment = null;
            string[] optionValues = null;

            for(int i =1;i<lines.Length;++i)
            {
                string trimLine = lines[i].Trim();
                //空行及注释行会被忽略
                if (string.IsNullOrEmpty(trimLine)|| trimLine.StartsWith("//"))
                {
                    continue;
                }
                char startChar = trimLine[0];
                switch(startChar)
                {
                    case '#'://#号开头表示分组
                        {
                            group = new IniGroup();
                            group.Name = trimLine.Substring(1);
                            group.Comment = comment;

                            comment = null;
                            optionValues = null;

                            groups.Add(group.Name, group);
                            continue;
                        }
                    case '-'://-号开头表示对字段或者分组的注释
                        {
                            if (!IsReadonly)
                            {
                                comment = trimLine.Substring(2);
                            }
                            continue;
                        }
                    case ':'://:开头并以,做为分割符，表示可供选择的列表。在编辑器中将会以下拉列表展示
                        {
                            if (!IsReadonly)
                            {
                                optionValues = trimLine.Substring(1).SplitToNotEmptyArray(',');
                            }
                            continue;
                        }
                }
                
                if (group == null)
                {
                    continue;
                }

                int splitIndex = trimLine.IndexOf('=');
                if(splitIndex>0)
                {
                    string key = trimLine.Substring(0, splitIndex);

                    string value = null;
                    if (splitIndex < trimLine.Length - 1)
                    {
                        value = trimLine.Substring(splitIndex + 1);
                    }

                    if(!string.IsNullOrEmpty(key))
                    {
                        group.AddData(key, value, comment, optionValues);
                    }
                }
                comment = null;
                optionValues = null;
            }
        }

        public string GetValue(string key,string defaultValue = null)
        {
            for(int i =0;i<groups.Count;++i)
            {
                if(groups[i].Contains(key))
                {
                    return groups[i].GetValue(key);
                }
            }
            return defaultValue;
        }

        public string GetValueInGroup(string group,string key,string defaultValue = null)
        {
            if(groups.TryGetValue(group,out IniGroup g) && g.Contains(key))
            {
                return g.GetValue(key);
            }
            return defaultValue;
        }

        public bool GetBool(string key,bool defaultValue = false)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value) || !bool.TryParse(value, out bool result))
            {
                return defaultValue;
            }else
            {
                return result;
            }
        }

        public bool GetBoolInGroup(string group,string key,bool defaultValue = false)
        {
            string value = GetValueInGroup(group, key);
            if(string.IsNullOrEmpty(value) || !bool.TryParse(value, out bool result))
            {
                return defaultValue;
            }else
            {
                return result;
            }
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value) || !int.TryParse(value, out int result))
            {
                return defaultValue;
            }else
            {
                return result;
            }
        }

        public int GetIntInGroup(string group, string key, int defaultValue = 0)
        {
            string value = GetValueInGroup(group, key);
            if (string.IsNullOrEmpty(value) || !int.TryParse(value, out int result))
            {
                return defaultValue;
            } else
            {
                return result;
            }
        }

        public float GetFloat(string key, float defaultValue = 0.0f)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value) || !float.TryParse(value, out float result))
            {
                return defaultValue;
            }else
            {
                return result;
            }
        }

        public float GetFloatInGroup(string group, string key, float defaultValue = 0.0f)
        {
            string value = GetValueInGroup(group, key);
            if (string.IsNullOrEmpty(value) || !float.TryParse(value, out float result))
            {
                return defaultValue;
            }else
            {
                return result;
            }
        }

        public void AddGroup(string name,string comment)
        {
            if(IsReadonly)
            {
                throw new InvalidOperationException("IniConfig::AddGroup->readonly");
            }
            if (!groups.TryGetValue(name, out IniGroup g))
            {
                g = new IniGroup() { Name = name };
                groups.Add(name, g);
            }
            g.Comment = comment;
        }

        public void DeleteGroup(string name)
        {
            if (IsReadonly)
            {
                throw new InvalidOperationException("IniConfig::DeleteGroup->readonly");
            }
            groups.Remove(name);
        }

        public void AddData(string group,string key,string value,string comment,string[] optionValues)
        {
            if (IsReadonly)
            {
                throw new InvalidOperationException("IniConfig::AddData->readonly");
            }
            if (groups.TryGetValue(group, out IniGroup g))
            {
                g.AddData(key, value, comment, optionValues);
            }else
            {
                throw new KeyNotFoundException("InitConfig::AddData->group not found");
            }
        }

        public void DeleteData(string group,string key)
        {
            if (IsReadonly)
            {
                throw new InvalidOperationException("IniConfig::DeleteData->readonly");
            }
            if (groups.TryGetValue(group, out IniGroup g))
            {
                g.RemoveData(key);
            }
            else
            {
                throw new KeyNotFoundException("InitConfig::DeleteData->group not found");
            }
        }

        public void Save(string filePath)
        {
            StringBuilder configSB = new StringBuilder();
            configSB.AppendLine(INI_HEAD_CONTENT);
            configSB.AppendLine();

            for(int i =0;i<groups.Count;++i)
            {
                IniGroup group = groups[i];
                if(!string.IsNullOrEmpty(group.Comment))
                {
                    configSB.AppendLine($"-{group.Comment}");
                }
                configSB.AppendLine($"#{group.Name}");

                string[] keys = group.Keys;
                for(int j = 0;j<keys.Length;++j)
                {
                    IniData data = group.GetData(keys[j]);
                    if (!string.IsNullOrEmpty(data.Comment))
                    {
                        configSB.AppendLine($"-{data.Comment}");
                    }
                    if (data.OptionValues != null && data.OptionValues.Length > 0)
                    {
                        configSB.AppendLine($":{string.Join(",", data.OptionValues)}");
                    }
                    configSB.AppendLine($"{data.Key} = {(data.Value == null?"":data.Value)}");
                }

                configSB.AppendLine();
            }

            File.WriteAllText(filePath, configSB.ToString());
        }
    }
}
