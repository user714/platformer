using System.Collections.Generic;
using UnityEngine;
 
using UnityEngine.SceneManagement;
[ExecuteAlways]

public class NewMonoBehaviourScript : MonoBehaviour
{

    CharacterController _rigidbody;
    Vector3 _jump_vector = new Vector3(1,0,0);
    float _horizontal_look = 1;
    float _arge = 0;
    float _jump_move_x = 0;
    bool _jump_play = false;
    bool _jump_move = false;

    GameObject platform;
    float deplata_platform_x;
    Vector3 _jump_pos;




    bool _use_finish_jump_points = false;
    Vector3 _finish_jump_points = Vector3.zero;

    Vector3 _start_jump_position = Vector3.zero;
    Vector3 _start_jump_position_mirror = Vector3.zero;



    [Header("Скорость задаеться в метрах в скунду 1.4м/c скорость шага")] 
    public float speed = 1.4f;
    [Header("Скорость врещенния персонажа придеться ставить на глаз если не успею задать градусы в секунду")]
    public float speed_rotation = 1f;
    [Header("Нстройка амплетуды прыжка")]
    public AnimationCurve jump_amplitude = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
    [Header("Время прыжка в секундах")]
    public float jump_time = 1;
    [Header("Использовать  контроллируйтемый прыжолк")]
    public bool jump_controll  = false;


    void Start()
    {
        _rigidbody = GetComponent<CharacterController>();
    

    }


     

    
  
    List<Vector3> getPointsCollisionJump(AnimationCurve jump_amplitude, Vector3 _start_jump_position, int sectors = 7, int  coint_points = 1, float _horizontal_look = 1)
    {
        List<Vector3> result_points_list = new List<Vector3>();


        Keyframe start_key = jump_amplitude.keys[0];
        Keyframe finish_key = jump_amplitude.keys[jump_amplitude.length - 1];
        float len_jump = finish_key.time - start_key.time;
 

        for (int i = 1; i <= sectors; i++)
        {
           


            Vector3 this_poin = new Vector3(_start_jump_position.x + ((len_jump / sectors) * (i - 1) * _horizontal_look) , _start_jump_position.y + jump_amplitude.Evaluate(((len_jump / sectors) * (i - 1))), _start_jump_position.z);

             


            Vector3 next_poin = new Vector3(_start_jump_position.x + ((len_jump / sectors) * i * _horizontal_look) , _start_jump_position.y + jump_amplitude.Evaluate(((len_jump / sectors) * i)), _start_jump_position.z);

            



            Vector3 direction = this_poin - next_poin;


            Ray ray = new Ray(this_poin, next_poin - this_poin);
            RaycastHit hit;

            Debug.DrawRay(this_poin, next_poin - this_poin, Color.green);

            if (Physics.Raycast(ray, out hit))

            {
                if (hit.collider.name != gameObject.name && hit.collider.name != "platform")
                {
                    if (Vector3.Distance(this_poin, next_poin) > hit.distance && i < sectors)
                    {
                        Debug.DrawLine((hit.point), new Vector3(hit.point.x - 1, hit.point.y, hit.point.z), Color.blue);
                        Debug.DrawLine((hit.point), new Vector3(hit.point.x + 1, hit.point.y, hit.point.z), Color.blue);
                        Debug.DrawLine((hit.point), new Vector3(hit.point.x, hit.point.y - 1, hit.point.z), Color.blue);
                        Debug.DrawLine((hit.point), new Vector3(hit.point.x, hit.point.y + 1, hit.point.z), Color.blue);

                        result_points_list.Add(hit.point);
                    }
                    else if (i == sectors)
                    {
                        Debug.DrawLine((this_poin), hit.point, Color.green);


                        Debug.DrawLine((hit.point), new Vector3(hit.point.x - 1, hit.point.y, hit.point.z), Color.blue);
                        Debug.DrawLine((hit.point), new Vector3(hit.point.x + 1, hit.point.y, hit.point.z), Color.blue);
                        Debug.DrawLine((hit.point), new Vector3(hit.point.x, hit.point.y - 1, hit.point.z), Color.blue);
                        Debug.DrawLine((hit.point), new Vector3(hit.point.x, hit.point.y + 1, hit.point.z), Color.blue);


                        result_points_list.Add(hit.point);
                    }

                     

                    if (result_points_list.Count == coint_points)
                    {
                       // break;
                    }
                }
            }


        }
 
        return result_points_list;

    }


    void JumpStop()
    {
        _jump_play = false;
        _jump_move_x = 0;
      
    }
    void Jump()
    {
         


        if (_jump_play && _jump_move_x == 0)
        {

         
            _start_jump_position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);



            //Debug.Log("Начинаю прыжок");
        }

        Keyframe start_key = jump_amplitude.keys[0];
        Keyframe finish_key = jump_amplitude.keys[jump_amplitude.length - 1];
        float len_jump = finish_key.time - start_key.time;


