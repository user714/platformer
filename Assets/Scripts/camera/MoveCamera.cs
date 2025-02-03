using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.PlayerSettings;

[ExecuteAlways]
public class MoveCamera : MonoBehaviour
{
    [Header("Задать точки для кривой безье")]
    public Transform[] points_bezier;
    [Header("Количество секторов между точками миниму 1 сектор")]
    public  int count_sector = 1;
    [Header("Необходимо добавить Player")]
    public Transform player;
    [Header("Режим дебага камеры")]
    public bool debug = true;
     





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(  points_bezier.Length < 3)
        {
            Debug.LogError(" Укажите правильно кличество точек  для настроки траектории катмеры минимально чило точек  3  вы задали " + points_bezier.Length);
        }
    
        if(count_sector < 1)
        {
            Debug.LogError("Количество секторов между точками меньшене 1 ваша длина  " + count_sector);
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        

        if (debug)
        {

            GetPointDraf();

            LookToPlayer();
            

        }

    }
    void FixedUpdate()
    {


        LookToPlayer();
    }


    void LookToPlayer()
    {
      // transform.LookAt(player);

        int step_points = 0;

        int to_points = 0;

        for (int i = 0; i < points_bezier.Length - 3; i += 3)
        {
            if (player.position.x < points_bezier[i].position.x)
            {
                to_points = i;
                step_points = i - 3;
                break;
            }

            if (i == points_bezier.Length - 4)
            {
                to_points = points_bezier.Length - 1;
                step_points = points_bezier.Length - 4;
            }



        }

        if (step_points < 0)
        {
            step_points = 0;
        }

        float lien_x_points = points_bezier[to_points].position.x - points_bezier[step_points].position.x;

        float pluer_delta_phat = player.position.x - points_bezier[step_points].position.x;

        float procent = (pluer_delta_phat * 100 / lien_x_points) * 0.01f;





        transform.position = GetPoint(procent, step_points);
    }

    void GetPointDraf()
    {
        List<Vector3> points_sector_ = new List<Vector3>();

        for (int step = 0; step < points_bezier.Length-3; step += 3)
        {
            
            for (int sector = 0; sector <= count_sector; sector++)
            {

                float procent = ((sector * 100) / count_sector) * 0.01f;

                //Debug.Log(procent);

                points_sector_.Add(GetPoint(procent, step));
            }

            

            for (int i = 1; i < points_sector_.Count; i++)
            {
                Debug.DrawLine(points_sector_[i - 1], points_sector_[i], Color.green);

            }
        }

    }
 

    Vector3 GetPoint(float procent, int  start_point = 0)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = start_point+1; i < start_point+4; i++)
        {
            Debug.DrawLine(points_bezier[i - 1].position, points_bezier[i].position, Color.red);

            points.Add(Vector3.Lerp(points_bezier[i - 1].position, points_bezier[i].position, procent));
            
        }
       
        List<Vector3> points1 = new List<Vector3>();
        for (int i = 1; i < points.Count; i++)
        {
            points1.Add(Vector3.Lerp(points[i - 1], points[i], procent));

        }
      
        return Vector3.Lerp(points1[0], points1[1], procent);
    }
}
