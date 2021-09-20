using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using System.Linq;

public class MoveBeam : MonoBehaviour
{
    public GameObject radarBeam;
    public GameObject targetMarker;
    public Text angleValue;
    public Text distanceValue;
    public Text ipAddress;
    public Text targetDistanceValue;
    public Text targetAngleValue;
    public Text targetSizeValue;

    Vector3 eulerAngles;
    int portNumberReceive = 5000;
    UdpClient receivingUdpClient;
    Thread udpListeningThread;
    string dataReceived = "0, 0";

    float oldAngle = 0;
    float angle = 0;
    float distance = 0;
    List<float> averageAngle = new List<float>();

    private void Start()
    {
        ipAddress.text = FindLocalIP();
        initListenerThread();
    }

    void Update()
    {
        eulerAngles = radarBeam.transform.rotation.eulerAngles;

        string[] words = dataReceived.Split(',');
        angle = float.Parse(words[0]);
        distance = float.Parse(words[1]);

        angleValue.text = "Angle: " + angle.ToString();
        distanceValue.text = "Distance: " + distance.ToString();
        if (distance > 0 && oldAngle != angle) 
        {
            Instantiate(targetMarker, CircleXY(angle, distance), Quaternion.identity);
        }
        radarBeam.transform.eulerAngles = new Vector3(0, 0, angle);

        // Find target distance, angle and size
        if (distance > 0.1f && oldAngle != angle)
        {
            targetDistanceValue.text = "Distance: " + distance.ToString();
            averageAngle.Add(angle);
            if (averageAngle.Count > 0)
            {
                targetAngleValue.text = "Angle: " + averageAngle.Average().ToString("0.#");
                targetSizeValue.text = "Size: " + averageAngle.Count.ToString();
            }
        }

        // Reset average value
        if (angle > 87 || angle < 2)
        {
            averageAngle.Clear();
            averageAngle.Add(0);
            targetDistanceValue.text = "Distance: 0";
            targetAngleValue.text = "Angle: " + averageAngle.Average().ToString("0.#");
            averageAngle.Clear();
            targetSizeValue.text = "Size: " + averageAngle.Count.ToString();
        }

        oldAngle = angle;
    }

    private void initListenerThread()
    {
        portNumberReceive = 5000;

        Debug.Log("Started on : " + portNumberReceive.ToString());
        udpListeningThread = new Thread(new ThreadStart(UdpListener));

        // Run in background
        udpListeningThread.IsBackground = true;
        udpListeningThread.Start();
    }

    private string FindLocalIP()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }

        return localIP;
    }

    public void UdpListener()
    {
        receivingUdpClient = new UdpClient(portNumberReceive);

        while (true)
        {
            try
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                if (receiveBytes != null)
                {
                    string returnData = Encoding.ASCII.GetString(receiveBytes);
                    dataReceived = returnData.ToString();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }

    Vector2 CircleXY(float angle, float distance)
    {
        float lengtUnit = 1.5f;   // fitted for my Radar on screen
        float radians = (angle+90) * Mathf.Deg2Rad;         // +90 to move the target to the correct quadrant
        float x = Mathf.Cos(radians) * distance * lengtUnit;
        float y = Mathf.Sin(radians) * distance * lengtUnit;
        Vector2 pos = new Vector2(x, y);                     // *-1 to flip the x axis

        return pos;
    }
}
