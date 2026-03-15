using UnityEngine;
public class Example : MonoBehaviour
{
    public CharacterController controller;

    void Start()
    {
        Transform t =GetComponent<Transform>();
        if(t = null ) {
            Debug.LogError("Transform component not found!");

        }

        // インスペクターで割り当てられていない場合は GetComponent を使用
        if (controller == null)
        {
            controller = this.gameObject.GetComponent<CharacterController>();

            if (controller == null)
            {
                Debug.LogError("CharacterController component not found!");
            }
        }
    }

    void Update()
    {
        if (controller != null)
        {
            if (controller.isGrounded)
            {
                print("CharacterController is grounded");
            }
            else
            {
                print("CharacterController is not grounded");
            }
        }
    }
}