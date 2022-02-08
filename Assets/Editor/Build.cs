using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class Build
{
    // 10.0.17763.0
    private const string UWP_SDK_VERSION = "10.0.19041.0";

    /// <summary>
    /// start build process for HoloLens App
    /// </summary>
    /// adding to Unity menu for quick testing
    [MenuItem("Holographic/Build/Start", false, 0)]
    public static void StartBuild()
    {
        var path = string.Empty;
        foreach (DictionaryEntry dic in System.Environment.GetEnvironmentVariables())
        {
            if (dic.Key.ToString().Equals("WORKSPACE", System.StringComparison.InvariantCultureIgnoreCase))
            {
                path = "" + dic.Value;
                break;
            }
        }
        if (string.IsNullOrEmpty(path))
        {//backup for building outside of Jenkins
            path = new DirectoryInfo(Path.Combine(Application.dataPath, "../UWP")).FullName;
        }

        string[] scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
        string locationPathName = (path + "\\output\\UWP");
        BuildUnity(locationPathName);

        UpdateVCXProj(locationPathName);
        UpdatePackageManifest(locationPathName);

    }

    /// <summary>
    /// letting unity create visual studio solution
    /// </summary>
    /// <param name="locationPathName"></param>
    private static void BuildUnity(string locationPathName)
    {
        BuildPlayerOptions opts = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray(),
            targetGroup = BuildTargetGroup.WSA,
            target = BuildTarget.WSAPlayer,            
            locationPathName = locationPathName            
        };

        PlayerSettings.SetScriptingBackend(opts.targetGroup, ScriptingImplementation.IL2CPP);
        EditorUserBuildSettings.wsaUWPSDK = UWP_SDK_VERSION;
        EditorUserBuildSettings.wsaSubtarget = WSASubtarget.HoloLens;
        BuildPipeline.BuildPlayer(opts);
        Debug.Log("COMPLETED Unity Build:" + opts.locationPathName);
                
}

    /// <summary>
    /// Updating Visual Studio project file
    /// Setting SDK Version and auto-update props
    /// </summary>
    /// <param name="locationPathName">path to solution folder</param>
    private static void UpdateVCXProj(string locationPathName)
    {//c:\workspace\DLR-Demo\UWP\output\UWP => c:\workspace\DLR-Demo\UWP\output\UWP\ProjectName\ProjectName.vcxproj
        var vcxPath = Path.Combine(locationPathName, string.Concat(PlayerSettings.productName, "\\", PlayerSettings.productName, ".vcxproj"));
        var content = File.ReadAllText(vcxPath);
        content = System.Text.RegularExpressions.Regex.Replace(content,
            "<WindowsTargetPlatformMinVersion>[\\d.]+</WindowsTargetPlatformMinVersion>",
            string.Concat("<WindowsTargetPlatformMinVersion>", UWP_SDK_VERSION, "</WindowsTargetPlatformMinVersion>"));

        content = content.Replace("<AppxBundle>Auto</AppxBundle>",
            "<AppxBundle>Always</AppxBundle>\r\n" + //Force creating App Bundle
            "    <GenerateAppInstallerFile>True</GenerateAppInstallerFile>\r\n" + //..and App Installer (for automatic update)
            "    <AppxAutoIncrementPackageRevision>True</AppxAutoIncrementPackageRevision>\r\n" + //doesn't matter because version is generate everytime
            "    <AppxBundlePlatforms>ARM64</AppxBundlePlatforms>\r\n" + //HoloLens uses ARM64
            "    <AppInstallerUpdateFrequency>1</AppInstallerUpdateFrequency>\r\n" + //auto-update enabled
            "    <AppInstallerCheckForUpdateFrequency>OnApplicationRun</AppInstallerCheckForUpdateFrequency>\r\n" + //try updating every App start
            "    <AppxPackageDir>\\\\tsclient\\Holographic\\UWP\\" + PlayerSettings.productName + "\\</AppxPackageDir>\r\n" + //network drive share where we keep our HoloLens App-Builds
            "    <AppInstallerUri>http://10.70.157.108/" + PlayerSettings.productName + "/</AppInstallerUri>"); //HTTP-File Server (just mirroring the Network Drive)
        File.WriteAllText(vcxPath, content);
    }

    /// <summary>
    /// Updating the Version for the App based on current Date and Time
    /// </summary>
    /// <param name="locationPathName">path to solution folder</param>
    /// <returns></returns>
    private static bool UpdatePackageManifest(string locationPathName)
    {
        var buildDir = Path.Combine(locationPathName, string.Concat(PlayerSettings.productName, "\\"));
        // Find the manifest, assume the one we want is the first one
        string[] manifests = Directory.GetFiles(buildDir, "Package.appxmanifest", SearchOption.AllDirectories);

        if (manifests.Length == 0)
        {
            Debug.LogError(string.Format("Unable to find Package.appxmanifest file for build (in path - {0})", buildDir));
            return false;
        }

        string manifest = manifests[0];
        var rootNode = XElement.Load(manifest);
        var identityNode = rootNode.Element(rootNode.GetDefaultNamespace() + "Identity");

        if (identityNode == null)
        {
            Debug.LogError(string.Format("Package.appxmanifest for build (in path - {0}) is missing an <Identity /> node", buildDir));
            return false;
        }

        var versionAttr = identityNode.Attribute(XName.Get("Version"));

        if (versionAttr == null)
        {
            Debug.LogError(string.Format("Package.appxmanifest for build (in path - {0}) is missing a version attribute in the <Identity /> node.", buildDir));
            return false;
        }

        // preparing and updating new package Version
        var version = PlayerSettings.WSA.packageVersion;
        var now = DateTime.Now;
        //Attention: this method only works until the year 2099 ;)
        var newVersion = new Version(now.Year - 2000, now.Month * 100 + now.Day, now.Hour, now.Minute * 100 + now.Second);

        PlayerSettings.WSA.packageVersion = newVersion;
        versionAttr.Value = newVersion.ToString();

        var deps = rootNode.Element(rootNode.GetDefaultNamespace() + "Dependencies");
        var devFam = deps.Element(rootNode.GetDefaultNamespace() + "TargetDeviceFamily");
        devFam.Attribute(XName.Get("MinVersion")).Value = UWP_SDK_VERSION; //set min WinSDK Version to desired
        devFam.Attribute(XName.Get("MaxVersionTested")).Value = UWP_SDK_VERSION;//same for max WinSDK verison
        rootNode.Save(manifest);
        return true;
    }
}
