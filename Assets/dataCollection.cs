using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class dataCollection : MonoBehaviour
{
    private string firstLine, output;
    public string pathname;
    public GameObject fetus;
    private string currentTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        currentTimestamp = System.DateTime.Now.ToString().Replace(":", "-");
        currentTimestamp = currentTimestamp.Replace(" ", "-");
        pathname = pathname + "\\ECV-Game-" + currentTimestamp + ".txt";
        writeFirstLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (dataReader.isGameRunning)
        {
            writeData();
        }
    }

    public void writeFirstLine()
    {
        firstLine = string.Format("time_remaining, fetus_angle, selected_mech_point");

        using (StreamWriter file = new StreamWriter(pathname, true))
        {
            file.WriteLine(firstLine);
        }
    }

    public void writeData()
    {
        output = string.Format("{0}, {1}, {2}", timerScript.timeRemaining.ToString(), fetus.transform.localEulerAngles.y, dataReader.selectedPoint);

        using (StreamWriter file = new StreamWriter(pathname, true))
        {
            file.WriteLine(output);
        }
    }
}
