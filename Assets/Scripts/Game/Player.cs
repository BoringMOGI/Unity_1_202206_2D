using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Attackable weapon;
    [SerializeField] Movement2D movement;
    [SerializeField] int hp;
    [SerializeField] float godTime;         // ���� �ð�.

    // Rigidbody2D ������Ʈ�� �����ϴ� ����.
    Rigidbody2D rigid;
    new Collider2D collider2D;

    bool isAlive => hp > 0;     // �÷��̾��� ���� ����.
    int score;                  // ���� ����.
    bool isLockControl;         // ĳ���� ��Ʈ�� ����.
    bool isPushForce;           // ���� ������ �¾Ƽ� ���� ������ ���ư��� �ִ�.
    
    void Start()
    {
        // GetComponent : ������Ʈ���� �پ��ִ� Rigidbody2D ������Ʈ�� �˻��Ѵ�.
        rigid = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

        GameUI.Instance.UpdateHpImage(hp);  // HP �̹��� ������Ʈ.
        weapon.Switch(true);                // ���⸦ �Ҵ�.
    }

    // �� �����Ӹ��� 1ȸ�� �Ҹ��� �Լ�.
    void Update()
    {
        // !bool : not����
        // true�� false, false�� true.
        if (isLockControl || isPushForce || !isAlive)
            return;

        // GetAxisRaw : -1 or 0 or 1
        // GetAxis    : -1.0f ~ 1.0f
        float x = Input.GetAxis("Horizontal");
        movement.Move(x);                           // �̵�.
        if (x < 0)
            movement.FlipX(true);
        else if (x > 0)
            movement.FlipX(false);

        if (Input.GetKeyDown(KeyCode.Space))        // ����.
        {
            bool isJump = movement.Jump();
            if (isJump)
                AudioManager.Instance.PlaySE("Jump");
        }
    }  

    // �⺻ �Ű�����
    // => ���ڸ� �ѱ��� ������ �⺻������ ����ϰڴ�.
    public void SwitchLockControl(bool isLock, bool isStopVelocity = true)
    {
        isLockControl = isLock;
        if(isLock)
        {
            // �÷��̾��� ��Ʈ�� ���� �ɸ� ��������
            // ���� X�� �ӵ��� ������ΰ� ������ ���ΰ�?
            float velocityX = isStopVelocity ? 0 : rigid.velocity.x;
            rigid.velocity = new Vector2(velocityX, rigid.velocity.y);
        }
    }

    public void OnDead()
    {
        hp = 0;
        GameUI.Instance.UpdateHpImage(hp);
        AudioManager.Instance.StopBGM();

        SwitchLockControl(true, false);                     // ��Ʈ�� ����.
        collider2D.isTrigger = true;                        // Ʈ���ŷ� ����.
        GameUI.Instance.SwitchClearPanel(true, false);      // Ŭ���� �г� Ȱ��ȭ.
        FollowCamera.Instance.ResetTarget();                // ī�޶� ����.
        weapon.Switch(false);                               // ���� ��Ȱ��ȭ.
    }
    public void OnDamage(bool isLeft, float force)
    {
        if (!isAlive)
            return;

        Vector2 direction = Vector2.up;
        direction += Vector2.right * (isLeft ? -1f : 1f);

        isPushForce = true;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(direction * force, ForceMode2D.Impulse);

        hp -= 1;
        GameUI.Instance.UpdateHpImage(hp);
        AudioManager.Instance.PlaySE("Hit");

        if (isAlive)
        {
            StartCoroutine(IEGodMode());
            StartCoroutine(IEForceLock());
        }
        else
        {
            OnDead();
        }
    }

    IEnumerator IEGodMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Player_God");
        weapon.Switch(false);                               // ���⸦ ����.

        float time = godTime;
        float blinkTime = 0.0f;
        bool isLight = true;
        while((time -= Time.deltaTime) > 0.0f)
        {
            if ((blinkTime += Time.deltaTime) >= 0.1f)
            {
                spriteRenderer.color = isLight ? Color.red : Color.white;
                isLight = !isLight;
                blinkTime = 0.0f;
            }
            
            yield return null;      // ��� ����ϰڴ�.
        }

        spriteRenderer.color = Color.white;
        weapon.Switch(true);                                // ���⸦ �Ҵ�.
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    IEnumerator IEForceLock()
    {
        while(true)
        {
            if (rigid.velocity.y <= 0f && movement.isGrounded)
                break;

            yield return null;
        }

        isPushForce = false;
    }
    public void GetScore()
    {
        Debug.Log("������ 1 ����!");
        score += 1;

        GameUI.Instance.UpdateScoreText(score);
    }
}
