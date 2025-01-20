using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//déclaration des variables / composants
public class FastEnemy : MonoBehaviour {
    public Transform target;
    public float speed = 5f; // définie la vitesse de déplacement de l'ennemi
    public float rotateSpeed = 0.0025f; // définie la vitesse de rotation de l'ennemi
    public float explosionDistance = 1.5f; //Pour les enemies qui explosent -> distance de l'explosion
    public int explosionDamage = 50; // Combien de dégàts d'explosion
    private Rigidbody2D rb; // Composant Rigidbody2D de l'enemi (défini les intéractions physiques (vitesse, dégàts par le jouer etc...))

    private void Start() {
        rb = GetComponent<Rigidbody2D>(); // Récupérer le composant Rigidbody2D de l'ennemi
    }

    private void Update() {
        if (!target) {
            GetTarget(); // Si la cible n'est pas définie -> chercher le joueur
        } else {
            RotateTowardsTarget(); // tourner vers la cible (joueur)
            CheckExplosionDistance(); // Pour les enemies qui explosent, verifier la distance d'explosion
        }
    }

    private void FixedUpdate(){
        rb.velocity = transform.up * speed; // L'enemie se déplce vers l'avant 
    }

    private void RotateTowardsTarget() {
        Vector2 targetDirection = target.position - transform.position;//Calcul de la direction vers la cible (joueur)
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg; // Calcul de l'angle de rotation (combien de dégrés l'enemie va devoir tourner pour être face au joueur)
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle)); //Création de la retation basé sur l'angle calculé
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void GetTarget(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player) {
            target = player.transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.CompareTag("Player")) {
            Destroy(other.gameObject);
            target = null;
        } else if (other.gameObject.CompareTag("Bullet")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void CheckExplosionDistance() {
        if (target) {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            if (distanceToTarget < explosionDistance) {
                Explode();
            }
        }
    }

    private void Explode() {
        // Appliquer dégâts au joueur
        ApplyDamageToPlayer();
    }

    private void ApplyDamageToPlayer() {
        if (target != null) {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(explosionDamage);
            }
        }
    }
}

public class PlayerHealth : MonoBehaviour {
    public int health = 100;

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        // Vérifier la mort du joueur
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
    }
}