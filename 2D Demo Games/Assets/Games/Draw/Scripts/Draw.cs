using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Draw : MonoBehaviour
{
    public Camera camera;
    public GameObject brush;

    public KeyCode reset;

    LineRenderer lineRenderer;

    Vector2 lastPos;

    Vector3 eraseSize;

    private void Update()
    {
        DrawFunction();
        ChangeBrushSize();
        Reload();
    }

    void DrawFunction()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            if(mousePosition != lastPos)
            {
                AddPoint(mousePosition);
                lastPos = mousePosition;
            }
        }
        else
        {
            lineRenderer = null;
        }
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        lineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);

        lineRenderer.SetPosition(0, mousePosition);
        lineRenderer.SetPosition(1, mousePosition);
    }

    void AddPoint(Vector2 pointPos)
    {
        lineRenderer.positionCount++;
        int positionIndex = lineRenderer.positionCount - 1;

        lineRenderer.SetPosition(positionIndex, pointPos);
    }

    void Reload()
    {
        if (Input.GetKeyDown(reset))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void ChangeBrushSize()
    {
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if(brush.GetComponent<LineRenderer>().startWidth < 1)
            {
                brush.GetComponent<LineRenderer>().startWidth += 0.1f;
                brush.GetComponent<LineRenderer>().endWidth = brush.GetComponent<LineRenderer>().startWidth;

                if (lineRenderer)
                {
                    lineRenderer.startWidth += 0.1f;
                    lineRenderer.endWidth = lineRenderer.startWidth;
                }
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (brush.GetComponent<LineRenderer>().startWidth > 0.1)
            {
                brush.GetComponent<LineRenderer>().startWidth -= 0.1f;
                brush.GetComponent<LineRenderer>().endWidth = brush.GetComponent<LineRenderer>().startWidth;

                if (lineRenderer)
                {
                    lineRenderer.startWidth -= 0.1f;
                    lineRenderer.endWidth = lineRenderer.startWidth;
                }
            } 
        }
    }
}
