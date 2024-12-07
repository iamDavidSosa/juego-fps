using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public Quaternion angulo;
    public float grado;

    public GameObject target;
    public bool atacando;

    public int attackDamage = 1; 
    public float attackCooldown = 2f;
    private bool canAttack = true;

    private int hitCount = 0; // Contador de impactos

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Player");
    }

    void Update()
    {
        Comportamiento_Enemigo();
    }

    public void Comportamiento_Enemigo()
    {
        if (target == null) return;

        PlayerMovement player = target.GetComponent<PlayerMovement>();
        if (player != null && player.isDead) return;

        if (Vector3.Distance(transform.position, target.transform.position) > 20)
        {
            Final_Animacion();
            ani.SetBool("Run", false);
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 2)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            switch (rutina)
            {
                case 0:
                    ani.SetBool("Walk", false);
                    break;
                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;
                case 2:
                    transform.rotation = Quaternion.Slerp(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * 4 * Time.deltaTime);
                    ani.SetBool("Walk", true);
                    break;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 5 && !atacando)
            {
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 120 * Time.deltaTime);
                ani.SetBool("Walk", false);

                ani.SetBool("Run", true);
                transform.Translate(Vector3.forward * 8 * Time.deltaTime);

                ani.SetBool("Ataque", false);
            }
            else
            {
                ani.SetBool("Run", false);
                ani.SetBool("Walk", false);
                if (canAttack)
                {
                    atacando = true;
                    ani.SetBool("Ataque", true);
                    AttackPlayer();
                }
            }
        }
    }

    public void Final_Animacion()
    {
        ani.SetBool("Ataque", false);
        atacando = false;
    }

    private void AttackPlayer()
    {
        if (target == null) return;

        PlayerMovement player = target.GetComponent<PlayerMovement>();
        if (player != null && !player.isDead)
        {
            player.TakeDamage(attackDamage);
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        atacando = false;
    }

    public void TakeDamage()
    {
        hitCount++;
        print("Impactos recibidos: " + hitCount);

        if (hitCount >= 5)
        {
            Die();
        }
    }

    private void Die()
    {
        print("¡Enemigo muerto!");
        ani.SetTrigger("Die"); // Ejecuta la animación de muerte
        Destroy(gameObject, 1f); // Desactiva el enemigo después de un tiempo para ver la animación
    }
}
