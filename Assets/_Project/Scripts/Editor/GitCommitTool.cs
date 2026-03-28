using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

public class GitCommitTool : EditorWindow
{
    private string commitMessage = "";

    [MenuItem("CargoShip/Git Push")]
    public static void ShowWindow()
    {
        GetWindow<GitCommitTool>("Git Push");
    }

    void OnGUI()
    {
        GUILayout.Label("Commit Mesaji:", EditorStyles.boldLabel);
        commitMessage = EditorGUILayout.TextArea(commitMessage, GUILayout.Height(60));
        GUILayout.Space(10);
        if (GUILayout.Button("Commit & Push", GUILayout.Height(40)))
        {
            if (string.IsNullOrEmpty(commitMessage))
            {
                EditorUtility.DisplayDialog("Hata", "Commit mesaji bos olamaz!", "Tamam");
                return;
            }
            RunGitCommitPush(commitMessage);
        }
    }

    void RunGitCommitPush(string message)
    {
        string repoPath = Path.GetFullPath(Application.dataPath + "/..");
        RunGit("add .", repoPath);
        RunGit($"commit -m \"{message}\"", repoPath);
        string result = RunGit("push origin main", repoPath);
        EditorUtility.DisplayDialog("Git Push", "Push tamamlandi!\n\n" + result, "OK");
        commitMessage = "";
    }

    string RunGit(string args, string workingDir)
    {
        var psi = new ProcessStartInfo("git", args)
        {
            WorkingDirectory = workingDir,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var p = Process.Start(psi);
        string output = p.StandardOutput.ReadToEnd();
        string error  = p.StandardError.ReadToEnd();
        p.WaitForExit();
        UnityEngine.Debug.Log($"[Git] {args}\n{output}{error}");
        return output + error;
    }
}
