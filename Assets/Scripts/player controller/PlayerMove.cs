using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


 
public class NewMonoBehaviourScript : MonoBehaviour
{

    Rigidbody _rigidbody;
    Vector3 _move = Vector3.zero;
    float _horizontal_look = 1;
    float _arge = 0;
    float _jump_move_x = 0;
    bool _jump_play = false;
    bool _jump_move = false;
    bool _jump_collider_bootom = false;
    Vector3 _start_jump_position = Vector3.zero;


    [Header("Скорость задаеться в метрах в скунду 1.4м/c скорость шага")] 
    public float speed = 1.4f;
    [Header("Скорость врещенния персонажа придеться ставить на глаз если не успею задать градусы в секунду")]
    public float speed_rotation = 1f;
    [Header("Нстройка амплетуды прыжка")]
    public AnimationCurve jump_amplitude = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
    [Header("Время прыжка в секундах")]
    public float jump_time = 1;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

    }

    void jampCollision(Collision collisionInfo)
    {
        for (int i = 0; i < collisionInfo.contacts.Length; i++)
        {
            Vector3 point = collisionInfo.contacts[i].point - transform.position;
            if ((point.y < -1 || point.y > 1))
            {
                JumpStop();

            }


            if ((point.y <= -1))
            {
                _jump_collider_bootom = true;

                
            }



             

            if ((point.x < -0.5f || point.x > 0.5f) && point.y > 0)
            {
                _jump_move = false;
            }
        }

    }

     void OnCollisionEnter(Collision collisionInfo)
    {
        jampCollision(collisionInfo);

        //_jump_move = false;
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        jampCollision(collisionInfo);

        //_jump_move = false;
    }

    

    void JumpStop()
    {
        _jump_play = false;
        _jump_move_x = 0;
        _rigidbody.constraints =  RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
      
        //_rigidbody.isKinematic = false;

        //Debug.Log("Завершаю прыжок");
    }
    void Jump()
    {
         


        if (_jump_play && _jump_move_x == 0)
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            _start_jump_position = transform.position;
            //Debug.Log("Начинаю прыжок");
        }

        Keyframe start_key = jump_amplitude.keys[0];
        Keyframe finish_key = jump_amplitude.keys[jump_amplitude.length - 1];
        float len_jump = finish_key.time - start_key.time;

        if (_jump_play)
        {
            bool exit_jump =  false;

            _jump_move_x += ((len_jump / jump_time) * Time.fixedDeltaTime);
            
            //_rigidbody.isKinematic = true;
            float posdition_x = transform.position.x;
            if (_jump_move)
            {
                posdition_x = (_start_jump_position.x + (_jump_move_x * _horizontal_look));
            }

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(new Vector3(posdition_x, _start_jump_position.y + jump_amplitude.Evaluate(len_jump), transform.position.z), transform.TransformDirection(new Vector3(0, -1, 0)), out hit, Mathf.Infinity))

            {   

                if(transform.position.y - (transform.TransformDirection(new Vector3(0, -1, 0)) * hit.distance).y <= 2)
                {
                    exit_jump =  true;
                    JumpStop();
                }
                Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0, -1, 0)) * hit.distance, Color.yellow);
                Debug.Log("Did Hit" + (transform.position.y - (transform.TransformDirection(new Vector3(0, -1, 0)) * hit.distance).y));
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0, -1, 0)) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }

            if(exit_jump == false)
            {
                transform.position = new Vector3(posdition_x, _start_jump_position.y + jump_amplitude.Evaluate(_jump_move_x), transform.position.z);


            }




            //Debug.Log("y =" + jump_amplitude.Evaluate(_jump_move_x) + "x = " + _jump_move_x);
        }


        if (_jump_move_x >= len_jump)
        {
            JumpStop();
            
        }

        if (_jump_play == false)
        {
            _jump_move = false;

        }
    }
    void FixedUpdate()
    {
        // пенести  сюда логику 

        Jump();
    }
    void Update()
    {
        bool _jump_move_ = false;
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        if (_jump_play == false && input != Vector3.zero)
        {
            _jump_move_ = true;
        }
        else
        {
            _jump_move_ = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _jump_collider_bootom)
        {
            if (_jump_move_)
            {
                _jump_move = true;
            }
            
            _jump_play = true;
            _jump_collider_bootom = false;


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
        if (input != Vector3.zero && _jump_play == false)
        {
            _horizontal_look = input.x;
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
 
         transform.LookAt(new Vector3((x + transform.position.x), transform.position.y, (z + transform.position.z)));
         if (_horizontal_look == z)
         {
             _rigidbody.MovePosition(transform.position + input * Time.fixedDeltaTime * speed);
         }

       




    }
}
