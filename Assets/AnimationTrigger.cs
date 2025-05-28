using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Animator 컴포넌트를 가져옵니다.
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // "P" 키가 눌렸을 때 애니메이션을 실행합니다.
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("애니메이션이 실행되었습니다!");
            animator.SetTrigger("Play"); // 애니메이션 트리거 이름을 설정합니다.
        }
    }
}
