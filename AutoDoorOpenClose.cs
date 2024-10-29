using NaughtyAttributes;
using UnityEngine;

public class AutoDoorOpenClose : MonoBehaviour
{
    [SerializeField] private float distanceForInteraction = 5f; //Distance for interaction
    [ReadOnly][SerializeField] private Animator doorAnimator;   //door animator controller!
    [SerializeField] private string doorOpenClipName;           //Door open clip string
    [SerializeField] private string doorCloseClipName;          //Door close clip string
    private Transform _playerTransform;                         //Making Transform variable for player to cache
    private bool _isDoorOpen;                                   //Private bool to check if door is open or not

    //Using OnValidate method to assign reference to variables without strting a game!
    private void OnValidate()
    {
        doorAnimator = GetComponent<Animator>();
    }

    //Caching player transform in the start of the game from its instance
    private void Start()
    {
        _playerTransform = PlayerMovement.instance.transform;
    }

    private void Update()
    {
        //Checking if player is in interact distance
        if(Vector3.Distance(transform.position, _playerTransform.position) < distanceForInteraction)
        {
            //Checking if player looking any interactable object if so open door
            if (PlayerMovement.instance.IsPlayerLookingAtSomething())
            {
                if (!_isDoorOpen)
                {
                    _isDoorOpen = true;
                    doorAnimator.Play(doorOpenClipName);
                }
            }
        }
        else
        {
            if(_isDoorOpen)
            {
                _isDoorOpen = false;
                doorAnimator.Play(doorCloseClipName);
            }
        }
    }

}
