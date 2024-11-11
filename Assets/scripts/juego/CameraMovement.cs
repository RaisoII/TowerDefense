using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public float maxZoom;
    public float minZoom;
    Vector3 currentPos;
    public float minX, minY, maxX, maxY;
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 direccion = currentPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position += direccion;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), -10);
        }

        zoom(Input.GetAxis("Mouse ScrollWheel")*4);

    }

    void zoom(float incremento) 
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - incremento, minZoom, maxZoom);
       // camaraCinematica.setAntiguoZoom();
    }

    public void setPos(Vector3 pos) => Camera.main.transform.position = pos;

}


