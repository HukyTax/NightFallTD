using System;
using System.ComponentModel.Design;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    Boolean activeShadow = false;
    public static LevelManager main;

    public Transform[] path;
    public Transform startPoint;

    private void Awake()
    {
        main = this;
    }
    public void UpdateActiveShadow(Boolean value)
    {
        activeShadow = value;
    }
    public Boolean getActiveShadow()
    {
       return activeShadow; 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
