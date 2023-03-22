using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public MeshRenderer[] roofs;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            foreach(MeshRenderer roof in roofs){
                roof.enabled = false;
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            foreach(MeshRenderer roof in roofs){
                roof.enabled = true;
            }
        }
    }
}
