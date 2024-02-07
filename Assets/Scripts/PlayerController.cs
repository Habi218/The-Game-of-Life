using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {
    [SerializeField] private Camera gameCamera;
    [SerializeField] private AudioClip _click;

    void Start() {
        int cameraY;
        if(GameManager.BoardWidth > GameManager.BoardHeight * 1.78f) {
            cameraY = (int)((GameManager.BoardWidth/1.7f) + 0.5f);
        } else {
            cameraY = (GameManager.BoardHeight + 1);
        }
        gameCamera.transform.position = new Vector3(0, cameraY, 0);
    }

    void Update() {
        if(GameManager.Instance.State == GameState.SetUp) {    
            Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo)) {
                Cell cellSelected = hitInfo.transform.GetComponent<Cell>();
                if(Input.GetMouseButton(0) && GameManager.Population < 20 && cellSelected.State == 0) {
                    cellSelected.State = 1;
                    SoundManager.Instance.PlaySound(_click);
                } else if(Input.GetMouseButton(1) && cellSelected.State == 1) {
                    cellSelected.State = 0;
                }
            }
        }
    }
}
