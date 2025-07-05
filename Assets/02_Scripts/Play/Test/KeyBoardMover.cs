using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardMover : MonoBehaviour
{
    [SerializeField] private int gameType = 1;
    private PlayerMover_ST001 mover;
    private VirtualJoystickCtrl_ST001 vstick;
    private PlayerMover_ST002 mover2;
    private VirtualJoystickCtrl_ST002 vstick2;
    private Vector2 lastDir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<PlayerMover_ST001>();
        mover2 = GetComponent<PlayerMover_ST002>();
        vstick = GetComponent<VirtualJoystickCtrl_ST001>();
        vstick2 = GetComponent<VirtualJoystickCtrl_ST002>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameType)
        {
            case 1:
                if (vstick.enabled == true) { vstick.enabled = false; }
                float haxis = Input.GetAxis("Horizontal");
                float vaxis = Input.GetAxis("Vertical");
                Vector2 vec = new Vector2(haxis, vaxis);
                Vector2 dir = convertToDirection(vec);
                mover.SetDirection(dir);
                break;
            case 2:
                if (vstick2.enabled == true) { vstick2.enabled = false; }
                float haxis2 = Input.GetAxis("Horizontal");
                float vaxis2 = Input.GetAxis("Vertical");
                Vector2 vec2 = new Vector2(haxis2, vaxis2);
                Vector2 dir2 = convertToDirection(vec2);
                mover2.SetDirection(dir2);
                break;
            default:
                break;
        }
        
    }
    private Vector2 convertToDirection(Vector2 inputDir)
    {
        float radius = Mathf.Clamp01(inputDir.magnitude);
        float angle = inputDir.AngleInXZ();
        Vector2 dir = radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        return dir;
    }
}
