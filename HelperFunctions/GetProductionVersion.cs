using System;

public static class VersionHelper
{
    public static string GetProductionVersion(string[] versions)
    {
        Version highestVersion = new Version(0, 0, 0);
        string highestVersionString = "";

        foreach (string versionString in versions)
        {
            string[] parts = versionString.Split('-');
            Version version = new Version(parts[0]);
            if (version.CompareTo(highestVersion) > 0)
            {
                highestVersion = version;
                highestVersionString = parts.Length > 1 ? $"{parts[0]}-{parts[1]}" : parts[0];
            }
        }

        return highestVersionString;
    }
}
