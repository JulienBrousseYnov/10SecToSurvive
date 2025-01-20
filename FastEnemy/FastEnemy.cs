using Systelm.colleciton;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : MonoBehaviour {
    public Transform target;
    public float speed = 5f;
    public float rotateSpeed = 0.0025f;
    private rigibody2D rb;

    private void start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (!target) {
            GetTarget();
        } else {
            RotateTowardsTarget();
        }
    }
    private void FixedUpdate(){
        rb.velocity  = transform.up * speed;
    }

    private void RotateTowardsTarget() {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transformation.localRotation = Quaterion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void GetTarget(){
        if (GameObject.FindGameObjectsWithTag("Player")) {
            target = GameObject.FindGameObjectsWithTag("Player").transform;
        }
    }
    private void OnCollisionEnter2D(Collision2D other){
        if (other.GameObject.CompareTag("Player")) {
                Destroy(other.GameObject);
                target = null;
        } else if (other.GameObject.CompareTag("Bullet")) {
                Destroy(other.GameObject);
                Destroy(gameObject);
        }
    }
}