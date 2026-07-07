using UnityEngine;
using UnityEngine.AI;
public class PatrolAgent : MonoBehaviour{
 [Header("Waypoint Configuration")]
 public Transform[] waypoints; // Array de puntos de patrullaje
 public float waypointTolerance = 0.5f; // Distancia para considerar que llegó
 private NavMeshAgent agent;
 private int currentWaypointIndex = 0;
 void Start() {
 agent = GetComponent<NavMeshAgent>();

 // Desactivar auto-braking para movimiento continuo entre puntos
 agent.autoBraking = false;

 // Iniciar el patrullaje
 GoToNextWaypoint();
 }
 void Update() {
 // Verificar si el agente ha llegado al waypoint actual
 if (!agent.pathPending && agent.remainingDistance < waypointTolerance) {
 GoToNextWaypoint();
 }
 }
 void GoToNextWaypoint() {
 // Si no hay waypoints, salir
 if (waypoints.Length == 0) {
 Debug.LogWarning("No waypoints assigned!");
 return;
 }
 // Establecer el destino actual
 agent.SetDestination(waypoints[currentWaypointIndex].position);

 // Avanzar al siguiente waypoint (cíclico)
 currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
 }
 // Método para visualizar los waypoints en la escena
 void OnDrawGizmosSelected() {
 if (waypoints == null || waypoints.Length == 0) return;

 Gizmos.color = Color.blue;
 for (int i = 0; i < waypoints.Length; i++) {
 if (waypoints[i] == null) continue;
 Gizmos.DrawSphere(waypoints[i].position, 0.3f);

 // Dibujar líneas entre waypoints
 int nextIndex = (i + 1) % waypoints.Length;
 if (waypoints[nextIndex] != null) {
 Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
 }
 }
 }
}
