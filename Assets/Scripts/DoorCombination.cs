using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCombination : MonoBehaviour
{
    [SerializeField] GameObject Door;
    public string combination = "312132";
    private string attempt = "";
    // Start is called before the first frame update
    void Start()
    {
        attempt = "";
    }

    // Update is called once per frame
    public void OnButtonCollision(string number)
    {
        attempt = attempt + number;
        if (attempt.Length == combination.Length)
        {
            if (attempt == combination)
            {
                Door.transform.position += new Vector3(0,400,0);
            }
            else 
            {
            Debug.Log("wrong");
            attempt = "";
            }

        }
        
        
    }
}
