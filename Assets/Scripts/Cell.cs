using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    private Renderer ren;
    public int State = 0;
    public int NumNeighbors = 0;

    void Start() {
        ren = GetComponent<Renderer>();
    }

    void Update() {
        if(State == 0) {
            ren.material.color = new Color(0.15f, 0.15f, 0.15f, 0f);
        } else if(State == 1) {
            ren.material.color = Color.white;
        }
    }
}