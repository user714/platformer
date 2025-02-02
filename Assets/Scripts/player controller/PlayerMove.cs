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

       
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        for (int i = 0; i < collisionInfo.contacts.Length; i++)
        {
            Vector3 point = collisionInfo.contacts[i].point - transform.position;
            if ((point.y  < -1 || point.y > 1))
            {
                JumpStop();
 

            }

            Debug.Log(collisionInfo.contacts[i].point - transform.position);

            if ((point.x < -0.5f || point.x > 0.5f))
            {
                _jump_move = false;
            }
        }
       
        //_jump_move = false;
    }

    

    void JumpStop()
    {
        _jump_play = false;
        _jump_move_x = 0;
        //_rigidbody.isKinematic = false;

       //Debug.Log("Завершаю прыжок");
    }
    void Jump()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        if (_jump_play == false && input != Vector3.zero)
        {
            _jump_move = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump_play = true;
      

        }


        if (_jump_play && _jump_move_x == 0)
        {
            _start_jump_position = transform.position;
            //Debug.Log("Начинаю прыжок");
        }

        Keyframe start_key = jump_amplitude.keys[0];
        Keyframe finish_key = jump_amplitude.keys[jump_amplitude.length - 1];
        float len_jump = finish_key.time - start_key.time;

        if (_jump_play)
        {

            _jump_move_x += ((len_jump / jump_time) * Time.deltaTime);

            //_rigidbody.isKinematic = true;
            float posdition_x = transform.position.x;
            if (_jump_move)
            {
                posdition_x = (_start_jump_position.x + (_jump_move_x * _horizontal_look));
            }
            transform.position = new Vector3(posdition_x, _start_jump_position.y + jump_amplitude.Evaluate(_jump_move_x), transform.position.z);



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
    }
    void Update()
    {
        Jump();


        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
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
