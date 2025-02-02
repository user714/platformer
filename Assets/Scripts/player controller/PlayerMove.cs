using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;


 
public class NewMonoBehaviourScript : MonoBehaviour
{

    Rigidbody _rigidbody;
    Vector3 _move = Vector3.zero;
    float _horizontal_look = 1;
    float arge = 0;
    

    [Header("—корость задаетьс€ в метрах в скунду 1.4м/c скорость шага")] 
    public float speed = 1.4f;
    [Header("—корость врещенни€ персонажа придетьс€ ставить на глаз если не успею задать градусы в секунду")]
    public float speed_rotation = 1f;



    public AnimationCurve jump = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

       
    }

    void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        if (input.x < 0)
        {
            input.x = -1;
        }
        else if (input.x > 0)
        {
            input.x = 1;
        }
        if (input != Vector3.zero)
        {
            _horizontal_look = input.x;
        }

        float z = Mathf.Sin(Mathf.PI * arge);
        float x = Mathf.Cos(Mathf.PI * arge);

        

        if (_horizontal_look != z)
        {
            arge += Time.deltaTime * _horizontal_look * speed_rotation;
        }
        if (arge > 0.5f)
        {
            arge = 0.5f;
        }

        if (arge < -0.5f)
        {
            arge = -0.5f;
        }


        transform.LookAt(new Vector3((x + transform.position.x), transform.position.y, (z + transform.position.z)));
        if (_horizontal_look == z)
        {
            _rigidbody.MovePosition(transform.position + input * Time.fixedDeltaTime * speed);
        }
        
        
      
        
    }
}
