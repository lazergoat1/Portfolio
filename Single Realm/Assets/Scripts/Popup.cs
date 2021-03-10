using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public float dissappearSpeed = 0.5f;

    private TextMeshPro textMesh;
    private float dissappearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public static Popup Create(Vector3 position, int popupValue, GameObject popupToCreate)
    {
        GameObject popupTransfomrm = Instantiate(popupToCreate, position, Quaternion.identity);

        Popup popup = popupTransfomrm.GetComponent<Popup>();
        popup.Setup(popupValue);

        return popup;
    }

    public void Setup(int popupValue)
    {
        textMesh.text = textMesh.text + popupValue.ToString();
        textColor = textMesh.color;
    }

    private void Update()
    {
        float speed = 0.2f;

        transform.position += new Vector3(0, speed * Time.deltaTime);
        dissappearTimer -= Time.deltaTime;

        if(dissappearTimer <= 0)
        {
            textColor.a -= dissappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if(textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
