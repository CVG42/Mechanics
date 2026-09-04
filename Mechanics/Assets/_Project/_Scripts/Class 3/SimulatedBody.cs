using UnityEngine;

public class SimulatedBody : MonoBehaviour
{
    [Header("Propiedades físicas")]
    public float masa = 1f;
    public Vector3 velocidad = Vector3.zero;

    // Acumulador de fuerzas — se reinicia cada frame
    private Vector3 _fuerzaTotal = Vector3.zero;

    // === API pública para aplicar fuerzas ===
    public void AplicarFuerza(Vector3 fuerza)
    {
        _fuerzaTotal += fuerza;
    }

    void FixedUpdate()
    {
        // 2ª Ley de Newton: a = F / m
        Vector3 aceleracion = _fuerzaTotal / masa;

        // Integración de Euler explícito
        velocidad += aceleracion * Time.fixedDeltaTime;
        transform.position += velocidad * Time.fixedDeltaTime;

        // Reiniciar acumulador (las fuerzas se re-aplican cada frame)
        _fuerzaTotal = Vector3.zero;
    }
}
