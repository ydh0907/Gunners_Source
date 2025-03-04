using System;
using System.IO;
using UnityEngine;

public class IOManager : MonoBehaviour
{
    public static IOManager Instance;

    public int Win = 0;
    public int Lose = 0;
    public string path = "Gunners.txt";

    private void Awake()
    {
        Instance = this;

        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);

        try
        {
            if (File.Exists(path))
            {
                using(FileStream fs = File.Open(path, FileMode.Open))
                {
                    byte[] buffer = new byte[12];
                    fs.Read(buffer, 0, 12);

                    int win = BitConverter.ToInt32(buffer, 0);
                    int lose = BitConverter.ToInt32(buffer, sizeof(int));
                    int temp = BitConverter.ToInt32(buffer, sizeof(int) * 2);

                    if((win + 9070) * 9070 + (lose + 9070) * 9070 == temp)
                    {
                        Win = win;
                        Lose = lose;
                    }
                    else
                    {
                        Win = 0;
                        Lose = 0;
                    }
                }
            }
        }
        catch
        {
            Win = 0;
            Lose = 0;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.onGameWin += CountWin;
        GameManager.Instance.onGameLose += CountLose;
    }

    private void OnDisable()
    {
        GameManager.Instance.onGameWin -= CountWin;
        GameManager.Instance.onGameLose -= CountLose;
    }

    private void CountWin() => ++Win;
    
    private void CountLose() => ++Lose;

    private void OnApplicationQuit()
    {
        using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
        {
            int temp = (Win + 9070) * 9070 + (Lose + 9070) * 9070;

            byte[] buffer = new byte[12];
            Buffer.BlockCopy(BitConverter.GetBytes(Win), 0, buffer, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Lose), 0, buffer, sizeof(int), 4);
            Buffer.BlockCopy(BitConverter.GetBytes(temp), 0, buffer, sizeof(int) * 2, 4);
            fs.Write(buffer, 0, sizeof(int) * 3);
        }
    }
}
