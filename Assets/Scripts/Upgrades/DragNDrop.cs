using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragNDrop : MonoBehaviour
{
    private Camera main;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    GridManager manager;
    [SerializeField]
    private InputAction mouseClick;
    [SerializeField]
    private float mouseDragSpeed = 0.1f;



    private void Awake()
    {
        main = Camera.main;
    }

    private void OnEnable()
    {
        mouseClick.Enable();

        mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        mouseClick.performed += MousePressed;
        mouseClick.Disable();
    }


    private void MousePressed(InputAction.CallbackContext context)
    {

        Ray ray = main.ScreenPointToRay(Mouse.current.position.ReadValue());


        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
        if (hit2D.collider != null && hit2D.collider.gameObject.GetComponent<RobotUpgrade>() != null)
        {
            StartCoroutine(DragUpdate(hit2D.collider.gameObject));
        }

    }

    private IEnumerator DragUpdate(GameObject clickedObject)
    {

        Vector3 initialPosition = clickedObject.transform.position;
        float initialDistance = Vector3.Distance(initialPosition, main.transform.position);
        RobotUpgrade upgrade = clickedObject.GetComponent<RobotUpgrade>();

        Vector2[] lastCells = new Vector2[upgrade.cells.Length];
        for (int i = 0; i < upgrade.cells.Length; i++)
        {

            lastCells[i] = upgrade.cells[i];

        }

        int rotations = 0;
        while (mouseClick.ReadValue<float>() != 0)
        {
            Ray ray = main.ScreenPointToRay(Mouse.current.position.ReadValue());
            clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position,
             ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);

            clickedObject.transform.position += Vector3.back;

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                upgrade.Rotate90(0);
                rotations++;
            }

            yield return null;
        }

        upgrade.resetToGrid();
        Debug.Log("Ultime Posizioni");
        foreach (Vector2 cell in lastCells)
        {
            Debug.Log(cell);
        }
        Debug.Log("Nuove Posizioni");
        foreach (Vector2 cell in upgrade.cells)
        {
            Debug.Log(cell);
        }

        //Cerchiamo se la nuova posizione è completamente illegale, altrimenti cancelliamo la figura
        if (manager.IsOutOfBounds(upgrade))
        {
            Destroy(upgrade.gameObject);
        }

        //Se la nuova posizione è legale almeno in parte controlliamo sia possibile
        if (!manager.AddShape(upgrade, initialPosition, lastCells))
        {
            clickedObject.transform.position = initialPosition;
            for (int i = 0; i < rotations; i++)
            {
                upgrade.Rotate90(1);
            }
        }

    }
}
