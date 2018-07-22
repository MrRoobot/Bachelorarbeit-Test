using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlfaktometerControl : MonoBehaviour {
    private string cmdStart="63";
    private Vector3 triggerPosition;
    private float triggerRadius;
    private float elapsed = 0f;

    [Tooltip("Werte von 0-255 \nReguliert die Intensität des Geruches")]
    public string Massendurchflussregler1 = "0";
    public string Massendurchflussregler2 = "0";
    public bool distanceOdorStrength = false;
    [HideInInspector]
    public bool[] Ventile=new bool[8];
   

    private void OnTriggerEnter(Collider col)
    {
        
        triggerPosition = GameObject.Find("Trigger").transform.position;
        triggerRadius = Vector3.Distance(triggerPosition, col.transform.position);
       
        string cmdVentil = cmdStart + convertVentileInputToString() + Massendurchflussregler1 + Massendurchflussregler2;
        
        Debug.Log(cmdVentil);
        Serial.Write(cmdVentil);
       
        
    }

    private void OnTriggerStay(Collider col)
    {
        if (distanceOdorStrength == true)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= 1f)
            {
                elapsed = elapsed % 1f;
                SmellStrenghtUpdate(col);
            }
           


        }
      
    }
    private void OnTriggerExit()
    {
        Serial.Write(cmdStart+"000000000");
       // Debug.Log("exit");
    }
    private float getColliderPos(Collider col)
    {
       
        Vector3 position = col.transform.position;
        float currentDistance = Vector3.Distance(triggerPosition, position);
        
        return currentDistance;

    }
    
    private string convertVentileInputToString()
    {
        BitArray arr = new BitArray(Ventile);
        byte[] data = new byte[1];
        arr.CopyTo(data, 0);
        string ventilInputToString = data[0].ToString();
        if (ventilInputToString.Length == 1)
        {
            ventilInputToString = "00" + ventilInputToString;
        }
        if (ventilInputToString.Length == 2)
        {
            ventilInputToString = "0" + ventilInputToString;
        }
        return ventilInputToString;
        
        
      

    }
    private void strengthDistanceRatio(float currentDistance)
    {
        
        float percentOfDistance = currentDistance/triggerRadius;
       // Debug.Log(percentOfDistance);
        if (percentOfDistance >= 0.9)
        {
            Massendurchflussregler1 = "025";
        }
        if (percentOfDistance >= 0.8 && percentOfDistance < 0.9)
        {
            Massendurchflussregler1 = "050";
        }
        if (percentOfDistance >= 0.7 && percentOfDistance < 0.8)
        {
            Massendurchflussregler1 = "075";
        }
        if (percentOfDistance >= 0.6 && percentOfDistance < 0.7)
        {
            Massendurchflussregler1 = "100";
        }
        if (percentOfDistance >= 0.5 && percentOfDistance < 0.6)
        {
            Massendurchflussregler1 = "125";
        }
        if (percentOfDistance >= 0.4 && percentOfDistance < 0.5)
        {
            Massendurchflussregler1 = "150";
        }
        if (percentOfDistance >= 0.3 && percentOfDistance < 0.4)
        {
            Massendurchflussregler1 = "175";
        }
        if (percentOfDistance >= 0.2 && percentOfDistance < 0.3)
        {
            Massendurchflussregler1 = "200";
        }
        if (percentOfDistance >= 0.1 && percentOfDistance < 0.2)
        {
            Massendurchflussregler1 = "225";
        }
        if (percentOfDistance < 0.1)
        {
            Massendurchflussregler1 = "250";
        }

    }
    
   
    void SmellStrenghtUpdate(Collider col)
    {
        strengthDistanceRatio(getColliderPos(col));
        Serial.Write(cmdStart + convertVentileInputToString() + Massendurchflussregler1 + Massendurchflussregler2);
        Debug.Log(cmdStart + convertVentileInputToString() + Massendurchflussregler1 + Massendurchflussregler2);
    }

}
