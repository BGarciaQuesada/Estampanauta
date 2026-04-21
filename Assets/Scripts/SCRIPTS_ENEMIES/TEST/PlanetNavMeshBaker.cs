using Unity.AI.Navigation;
using UnityEngine;

public class PlanetNavMeshBaker : MonoBehaviour
{
    static readonly Quaternion[] orientations = new Quaternion[]
    {
        // 6 caras originales
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(180, 0, 0),
        Quaternion.Euler(90, 0, 0),
        Quaternion.Euler(-90, 0, 0),
        Quaternion.Euler(0, 0, 90),
        Quaternion.Euler(0, 0, -90),

        // 8 esquinas
        Quaternion.Euler(45, 45, 0),
        Quaternion.Euler(45, -45, 0),
        Quaternion.Euler(45, 135, 0),
        Quaternion.Euler(45, -135, 0),
        Quaternion.Euler(135, 45, 0),
        Quaternion.Euler(135, -45, 0),
        Quaternion.Euler(135, 135, 0),
        Quaternion.Euler(135, -135, 0),

        // Laterales
        Quaternion.Euler(45F, -45F, -90F),
        Quaternion.Euler(45F, 45F, 90F),
        Quaternion.Euler(-45F, 45F, -90F),
        Quaternion.Euler(-45F, -45F, 90F)
    };

    [ContextMenu("Create And Bake All NavMeshSurface's")]
    void CreateAndBake()
    {
        EraseExistant();
        foreach (Quaternion rotation in orientations)
        {
            GameObject sonObj = new GameObject("NavMeshSurface_"+rotation.eulerAngles);
            sonObj.transform.SetParent(transform);
            sonObj.transform.localRotation = rotation;
            sonObj.transform.localPosition = Vector3.zero;

            NavMeshSurface surface = sonObj.AddComponent<NavMeshSurface>();
            surface.BuildNavMesh();
        }
    }

    [ContextMenu("Erase Existant NavMeshSurface's")]
    void EraseExistant()
    {
        foreach (NavMeshSurface nms in GetComponentsInChildren<NavMeshSurface>())
        {
            DestroyImmediate(nms.transform.gameObject);
        }
    }

}
