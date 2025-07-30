using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Threading;

public class openweb : MonoBehaviour
{
    public PauseManager pauseManager;    // lowercase 'p'
    public ScoreTimerManager ScoreTimerManager;
    public SkillManager skillManager;
    public string scriptPath = @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\backend\app.py";
    public string scriptPath1 = @"C:\Users\kanch\Downloads\PyVenture_ide-main\PyVenture_ide-main\fastapi_server.py";
    public string baseUrl = @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\quest\index.html";
    public string pythonPath = @"C:\Users\kanch\AppData\Local\Programs\Python\Python311\python.exe";
    public string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
    // พาธที่ Python server จะเขียน done.txt
    private string doneFilePath = @"C:\Users\kanch\Downloads\PyVenture_ide-main\PyVenture_ide-main\project\backend\done.txt";

    private Dictionary<string, string> chapterPages;

    void Start()
    {
        // สตาร์ท Python servers
        RunPythonServer(scriptPath);
        RunPythonServer(scriptPath1);
        // เตรียม chapterPages ตามเดิม


        chapterPages = new Dictionary<string, string>
        {
            // CHAPTER 1
            { "ch1_1", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\easy\ch1\index.html" },
            { "ch1_2", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\easy\ch2\index.html" },
            { "ch1_3", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\easy\ch3\index.html" },
            { "ch1_4", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\easy\ch4\index.html" },

            // CHAPTER 2
            { "ch2_1", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\normal\ch1\index.html" },
            { "ch2_2", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\normal\ch2\index.html" },
            { "ch2_3", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\normal\ch3\index.html" },
            { "ch2_4", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\normal\ch4\index.html" },

            // CHAPTER 3
            { "ch3_1", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\hard\ch1\index.html" },
            { "ch3_2", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\hard\ch2\index.html" },
            { "ch3_3", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\hard\ch3\index.html" },
            { "ch3_4", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\hard\ch4\index.html" },

            // CHAPTER 4
            { "ch4_1", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\advance\ch1\index.html" },
            { "ch4_2", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\advance\ch2\index.html" },
            { "ch4_3", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\advance\ch3\index.html" },
            { "ch4_4", @"C:\Users\kanch\Downloads\New_CRT_NSC27\Asset\project\project\frontend\advance\ch4\index.html" }
        };
    }


    public void OpenChapter(string key)
    {
        if (!chapterPages.ContainsKey(key))
        {
            UnityEngine.Debug.LogError("❌ ไม่พบ chapter: " + key);
            return;
        }

        if (!ProcessIsRunning("python"))
            RunPythonServer(scriptPath);

        WaitForServerReady();
        OpenInChrome(chapterPages[key]);
    }

    public void LoadQuestionByIndex(int index)
    {
        string htmlUrl = $"file:///{baseUrl.Replace("\\", "/")}?choice={index}";

        if (File.Exists(chromePath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = chromePath,
                Arguments = $"--start-fullscreen \"{htmlUrl}\"", // เพิ่ม --start-fullscreen เพื่อเปิดหน้าเว็บในโหมดเต็มจอ
                UseShellExecute = true // ✅ สำคัญ!  
            });

            UnityEngine.Debug.Log($"🌐 เปิดด้วย Chrome: {htmlUrl}");
        }
        else
        {
            UnityEngine.Debug.LogError("❌ ไม่พบ Chrome ที่ path ที่กำหนด");
        }
    }

    void OpenInChrome(string htmlPath)
    {
        if (!File.Exists(htmlPath))
        {
            UnityEngine.Debug.LogError("❌ ไฟล์ HTML ไม่พบ: " + htmlPath);
            return;
        }

        string htmlUrl = $"file:///{htmlPath.Replace("\\", "/")}";

        if (File.Exists(chromePath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = chromePath,
                Arguments = $"--start-fullscreen \"{htmlUrl}\"", // เพิ่ม --start-fullscreen เพื่อเปิดหน้าเว็บในโหมดเต็มจอ
                UseShellExecute = true // ✅ สำคัญ!
            });

            UnityEngine.Debug.Log("🌐 เปิด HTML ด้วย Chrome ในโหมดเต็มจอแล้ว!");
        }
        else
        {
            UnityEngine.Debug.LogError("❌ ไม่พบ Chrome ที่ path ที่กำหนด");
        }
    }

    void RunPythonServer(string script)
    {
        if (File.Exists(pythonPath) && File.Exists(script))
        {
            var psi = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"\"{script}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = new Process();
            process.StartInfo = psi;
            process.Start();

            UnityEngine.Debug.Log("✅ Flask server started.");
        }
        else
        {
            UnityEngine.Debug.LogError("❌ Python path หรือ script path ไม่ถูกต้อง");
        }
    }

    void WaitForServerReady()
    {
        // Optional: ใส่ logic รอ server ที่พัฒนาในอนาคตได้
    }

    bool ProcessIsRunning(string name)
    {
        return Process.GetProcessesByName(name).Length > 0;
    }

    void Update()
    {
        // เช็คไฟล์ done.txt ที่ Python เขียน
        if (File.Exists(doneFilePath))
        {
            string result = File.ReadAllText(doneFilePath).Trim();
            UnityEngine.Debug.Log("🎉 เจอ done.txt ผลลัพธ์: " + result);

            if (result == "correct")
            {
                PlayerPrefs.SetInt("last_result", 1);
                skillManager.ShowSkillPanel();
                UnityEngine.Debug.Log("✅ Quiz ถูกต้อง! ให้คะแนน ✔️");
            }
            else if (result == "wrong")
            {
                PlayerPrefs.SetInt("last_result", 0);
                UnityEngine.Debug.Log("❌ Quiz ผิด ลองใหม่อีกครั้ง 🔁");
            }

            if (result == "complete")
            {
                PlayerPrefs.SetInt("last_result", 0);
                pauseManager.ResumeGame1();      // <-- instance method
                UnityEngine.Debug.Log("✅ Quiz complete → resumed game");
                // แจ้ง Unity ว่า task เสร็จทั้งหมด
                // เพิ่มฟังก์ชันหรือส่งข้อมูลไปยัง Unity/Backend ได้ที่นี่
            }
            if (result == "complete_final")
            {
                PlayerPrefs.SetInt("last_result", 0);
                pauseManager.ResumeGame1();      // <-- instance method
                                                 //callhere func:
                UnityEngine.Debug.Log("✅ SPCQuiz complete → resumed game");
                // แจ้ง Unity ว่า task เสร็จทั้งหมด
                // เพิ่มฟังก์ชันหรือส่งข้อมูลไปยัง Unity/Backend ได้ที่นี่
            }
            // ลบไฟล์ออกแล้วกลับไป MainMenu
            File.Delete(doneFilePath);
            //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }

}
