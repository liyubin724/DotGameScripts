﻿using DotTool.ETD.Data;
using DotTool.ETD.Validation;
using System.Collections.Generic;

namespace DotTool.ETD.Fields
{
    public class ListField : Field
    {
        public FieldType ValueType { get; private set; } = FieldType.None;
        public string ValueRefName { get; private set; } = string.Empty;

        public ListField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            ValueType = FieldTypeUtil.GetListInnerType(t, out string refName);
            ValueRefName = refName;
        }

        public override string GetDefaultValue()
        {
            return defaultValue;
        }

        protected override void AppendDefaultValidation(List<IFieldValidation> validations)
        {
            validations.Add(new ListValidation() { Rule = "List" });
        }
    }
}
