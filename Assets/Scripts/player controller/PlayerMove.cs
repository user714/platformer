using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


 
public class NewMonoBehaviourScript : MonoBehaviour
{

    Rigidbody _rigidbody;
    [Header("Скорость задаеться в метрах в скунду 1.4м/c скорость шага")] 
    public float speed = 1.4f;


  
    public AnimationCurve jump = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

       
    }

    void FixedUpdate()
    {
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        _rigidbody.MovePosition(transform.position + m_Input * Time.fixedDeltaTime * speed);
      
        Debug.Log(jump.Evaluate(Time.time));
       // transform.position = new Vector3(transform.position.x, jump.Evaluate(Time.time), transform.position.z);
    }
}
