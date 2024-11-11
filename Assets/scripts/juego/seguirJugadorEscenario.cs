using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class seguirJugadorEscenario : MonoBehaviour
{
    public float maxZoom;
    public float minZoom;
    Vector3 vectorTocado;
    public float minX, minY, maxX, maxY;
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            vectorTocado = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 direccion = vectorTocado - Camera.main.ScreenToWorldPoint(Input.mousePosition);

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

    public void setLimites(GameObject salaObjeto)
    {
        sala sala = salaObjeto.GetComponent<sala>();
        BoundsInt datos = sala.getLimites(); 

        minX = datos.xMin;
        maxX = datos.xMax;
        minY = datos.yMin;
        maxY = datos.yMax;
    }

    public void setLimites(float maxZoom,float minZoom,float minX, float minY, float maxX, float maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;

        this.maxZoom = maxZoom;
        this.minZoom = minZoom;
    }

    public void setPos(Vector3 pos) => Camera.main.transform.position = pos;

    private IEnumerator esperarBoton()
    {
        while(true)
        {
            if(!Input.GetMouseButtonDown(1))
            {
                activar();
                yield break;       
            }

            yield return null;
        }

    }

    private void activar()
    {
        enabled = true;
    }

    public float getMaxZoom() => maxZoom;
    public float getMinZoom() => minZoom;
    public float getMinX() => minX;
    public float getMinY() => minY;
    public float getMaxX() => maxX;
    public float getMaxY() => maxY;
}


