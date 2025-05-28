using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Animator ������Ʈ�� �����ɴϴ�.
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // "P" Ű�� ������ �� �ִϸ��̼��� �����մϴ�.
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("�ִϸ��̼��� ����Ǿ����ϴ�!");
            animator.SetTrigger("Play"); // �ִϸ��̼� Ʈ���� �̸��� �����մϴ�.
        }
    }
}
