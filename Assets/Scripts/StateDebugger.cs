using UnityEngine;
using UnityEngine.UI;
public class StateDebugger : MonoBehaviour{
 public Text stateText;
 public FSM_Agent agent;
 void Update() {
 if (agent != null && stateText != null) {
 stateText.text = $"State: {agent.currentState}";
 }
 }
}