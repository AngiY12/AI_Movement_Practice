using UnityEngine;
using System.Collections.Generic;
public class FlockAgent : MonoBehaviour{
 [Header("Flock Parameters")]
 public float moveSpeed = 5f;
 public float rotationSpeed = 3f;
 public float maxSpeed = 8f;

 [Header("Behavior Weights")]
 public float cohesionWeight = 0.5f; // Peso de cohesión
 public float separationWeight = 1.5f; // Peso de separación
 public float alignmentWeight = 0.8f; // Peso de alineación

 [Header("Detection")]
 public float detectionRadius = 5f;
 public float separationRadius = 2f;
 private Vector3 velocity;
 public List<FlockAgent> neighbors = new List<FlockAgent>();
 public static List<FlockAgent> allAgents = new List<FlockAgent>();
 void Start() {
 velocity = Random.insideUnitSphere * moveSpeed;
 allAgents.Add(this);
 }
 void OnDestroy() {
 allAgents.Remove(this);
 }
 void Update() {
 FindNeighbors();
 Vector3 flockForce = CalculateFlockForce();

 // Aplicar fuerza y limitar velocidad máxima
 velocity += flockForce * Time.deltaTime;
 velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

 // Movimiento
 transform.position += velocity * Time.deltaTime;

 // Rotación hacia la dirección de movimiento
 if (velocity.magnitude > 0.1f) {
 Quaternion targetRotation = Quaternion.LookRotation(velocity);
 transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
 }
 }
 void FindNeighbors() {
 neighbors.Clear();
 foreach (FlockAgent other in allAgents) {
 if (other == this) continue;

 float distance = Vector3.Distance(transform.position, other.transform.position);
 if (distance < detectionRadius) {
 neighbors.Add(other);
 }
 }
 }
 Vector3 CalculateFlockForce() {
 Vector3 cohesion = Vector3.zero;
 Vector3 separation = Vector3.zero;
 Vector3 alignment = Vector3.zero;

 if (neighbors.Count == 0) return Vector3.zero;
 // Calcular centro, dirección y separación
 Vector3 center = Vector3.zero;
 Vector3 averageDirection = Vector3.zero;

 foreach (FlockAgent neighbor in neighbors) {
 center += neighbor.transform.position;
 averageDirection += neighbor.velocity.normalized;

 // Separación: alejarse de vecinos demasiado cercanos
 float distance = Vector3.Distance(transform.position, neighbor.transform.position);
 if (distance < separationRadius) {
 Vector3 away = (transform.position - neighbor.transform.position).normalized;
 separation += away * (1f - distance / separationRadius);
 }
 }

 center /= neighbors.Count;
 averageDirection /= neighbors.Count;

 // Cohesión: moverse hacia el centro
 cohesion = (center - transform.position).normalized;

 // Alineación: igualar dirección
 alignment = averageDirection.normalized;

 // Combinar comportamientos con pesos
 Vector3 flockForce = cohesion * cohesionWeight +
 separation * separationWeight +
 alignment * alignmentWeight;

 return flockForce.normalized * moveSpeed;
 }
 // Visualización de radios en la escena
 void OnDrawGizmosSelected() {
 Gizmos.color = Color.cyan;
 Gizmos.DrawWireSphere(transform.position, detectionRadius);

 Gizmos.color = Color.yellow;
 Gizmos.DrawWireSphere(transform.position, separationRadius);
 }
}