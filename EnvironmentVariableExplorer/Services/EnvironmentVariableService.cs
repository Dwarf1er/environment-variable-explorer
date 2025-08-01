using EnvironmentVariableExplorer.Extensions;
using EnvironmentVariableExplorer.Models;
using System;
using System.Collections.Generic;

namespace EnvironmentVariableExplorer.Services
{
    public class EnvironmentVariableService
    {
        public List<EnvironmentVariable> GetEnvironmentVariablesByTarget(EnvironmentVariableTarget environmentVariableTarget)
        {
            return (Environment.GetEnvironmentVariables(environmentVariableTarget)).ToEnvironmentVariableList(environmentVariableTarget);
        }

        public List<EnvironmentVariable> GetAllEnvironmentVariables()
        {
            List<EnvironmentVariable> allEnvironmentVariables = new List<EnvironmentVariable>();
            allEnvironmentVariables.AddRange(GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.User));
            allEnvironmentVariables.AddRange(GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.Machine));
            return allEnvironmentVariables;
        }

        public void SetEnvironmentVariable(string environmentVariableName, string environmentVariableValue, EnvironmentVariableTarget environmentVariableTarget)
        {
            Environment.SetEnvironmentVariable(environmentVariableName, environmentVariableValue, environmentVariableTarget);
        }

        public void DeleteEnvironmentVariable(string environmentVariableName, EnvironmentVariableTarget environmentVariableTarget)
        {
            Environment.SetEnvironmentVariable(environmentVariableName, null, environmentVariableTarget);
        }
    }
}