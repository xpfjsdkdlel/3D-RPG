using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    private Transform cam;
    private float curYAngle;
    [SerializeField]
    private int zoom = 0;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        cam = transform.GetChild(0);
        offset = new Vector3(0, 10, -10);
    }

    void Update()
    {
        if(target != null)
        {
            transform.position = target.position;
            zoom = Mathf.Clamp(zoom, 0, 4);
            cam.transform.localPosition = offset;
            if (Input.GetMouseButton(0))
            {// ��Ŭ�� �� ī�޶� ȸ�����
                curYAngle -= Input.GetAxis("Mouse X") * 3;
                transform.rotation = Quaternion.Euler(0, curYAngle, 0);
            }
            if(Input.GetAxis("Mouse ScrollWheel") < 0)
            {// ���콺 ���� �Ʒ��� ������ �� �ƿ�
                zoom--;
                offset = new Vector3(0, 9 - zoom, -9 + zoom);
            }
            else if(Input.GetAxis("Mouse ScrollWheel") > 0)
            {// ���콺 ���� ���� ������ �� ��
                zoom++;
                offset = new Vector3(0, 10 - zoom, -10 + zoom);
            }
        }
    }
}
