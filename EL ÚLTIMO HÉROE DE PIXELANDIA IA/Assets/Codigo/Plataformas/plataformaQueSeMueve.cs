using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class plataformaQueSeMueve : MonoBehaviour
{
    [SerializeField] private float tiempoEspera;

    private Rigidbody2D rb;

    [SerializeField] private float velocidadRotacion;

    private Animator animator;

    private bool caida = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (caida) 
        {
            transform.Rotate(new Vector3(0, 0, -velocidadRotacion * Time.deltaTime));
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            StartCoroutine(Caida(other));
        }
    }

    private IEnumerator Caida(Collision2D other)
    {
        animator.SetTrigger("desactivada");
        yield return new WaitForSeconds(tiempoEspera);
        caida = true;
        Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), other.transform.GetComponent<Collider2D>());
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(new Vector2(0.1f, 0));
    }
}