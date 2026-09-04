using UnityEngine;

public class SecondLaw : MonoBehaviour
{
    SimulatedBody cuerpo;

    [Header("Configuración")]
    public float fuerzaImpulso = 500f;
    public Vector3 direccionDisparo = new Vector3(1f, 1f, 0f);

    // Gravedad manual — NO usamos Physics.gravity
    private readonly Vector3 _gravedad = new Vector3(0f, Constants.GRAVITY, 0f);

    void Start()
    {
        cuerpo = GetComponent<SimulatedBody>();
        // Aplicar impulso inicial (fuerza enorme un solo frame)
        cuerpo.AplicarFuerza(direccionDisparo.normalized * fuerzaImpulso);
    }

    void FixedUpdate()
    {
        // Gravedad: F = m * g (aplicada cada frame)
        cuerpo.AplicarFuerza(cuerpo.masa * _gravedad);
    }
}
