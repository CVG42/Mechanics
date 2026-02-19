using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ScriptDragToHierarchy
{
    static ScriptDragToHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        Event currentEvent = Event.current;

        if (currentEvent.type != EventType.DragUpdated && currentEvent.type != EventType.DragPerform) return;

        Object draggedObject = DragAndDrop.objectReferences.Length > 0 ? DragAndDrop.objectReferences[0] : null;

        if (draggedObject is not MonoScript monoScript) return;

        System.Type scriptType = monoScript.GetClass();

        if (scriptType == null || !scriptType.IsSubclassOf(typeof(MonoBehaviour))) return;

        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

        if (currentEvent.type == EventType.DragPerform)
        {
            DragAndDrop.AcceptDrag();
            CreateGameObjectWithScript(scriptType, monoScript.name);
            currentEvent.Use();
        }
    }

    private static void CreateGameObjectWithScript(System.Type scriptType, string objectName)
    {
        GameObject go = new GameObject(objectName);
        go.AddComponent(scriptType);

        Undo.RegisterCreatedObjectUndo(go, "Create GameObject with Script");
        Selection.activeGameObject = go;
    }
}

