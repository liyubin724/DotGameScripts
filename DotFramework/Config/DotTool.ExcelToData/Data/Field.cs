using DotTool.ETD.Fields;
using DotTool.ETD.Validation;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DotTool.ETD.Data
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class FieldRealyType : Attribute
    {
        public Type RealyType { get; set; }
        public FieldRealyType(Type type)
        {
            RealyType = type;
        }
    }

    public class LuaType
    { }

    public enum FieldType
    {
        None = 0,

        [FieldRealyType(typeof(int))]
        Int,
        [FieldRealyType(typeof(int))]
        Id,
        [FieldRealyType(typeof(int))]
        Ref,

        [FieldRealyType(typeof(long))]
        Long,

        [FieldRealyType(typeof(float))]
        Float,

        [FieldRealyType(typeof(bool))]
        Bool,

        [FieldRealyType(typeof(string))]
        String,
        [FieldRealyType(typeof(string))]
        Address,
        [FieldRealyType(typeof(string))]
        Lua,

        [FieldRealyType(typeof(IList))]
        List,
    }

    public enum FieldPlatform
    {
        None = 0,
        Client,
        Server,
        All,
    }

    public abstract class Field
    {
        public int col;
        public string name;
        public string desc;
        public string type;
        public string platform;
        public string defaultValue;
        public string validationRule;

        public FieldType FieldType { get; private set; } = FieldType.None;
        public FieldPlatform FieldPlatform { get; private set; } = FieldPlatform.None;
        private IFieldValidation[] validations = null;
        public IFieldValidation[] GetValidations()
        {
            if(validations != null)
            {
                return validations;
            }

            string validationStr = validationRule + ";" +GetDefaultValidation();

            List<IFieldValidation> validationList = new List<IFieldValidation>();
            ValidationFactory.ParseValidations(validationStr, validationList);
            validations = validationList.ToArray();

            return validations;
        }

        protected Field(int c, string n, string d, string t, string p, string v, string r)
        {
            col = c;
            name = n;
            desc = d;
            type = t ?? t.Trim().ToLower();
            platform = p ?? p.Trim().ToLower();
            defaultValue = v;
            validationRule = string.IsNullOrEmpty(r)?"":r.Trim();

            FieldType = FieldTypeUtil.GetFieldType(type);
            FieldPlatform = GetPlatform(platform);
        }

        protected virtual string GetDefaultValidation() { return ""; }

        private FieldPlatform GetPlatform(string platform)
        {
            if (platform == "c")
            {
                return FieldPlatform.Client;
            }
            else if (platform == "s")
            {
                return FieldPlatform.Server;
            }
            else if (platform == "cs")
            {
                return FieldPlatform.All;
            }

            return FieldPlatform.None;
        }

        public override string ToString()
        {
            return $"<col = {col},name = {name},desc={desc},type={type},platform={platform},defaultValue={defaultValue},validationRule={validationRule}>";
        }
    }
}
