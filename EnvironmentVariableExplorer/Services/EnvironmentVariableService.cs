using EnvironmentVariableExplorer.Extensions;
using EnvironmentVariableExplorer.Models;
using EnvironmentVariableExplorer.Helpers;
using System;
using System.Collections.Generic;
using EnvironmentVariableExplorer.Shared;

namespace EnvironmentVariableExplorer.Services
{
    public class EnvironmentVariableService
    {
        public Result<List<EnvironmentVariable>, string> GetEnvironmentVariablesByTarget(EnvironmentVariableTarget environmentVariableTarget)
        {
            return ResultExtensions.Try(() => Environment.GetEnvironmentVariables(environmentVariableTarget).ToEnvironmentVariableList(environmentVariableTarget), $"Failed to get environment variables for {environmentVariableTarget}");
        }

        public Result<List<EnvironmentVariable>, string> GetAllEnvironmentVariables()
        {
            Result<List<EnvironmentVariable>, string> processEnvironmentVariablesResult = GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.Process);
            if (processEnvironmentVariablesResult.IsFailure)
            {
                return Result<List<EnvironmentVariable>, string>.Err(processEnvironmentVariablesResult.Error);
            }

            List<EnvironmentVariable> allEnvironmentVariables = new List<EnvironmentVariable>(processEnvironmentVariablesResult.Value);

            if (SystemUtils.IsWindows)
            {
                Result<List<EnvironmentVariable>, string> userEnvironmentVariablesResult = GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.User);
                if (userEnvironmentVariablesResult.IsFailure)
                {
                    return Result<List<EnvironmentVariable>, string>.Err(userEnvironmentVariablesResult.Error);
                }

                allEnvironmentVariables.AddRange(userEnvironmentVariablesResult.Value);

                Result<List<EnvironmentVariable>, string> machineEnvironmentVariablesResult = GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.Machine);
                if (machineEnvironmentVariablesResult.IsFailure)
                {
                    return Result<List<EnvironmentVariable>, string>.Err(machineEnvironmentVariablesResult.Error);
                }

                allEnvironmentVariables.AddRange(machineEnvironmentVariablesResult.Value);
            }

            return Result<List<EnvironmentVariable>, string>.Ok(allEnvironmentVariables);
        }

        public Result<Unit, string> SetEnvironmentVariable(string environmentVariableName, string environmentVariableValue, EnvironmentVariableTarget environmentVariableTarget)
        {
            if (!SystemUtils.IsWindows && (environmentVariableTarget == EnvironmentVariableTarget.User || environmentVariableTarget == EnvironmentVariableTarget.Machine))
            {
                return Result<Unit, string>.Err($"{environmentVariableTarget} environment variables are only supported on Windows.");
            }

            return ResultExtensions.Try(() => Environment.SetEnvironmentVariable(environmentVariableName, environmentVariableValue, environmentVariableTarget), $"Failed to set environment variable '{environmentVariableName}'");
        }

        public Result<Unit, string> DeleteEnvironmentVariable(string environmentVariableName, EnvironmentVariableTarget environmentVariableTarget)
        {
            if (!SystemUtils.IsWindows && (environmentVariableTarget == EnvironmentVariableTarget.User || environmentVariableTarget == EnvironmentVariableTarget.Machine))
            {
                return Result<Unit, string>.Err($"Deleting {environmentVariableTarget} environment variables is only supported on Windows.");
            }

            return ResultExtensions.Try(() => Environment.SetEnvironmentVariable(environmentVariableName, null, environmentVariableTarget), $"Failed to delete environment variable '{environmentVariableName}'");
        }
    }
}