        if(!_jump_play)
        {
            Vector3 _start_jump_position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            List<Vector3> all_poits = getPointsCollisionJump(jump_amplitude, _start_jump_position, 100, -1, _horizontal_look);
        }


        if (_jump_play && _jump_move_x <= len_jump)
        {


            float posdition_x = transform.position.x;

            _jump_move_x += ((len_jump / jump_time) * Time.fixedDeltaTime);

            if (_jump_move)
            {
                posdition_x = (_start_jump_position.x + (_jump_move_x * _horizontal_look));
            }

            transform.position = new Vector3(posdition_x, _start_jump_position.y + 1 + jump_amplitude.Evaluate(_jump_move_x), transform.position.z);


            List<Vector3> all_poits = getPointsCollisionJump(jump_amplitude, _start_jump_position, 100, 0, _horizontal_look);
            for ( int i = 0;  i < all_poits.Count; i++)
            {
           
                if (Vector3.Distance(all_poits[i], new Vector3(transform.position.x, transform.position.y - 1, transform.position.z)) < 0.2f)
                {
                    Debug.Log("Stop jump");
                    _jump_play = false;
                    JumpStop();
                    break;
                  
                }

            }
            
        }


        if (_jump_move_x > len_jump)
        {


            
           
                    JumpStop();
           
 
        }

        if (_jump_play == false)
        {
            _jump_move = false;

        }
    }
   
    void Update()
    {


        Jump();

        bool _jump_move_ = false;
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, 0);



 



        if (Input.GetKey(KeyCode.Space))
        {
         
            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y , transform.position.z), new Vector3(0, -1,0));
          
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z), Color.blue);
            
                if (hit.collider)
                {
                     
                    if(hit.distance <= 1.09)
                    {
                        if (_jump_play == false && input != Vector3.zero)
                        {
                            _jump_move = true;

                        }
                        _jump_play = true;

                        _jump_vector.x = _horizontal_look;
                    }
                    
                }
            }
        }

        

        //Debug.Log(_jump_play + " " + _jump_collider_bootom);

         
        if (input.x < 0)
        {
            input.x = -1;
        }
        else if (input.x > 0)
        {
            input.x = 1;
        }

        if (input != Vector3.zero && ((jump_controll  && _jump_play) || !_jump_play))
        {
 
            _horizontal_look = input.x;


            
        }

        if (_jump_play && _jump_vector.x != _horizontal_look && jump_controll)
        {

            Keyframe start_key = jump_amplitude.keys[0];
            Keyframe finish_key = jump_amplitude.keys[jump_amplitude.length - 1];
            float len_jump = finish_key.time - start_key.time;

            float delta = ( Mathf.Abs(_start_jump_position.x - transform.position.x) *2 ) * _horizontal_look;


            _jump_vector.x = _horizontal_look;

            _start_jump_position.x -= delta;

        }

        float z = Mathf.Sin(Mathf.PI * _arge);
        float x = Mathf.Cos(Mathf.PI * _arge);

        

        if (_horizontal_look != z)
        {
            _arge += Time.deltaTime * _horizontal_look * speed_rotation;
        }
        if (_arge > 0.5f)
        {
            _arge = 0.5f;
        }

        if (_arge < -0.5f)
        {
            _arge = -0.5f;
        }
        if (!platform)
        {
            transform.LookAt(new Vector3((x + transform.position.x), transform.position.y, (z + transform.position.z)));
        }
        else
        {
            if (_jump_play == false)
            {
                Vector3 tr = platform.gameObject.transform.position;
                tr.x -= deplata_platform_x;
                tr.y += 1.5f;
                transform.position = tr;

                transform.LookAt(new Vector3((x + transform.position.x), transform.position.y, (z + transform.position.z)));
            }
        }
         if (_horizontal_look == z)
         {
            
            // _rigidbody.MovePosition(_rigidbody.position + input * Time.fixedDeltaTime * speed);
            if (_jump_play == false)
            {
                input.y = -2f;
                Vector3 move = input * Time.fixedDeltaTime * speed;

                if (platform)
                {
                    deplata_platform_x -= move.x;
                    //Vector3 delata =  platform.gameObject.transform.position - transform.position;
                    Vector3 tr = platform.gameObject.transform.position;
                    tr.x -= deplata_platform_x;
                    tr.y += 1.5f;
                    transform.position = tr;

                }
                else
                {
                    _rigidbody.Move(move);
                }
 
                
            }
            
         }
         if(transform.position.y < -10)
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
        

        



    }

     

   

    private void OnTriggerEnter(Collider other)
    {
      
        platform = other.gameObject;
        deplata_platform_x = other.transform.position.x - transform.position.x;
        JumpStop();
    }

    private void OnTriggerExit(Collider other)
    {
        platform = null;
       
        // other.gameObject.transform.SetParent(null);
    }
}
