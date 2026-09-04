using UnityEngine;

public class ThirdLaw : MonoBehaviour
{
    public SimulatedBody cuerpoA;
    public SimulatedBody cuerpoB;
    public float radioColision = 1f;

    // Coeficiente de restitución: 1 = elástico, 0 = inelástico
    [Range(0f, 1f)]
    public float restitución = 0.8f;

    void FixedUpdate()
    {
        Vector3 diff = cuerpoB.transform.position - cuerpoA.transform.position;
        float distancia = diff.magnitude;

        if (distancia < radioColision * 2f)
        {
            ResolverColision(diff.normalized);
        }
    }

    void ResolverColision(Vector3 normal)
    {
        // Velocidad relativa en la dirección de la normal
        Vector3 velRelativa = cuerpoB.velocidad - cuerpoA.velocidad;
        float velEnNormal = Vector3.Dot(velRelativa, normal);

        // Si ya se están separando, no resolver
        if (velEnNormal > 0) return;

        // Cálculo del impulso (J)
        float j = -(1f + restitución) * velEnNormal;
        j /= (1f / cuerpoA.masa) + (1f / cuerpoB.masa);

        Vector3 impulso = j * normal;

        // 3ª Ley: fuerzas iguales y opuestas
        cuerpoA.velocidad -= impulso / cuerpoA.masa;
        cuerpoB.velocidad += impulso / cuerpoB.masa;
    }
}
