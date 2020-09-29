using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform followCharacter = null;
    [SerializeField] float followSpeed = 0.2f;

    private Vector3 cameraDistance;

    private void Start()
    {
        cameraDistance = new Vector3(transform.position.x - followCharacter.position.x,
                                      transform.position.y - followCharacter.position.y,
                                       transform.position.z - followCharacter.position.z);
    }

    private void Update()
    {
        Vector3 targetPos = followCharacter.position;
        transform.DOMove(new Vector3(targetPos.x + cameraDistance.x, transform.position.y, targetPos.z + cameraDistance.z), followSpeed, false);
    }
}
