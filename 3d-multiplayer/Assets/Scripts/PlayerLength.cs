using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class PlayerLength : NetworkBehaviour
{
    [SerializeField] private GameObject tailPrefab;

    public NetworkVariable<ushort> length = new(1, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server); //1 is default value
    /* network variable is synched across all instances, 
     * no need to send RPC for updating values,
     * and can subscribe to vaue changes, 
     * must defined in a network behavior
     * server authoratative - only server can change this value unless change permision
     *  rn it is in default
     */

    //ushort == unsigned int16, better performance

    private List<GameObject> _tails;
    private Transform _lastTail;
    private CapsuleCollider _collider;

    //called by the server
    [ContextMenu("Add Length")]
    public void AddLength()
    {
        length.Value += 1;
        InstantiateTail();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _tails = new List<GameObject>();
        _lastTail = transform; //the head
        _collider = GetComponent<CapsuleCollider>();
        if (!IsServer) length.OnValueChanged += LengthChanged; //for clients to know length has changed
    }

    private void LengthChanged(ushort previousValue, ushort newValue)
    {
        Debug.Log("LengthChanged Callback");
        InstantiateTail();
    }
    private void InstantiateTail()
    {
        GameObject tailGameObject = Instantiate(tailPrefab, transform.position, Quaternion.identity);
        tailGameObject.GetComponent<MeshRenderer>().sortingOrder = -length.Value;
        if (tailGameObject.TryGetComponent(out Tail tail))
        {
            tail.networkedOwner = transform;
            tail.followTransform = _lastTail;
            _lastTail = tailGameObject.transform;
            //ignore collision between tails
            Physics.IgnoreCollision(tailGameObject.GetComponent<SphereCollider>(), _collider);
        }
        _tails.Add(tailGameObject);
    }
}
