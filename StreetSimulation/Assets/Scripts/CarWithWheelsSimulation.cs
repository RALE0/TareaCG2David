using UnityEngine;

public class CarWithWheelsSimulation : MonoBehaviour
{
    // Referencias a las ruedas, asignadas desde el inspector de Unity
    public GameObject[] wheels = new GameObject[4];

    // Parámetros de movimiento
    public float speed = 5.0f;
    public float turnAngle = 30.0f;

    // Velocidad de rotación de las ruedas
    public float wheelSpeed = 300.0f;

    // Variables para ajustar la posición de las ruedas
    public float idkVar1 = 0.5f;
    public float idkVar2 = 0.5f;

    private Vector3 currentVelocity;

    void Start()
    {
        // Configuración inicial del vehículo
        currentVelocity = Vector3.forward * speed;
    }
    void ApplyPositionAndRotationToWheel(GameObject wheel, Vector3 position, Quaternion rotation)
    {
        MeshFilter wheelMeshFilter = wheel.GetComponent<MeshFilter>();
        Vector3[] vertices = wheelMeshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rotation * vertices[i] + position;
        }

        wheelMeshFilter.mesh.vertices = vertices;
        wheelMeshFilter.mesh.RecalculateNormals();
    }
    void Update()
    {
        // Calcular la rotación y traslación del chasis
        float rotationInput = Input.GetAxis("Horizontal");
        Matrix4x4 rotationMatrix = HW_Transforms.RotateMat(rotationInput * turnAngle * Time.deltaTime, AXIS.Y);
        Matrix4x4 translationMatrix = HW_Transforms.TranslationMat(currentVelocity.x * Time.deltaTime,
                                                                   currentVelocity.y * Time.deltaTime,
                                                                   currentVelocity.z * Time.deltaTime);

        // Combinar rotación y traslación para el chasis
        Matrix4x4 vehicleMatrix = translationMatrix * rotationMatrix;
        ApplyMatrixToMesh(GetComponent<MeshFilter>().mesh, vehicleMatrix);

        // Obtener la nueva posición y rotación del chasis
        Vector3 vehiclePosition = vehicleMatrix.MultiplyPoint3x4(Vector3.zero);
        Quaternion vehicleRotation = Quaternion.LookRotation(vehicleMatrix.GetColumn(2), vehicleMatrix.GetColumn(1));

        // Aplicar rotaciones y traslaciones a las ruedas
        for (int i = 0; i < wheels.Length; i++)
        {
            // Ajustar la posición relativa de cada rueda
            Vector3 wheelRelativePosition = GetRelativeWheelPosition(i, vehicleRotation);

            // Calcular la posición y rotación de la rueda
            Quaternion wheelRotation = GetWheelRotation(i, vehicleRotation, rotationInput);
            Vector3 wheelPosition = vehiclePosition + wheelRelativePosition;

            // Aplicar la posición y rotación a la malla de la rueda
            ApplyPositionAndRotationToWheel(wheels[i], wheelPosition, wheelRotation);
        }
    }

    Vector3 GetRelativeWheelPosition(int wheelIndex, Quaternion vehicleRotation)
    {
        // Ajustar estas posiciones según el tamaño y la forma de tu modelo de vehículo
        float xPosition = (wheelIndex < 2) ? -idkVar1 : idkVar1; // Posición izquierda o derecha
        float zPosition = (wheelIndex % 2 == 0) ? -idkVar2 : idkVar2; // Posición delantera o trasera
        return vehicleRotation * new Vector3(xPosition, 0, zPosition);
    }

    Quaternion GetWheelRotation(int wheelIndex, Quaternion vehicleRotation, float steeringInput)
    {
        // Aplicar rotación adicional para las ruedas delanteras
        if (wheelIndex < 2)
        {
            return vehicleRotation * Quaternion.Euler(0, steeringInput * turnAngle, 0);
        }
        return vehicleRotation;
    }

    void ApplyMatrixToMesh(Mesh mesh, Matrix4x4 matrix)
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector4 vertex = matrix * new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, 1);
            vertices[i] = new Vector3(vertex.x, vertex.y, vertex.z);
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals(); // Recalcular las normales si es necesario
    }
}
