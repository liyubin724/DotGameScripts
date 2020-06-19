using DotTool.ETD.Fields;
using DotTool.ETD.Validation;
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
        protected int col;
        protected string name;
        protected string desc;
        protected string type;
        protected string platform;
        protected string defaultValue;
        protected string validationRule;

        public int Col { get => col; }

        public FieldType Type { get; private set; } = FieldType.None;
        public FieldPlatform Platform { get; private set; } = FieldPlatform.None;
        private IFieldValidation[] validations = null;
        public IFieldValidation[] GetValidations()
        {
            if(validations != null)
            {
                return validations;
            }

            List<IFieldValidation> validationList = new List<IFieldValidation>();
            AppendDefaultValidation(validationList);
            ValidationFactory.ParseValidations(validationRule, validationList);
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
            validationRule = r;

            Type = FieldTypeUtil.GetFieldType(type);
            Platform = GetPlatform(platform);
        }

        public abstract string GetDefaultValue();
        protected abstract void AppendDefaultValidation(List<IFieldValidation> validations);

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
