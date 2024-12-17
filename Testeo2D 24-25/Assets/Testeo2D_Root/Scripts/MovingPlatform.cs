using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed; //velocidad de la plataforma
    [SerializeField] int startingPoint; //N�mero para determinar el index del punto de inicio del movimiento
    [SerializeField] Transform[] points; //Array de puntos de posici�n a los que la plataforma "perseguir�"
    int i; //index que determina que n�mero de plataforma se persigue actualmente


    // Start is called before the first frame update
    void Start()
    {
        //Setear la posici�n inicial de la plataforma en uno de los puntos.
        transform.position = points[startingPoint].position; 
    }

    // Update is called once per frame
    void Update()
    {
        PlatformMove();
    }

    void PlatformMove()
    {
        //Deetector de si la plataforma ha legado al destino, cambiano el destino
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++; //Aumenta en uno el index, cambia de objetivo
            if (i == points.Length) i = 0;
        }

        //Movimiendo: SIEMPRE DESPU�S DE LA DETECCI�N
        //Mueve la lataforma al punto del array que coincide con el valor de i
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
}

