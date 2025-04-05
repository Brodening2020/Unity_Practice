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

        // �C���X�y�N�^�[�Ŋ��蓖�Ă��Ă��Ȃ��ꍇ�� GetComponent ���g�p
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