using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotUpgrade : MonoBehaviour
{
    [SerializeField] private int shapeNum;
    [SerializeField] private Vector2 pointOfOrigin;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float cellLayer;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.UI.Enable();
    }

    public Vector2[] cells;
    private int lastShape = -1;
    private float cellCorrection = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

        UpdatePOI();

        ShapeIdentifier(shapeNum);

    }

    private void UpdatePOI()
    {
        pointOfOrigin = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {




    }

    public void resetToGrid()
    {
        transform.position = new Vector3(Mathf.Floor(transform.position.x),
             Mathf.Floor(transform.position.y));

        transform.position += new Vector3(cellCorrection, cellCorrection, cellLayer);
        UpdatePOI();
    }



    //Non avevo altre idee per costruire le figure se non attraverso queste costanti
    //Fare una classe diversa per ogni roba mi sembra eccessivo, per ora
    private Vector2[] LINE_2 = { new Vector2(0, 0), new Vector2(0, 1) };
    private Vector2[] LINE_3 = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2) };
    private Vector2[] LINE_4 = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2), new Vector2(0, 3) };

    private Vector2[] ANGLE_2X2 = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0) };
    private Vector2[] ANGLE_3X2 = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 0) };
    private Vector2[] ANGLE_4X2 = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2),
                                    new Vector2(0, 3), new Vector2(1, 0) };
    private Vector2[] SQUARE = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

    private void ShapeIdentifier(int shapeNum)
    {
        switch (shapeNum)
        {

            case 2: cells = LINE_3; break;  //Linea 3
            case 3: cells = SQUARE; break;  //Quadrato
            case 4: cells = LINE_4; break;
            case 5: cells = ANGLE_2X2; break;
            case 6: cells = ANGLE_3X2; break;
            case 7: cells = ANGLE_4X2; break;
            default:
            case 1: cells = LINE_2; break;  //Linea 2

        }
        lastShape = shapeNum;

    }




    //0 clock 1 anti-clock
    public void Rotate90(int dir)
    {
        if (dir == 0) //Clock
        {

            for (int i = 0; i < cells.Length; i++)
            {
                float x = cells[i].x, y = cells[i].y;

                cells[i] = new Vector2(y, -x);
            }

            float yRotation = 90;

            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - yRotation);

        }
        else if (dir == 1) //anti-clock
        {
            for (int i = 0; i < cells.Length; i++)
            {
                float x = cells[i].x, y = cells[i].y;

                cells[i] = new Vector2(-y, x);
            }

            float yRotation = 90;

            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + yRotation);
        }

    }

    public Vector2[] getCells()
    {
        return cells;
    }

    public Vector2 getPOI()
    {
        return pointOfOrigin;
    }


}
