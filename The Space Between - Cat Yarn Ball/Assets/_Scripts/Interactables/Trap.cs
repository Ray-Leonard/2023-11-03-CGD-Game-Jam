using DG.Tweening;
using UnityEngine;
using DG.Tweening;

public class Trap : MonoBehaviour
{

    public int damage = 1;
    public AudioClip sound;

    public bool hasMoveY = false;
    public bool rotate = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StatManager.Instance.DeductHealth(damage);

            if (sound != null)
            {
                AudioSource.PlayClipAtPoint(sound, transform.position);
            }

        }
    }

    private void OnEnable()
    {
        if(hasMoveY){
            transform.DOLocalMoveZ(transform.localPosition.y - 2, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        if (rotate){
            transform.DOLocalRotate(new Vector3(0f, 0f, 361f), 3.0f, RotateMode.LocalAxisAdd)
                        .SetLoops(-1, LoopType.Restart) // -1 indicates infinite loops, LoopType.Restart restarts the loop seamlessly
                        .SetEase(Ease.Linear); // Adjust the ease type if needed
        }

    }
}
