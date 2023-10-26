using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public LayerMask layerMask;

    public float speed;

    protected Vector3 vector;

    public int walkCount;
    protected int currentWalkCount;
    public Animator animator;
}
