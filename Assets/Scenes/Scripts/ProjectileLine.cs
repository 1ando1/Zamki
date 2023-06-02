using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //�������� ��'���

    [Header("Set in Inspector")]
    public float minDist = 1.0f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    // Start is called before the first frame update
    void Awake()
    {
        S = this; //���������� ��������� �� �������� ��'���
        //�������� ��������� �� LineRender
        line = GetComponent<LineRenderer>();
        //��������� LineRender, ���� �� �� �����������
        line.enabled = false;
        //������������� ������ �����
        points = new List<Vector3>();
    }
    public GameObject poi
    {
        get { return (_poi); }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //���� ���� _poi �� ����� ���������, ������� �� �������� ��������� � ���������� ����
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    //��� ����� ����� ���������, ��� ������ ����
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        //����������� ��� ��������� ����� � ����
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
            //���� ����� ����������� ������ �� ���������� - ������ �����
            return;

        if (points.Count == 0)
        {
            //���� �� ����� �������, ������ ���������� �������� ����, ��� ��������� ����� ����������� �����
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;

            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //���������� ����� 2 �����
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //������� LineRender
            line.enabled = true;
        }
        else
        {
            //�������� ������������ ��������� �����
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
    //������� ��������������� �������� �����
    public Vector3 lastPoint
    { 
        get
        {
            if (points == null)
                return (Vector3.zero); //���� ����� ���� - ��������� Vector3.zero
            return (points[points.Count - 1]);
        }
    }
    private void FixedUpdate()
    {
        if (poi == null)
            //���� poi �� ����� ��������, ������ �������� ��'���
            if (FollowCam.POI != null)
                if (FollowCam.POI.tag == "Projectile")
                    poi = FollowCam.POI;
                else
                    return; //�����, ���� ��'��� �� ���������
            else
                return; //�����, ���� ��'��� �� ���������

        //���� �������� ��'��� �������� - ���������� ������ ����� � ���� ������������ � ������� FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
            poi = null; //���� FollowCam.POI �� null, �������� null � poi
    }
}
