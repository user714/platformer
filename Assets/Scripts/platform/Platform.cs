using System.Collections.Generic;
using UnityEngine;

 
public class Platform : MonoBehaviour
{
    private Vector3 coursor =   Vector3.zero;
    private int  move_to = 1;

    [Header("Задать точки для кривой безье")]
    public Transform[] points_bezier;
    [Header("Количество секторов между точками миниму 1 сектор")]
    public  int count_sector = 1;
    [Header("Необходимо добавить  начальную  точку")]
    public Transform start_point;
    [Header("Необходимо добавить  начальную  точку")]
    public Transform finish_point;
    [Header("Необходимо добавить платформу кторая будет двигаться по траиктории")]
    public Transform platform;
    [Header("Время за кторое платформа должна пройти дистанцию")]
    public float time = 10;

    [Header("Режим дебага камеры")]
    public bool debug = true;

    public Transform cursor;

     





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        if (  points_bezier.Length < 3)
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
       float  delata =   Vector3.Distance(start_point.position, finish_point.position);

        // time

        if (cursor.transform.position == finish_point.position && move_to == 1)
        {
            move_to = -1;
        }
        if (cursor.transform.position == start_point.position && move_to == -1)
        {
            move_to = 1;
        }

        if (move_to == 1)
        {
            cursor.transform.position = Vector3.MoveTowards(cursor.transform.position, finish_point.position, (delata / time) * Time.fixedDeltaTime);

        }
        if (move_to == -1)
        {
            cursor.transform.position = Vector3.MoveTowards(cursor.transform.position, start_point.position, (delata / time) * Time.fixedDeltaTime);

        }

        if (debug)
        {

            GetPointDraf();

            
            

        }
        LookToPlayer();
    }
    void FixedUpdate()
    {


        //LookToPlayer();
    }


    void LookToPlayer()
    {
      // transform.LookAt(player);

        int step_points = 0;

        int to_points = 0;

        for (int i = 0; i < points_bezier.Length - 3; i += 3)
        {
            if (Vector3.Distance(cursor.position, points_bezier[i].position) <= 0  )
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

        float lien_x_points = Vector3.Distance(points_bezier[to_points].position,  points_bezier[step_points].position);

        float pluer_delta_phat = Vector3.Distance(cursor.position,  points_bezier[step_points].position);

        float procent = (pluer_delta_phat * 100 / lien_x_points) * 0.01f;


        if(procent <= 1 && procent > 0)
        {
          

            platform.position = GetPoint(procent, step_points);


        }

      
       
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
