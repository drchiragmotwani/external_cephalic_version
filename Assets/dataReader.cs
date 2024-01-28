using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using UnityEngine.UI;

public class dataReader : MonoBehaviour
{
    public SerialPort serial = new SerialPort("\\\\.\\COM3", 9600);
    public string flex0String, flex1String, hardwareString, imuX, imuY, imuZ, pushButtonString;
    public string[] hardwareStringArray;
    public float flex0Value, flex1Value, imuXVal, imuYVal, imuZVal, fetusCurrentAngle, angleStep = 1.5f;
    static public int highlightedPoint = 1, selectedPoint = 1, pushButtonValue = 0;
    public float[] flipperPoints;
    public GameObject flipper, pointShadowHighlighted, pointShadowSelected, point1Shadow, point2Shadow, point3Shadow, point4Shadow, fetus;
    static public bool isGameRunning = false;
    public Material mech_point_shadow, mech_point_shadow_highlighted, mech_point_shadow_selected;
    public Text gameStatusText;

    // Start is called before the first frame update
    void Start()
    {
        serial.Open();
        Debug.Log("Serial Open");

        point1Shadow = GameObject.Find("point1Shadow");
        point2Shadow = GameObject.Find("point2Shadow");
        point3Shadow = GameObject.Find("point3Shadow");
        point4Shadow = GameObject.Find("point4Shadow");
        fetus = GameObject.Find("fetusBody");

        isGameRunning = true;
        flipperPoints = new float[4];
        gameStatusText.text = "Game Running";
        //mech_point_shadow = Resources.Load<Material>("Material/mech_point_shadow.mat");
        //mech_point_shadow_highlighted = Resources.Load<Material>("Material/mech_point_shadow_highlighted.mat");
        //mech_point_shadow_selected = Resources.Load<Material>("Material/mech_point_shadow_selected.mat");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            serial.Close();
        }

        fetusCurrentAngle = fetus.transform.localEulerAngles.y;
        Debug.Log("Fetus angle: " + fetusCurrentAngle);

        if ((fetusCurrentAngle >= 267 && fetusCurrentAngle <= 273) || timerScript.timeRemaining == 0)
        {
            isGameRunning = false;

            if (fetusCurrentAngle >= 267 && fetusCurrentAngle <= 273)
            {
                gameStatusText.text = "Fetus oriented successfully for Normal Delivery";
            }
            else
            {
                gameStatusText.text = "Game Over - Fetus orientation is a contra-indication for Normal Delivery";
            }
        }
        else if (fetusCurrentAngle > 0 && fetusCurrentAngle <= 90)
        {
            flipperPoints[0] = -angleStep;
            flipperPoints[1] = angleStep;
            flipperPoints[2] = -angleStep;
            flipperPoints[3] = angleStep;
        }
        else if (fetusCurrentAngle > 90 && fetusCurrentAngle <= 180)
        {
            flipperPoints[0] = angleStep;
            flipperPoints[1] = -angleStep;
            flipperPoints[2] = angleStep;
            flipperPoints[3] = -angleStep;
        }
        else if (fetusCurrentAngle > 180 && fetusCurrentAngle < 270)
        {
            flipperPoints[0] = -angleStep;
            flipperPoints[1] = angleStep;
            flipperPoints[2] = -angleStep;
            flipperPoints[3] = angleStep;
        }
        else if (fetusCurrentAngle > 270 && fetusCurrentAngle <= 360)
        {
            flipperPoints[0] = angleStep;
            flipperPoints[1] = -angleStep;
            flipperPoints[2] = angleStep;
            flipperPoints[3] = -angleStep;
        }

        if (fetusCurrentAngle < 5 || fetusCurrentAngle > 360 || (fetusCurrentAngle > 175 && fetusCurrentAngle < 185))
        {
            serial.Write("1");
        }
        else
        {
            serial.Write("0");
        }

        if (isGameRunning)
        {
            point1Shadow.GetComponent<MeshRenderer>().material = mech_point_shadow;
            point2Shadow.GetComponent<MeshRenderer>().material = mech_point_shadow;
            point3Shadow.GetComponent<MeshRenderer>().material = mech_point_shadow;
            point4Shadow.GetComponent<MeshRenderer>().material = mech_point_shadow;

            hardwareString = serial.ReadLine();
            hardwareStringArray = hardwareString.Split(',');
            Debug.Log(hardwareStringArray[7]);

            flex0String = hardwareStringArray[0];
            flex1String = hardwareStringArray[2];
            imuX = hardwareStringArray[4];
            imuY = hardwareStringArray[5];
            imuZ = hardwareStringArray[6];
            pushButtonString = hardwareStringArray[7];

            float.TryParse(flex0String, out flex0Value);
            float.TryParse(flex1String, out flex1Value);
            float.TryParse(imuX, out imuXVal);
            float.TryParse(imuY, out imuYVal);
            float.TryParse(imuZ, out imuZVal);
            pushButtonValue = System.Int32.Parse(pushButtonString);

            // Debug.Log(flexValue);

            if (imuZVal < -20)
            {
                highlightedPoint = 4;
            }
            else if (imuZVal > 20)
            {
                highlightedPoint = 2;
            }

            if (imuYVal < -20)
            {
                highlightedPoint = 3;
            }
            else if (imuYVal > 20)
            {
                highlightedPoint = 1;
            }

            if (pushButtonValue == 1)
            {
                selectedPoint = highlightedPoint;
            }

            if (highlightedPoint > 0)
            {
                pointShadowHighlighted = GameObject.Find("point" + highlightedPoint + "Shadow");
                pointShadowHighlighted.GetComponent<MeshRenderer>().material = mech_point_shadow_highlighted;
            }

            if (selectedPoint > 0)
            {
                flipper = GameObject.Find("flipper" + flipperPoints[selectedPoint - 1]);
                pointShadowSelected = GameObject.Find("point" + selectedPoint + "Shadow");

                pointShadowSelected.GetComponent<MeshRenderer>().material = mech_point_shadow_selected;
            }

            if (flex0Value < 0)
            {
                flex0Value = 0;
            }

            if (flex0Value > 0 && selectedPoint > 0)
            {
                // flipper.transform.Translate(0, 0, flex0Value * 0.01f * Time.deltaTime);
                fetus.transform.Rotate(flipperPoints[selectedPoint - 1] * Time.deltaTime, 0, 0);
            }

            if (flex1Value < 0)
            {
                flex1Value = 0;
            }

            if (flex1Value > 0 && selectedPoint > 0)
            {
                // flipper.transform.Translate(0, 0, -1 * flex1Value * 0.01f * Time.deltaTime);
                fetus.transform.Rotate(-flipperPoints[selectedPoint - 1] * Time.deltaTime, 0, 0);
            }
        }
    }
}
