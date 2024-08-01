using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS.Gameplay
{
    /// <summary>
    /// script responsible for assigning hitboxes to character model
    /// </summary>
    public class HitboxSetup : MonoBehaviour
    {
        List<Transform> myHitboxes = new List<Transform>();
        public void SetHiboxes(GameObject _armatureRoot, Health _health)
        {
            GameTools.SetLayerRecursively(gameObject, 8);

            foreach (HitBox hitBox in transform.GetComponentsInChildren<HitBox>(true))
            {
                Transform hitboxParent = GameTools.GetChildByName(_armatureRoot, hitBox.name);

                if (hitboxParent)
                {
                    hitBox.transform.SetParent(hitboxParent);
                    // hitBox.transform.SetPositionAndRotation(hitboxParent.position, hitboxParent.rotation);
                    hitBox._health = _health;
                    myHitboxes.Add(hitBox.transform);
                }
                else
                {
                    hitBox.gameObject.SetActive(false);
                }
            }
        }
        public void DisableHitboxes()
        {
            foreach (Transform hitbox in myHitboxes)
            {
                hitbox.gameObject.SetActive(false);
            }
        }
    }
}