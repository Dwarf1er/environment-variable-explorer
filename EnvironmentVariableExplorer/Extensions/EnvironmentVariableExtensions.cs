using EnvironmentVariableExplorer.Models;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

namespace EnvironmentVariableExplorer.Extensions
{
    public static class EnvironmentVariableExtensions
    {
        public static List<EnvironmentVariable> ToEnvironmentVariableList(this IDictionary environmentVariables, EnvironmentVariableTarget target)
        {
            var variableList = new List<EnvironmentVariable>();

            foreach (DictionaryEntry entry in environmentVariables)
            {
                variableList.Add(new EnvironmentVariable
                {
                    Name = entry.Key.ToString(),
                    Value = entry.Value.ToString(),
                    Target = target
                });
            }

            return variableList.OrderBy(variable => variable.Name).ToList();
        }
    }
}
