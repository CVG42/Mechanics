using UnityEngine;

public class FirstLaw : MonoBehaviour
{
    SimulatedBody cuerpo;

    void Start()
    {
        cuerpo = GetComponent<SimulatedBody>();
        // Darle velocidad inicial — nadie la frena
        cuerpo.velocidad = new Vector3(2f, 0f, 0f);
        // No aplicamos ninguna fuerza en FixedUpdate -> sigue recto
    }
}
