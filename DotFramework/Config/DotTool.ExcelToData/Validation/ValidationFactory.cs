using DotTool.ETD.Util;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DotTool.ETD.Validation
{
    public static class ValidationFactory
    {
        private static readonly string VALIDATION_NAME_REGEX = @"(?<name>[A-Za-z]+)";

        public static void ParseValidations(string multiRule,List<IFieldValidation> validations)
        {
            if(string.IsNullOrEmpty(multiRule))
            {
                return;
            }

            multiRule = multiRule.Trim();
            if(string.IsNullOrEmpty(multiRule))
            {
                return;
            }

            string[] rules = multiRule.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (rules == null || rules.Length == 0)
            {
                return;
            }

            foreach (var rule in rules)
            {
                IFieldValidation validation = GetValidation(rule);
                if (validation == null)
                {
                    validation = new ErrorValidation();
                    validation.Rule = rule;
                }
                validations.Add(validation);
            }
        }

        private static IFieldValidation GetValidation(string rule)
        {
            if (string.IsNullOrEmpty(rule))
            {
                return null;
            }
            rule = rule.Trim();
            if (string.IsNullOrEmpty(rule))
            {
                return null;
            }

            Match match = new Regex(VALIDATION_NAME_REGEX).Match(rule);
            if (match.Success)
            {
                string validationName = match.Groups["name"].Value;
                if (string.IsNullOrEmpty(validationName))
                {
                    return null;
                }
                validationName += "Validation";
                Type type = AssemblyUtil.GetTypeByName(validationName);
                if (type == null || !typeof(IFieldValidation).IsAssignableFrom(type))
                {
                    return null;
                }

                IFieldValidation validation = (IFieldValidation)Activator.CreateInstance(type);
                validation.Rule = rule;

                return validation;
            }

            return null;
        }
    }
}
