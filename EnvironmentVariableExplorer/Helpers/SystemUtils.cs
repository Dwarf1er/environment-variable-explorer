using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace EnvironmentVariableExplorer.Helpers
{
    public static class SystemUtils
    {
        public static readonly bool IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static readonly string currentUserName = Environment.UserName;

        public static IEnumerable<EnvironmentVariableTarget> GetPlatformSupportedTargets()
        {
            yield return EnvironmentVariableTarget.Process;

            if (SystemUtils.IsWindows)
            {
                yield return EnvironmentVariableTarget.User;
                yield return EnvironmentVariableTarget.Machine;
            }
        }
    }
}
