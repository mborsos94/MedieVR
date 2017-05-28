using UnityEngine;
using System.Collections;

namespace DigitalRuby.PyroParticles
{
    public class ProjectileShooter : MonoBehaviour
    {
        public GameObject[] Prefabs;
        public float GazeTimeToShoot = 2;
        private float gazeTime = 0;
        private bool isGazedAt = false;
        private GameObject currentPrefabObject;
        private FireBaseScript currentPrefabScript;
        private int currentPrefabIndex;

        private void UpdateEffect()
        {
            if (gazeTime > GazeTimeToShoot)
            {
                StartCurrent();
                gazeTime = 0;
            }
        }

        private void BeginEffect()
        {
            Vector3 pos;
            float yRot = transform.rotation.eulerAngles.y;
            Vector3 forwardY = Quaternion.Euler(0.0f, yRot, 0.0f) * Vector3.forward;
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            Vector3 up = transform.up;
            Quaternion rotation = Quaternion.identity;
            currentPrefabObject = GameObject.Instantiate(Prefabs[currentPrefabIndex]);
            currentPrefabScript = currentPrefabObject.GetComponent<FireConstantBaseScript>();

            if (currentPrefabScript == null)
            {
                // temporary effect, like a fireball
                currentPrefabScript = currentPrefabObject.GetComponent<FireBaseScript>();
                if (currentPrefabScript.IsProjectile)
                {
                    // set the start point near the player
                    rotation = transform.rotation;
                    pos = transform.position + forward + right + up;
                }
                else
                {
                    // set the start point in front of the player a ways
                    pos = transform.position + (forwardY * 10.0f);
                }
            }
            else
            {
                // set the start point in front of the player a ways, rotated the same way as the player
                pos = transform.position + (forwardY * 5.0f);
                rotation = transform.rotation;
            }

            pos.x = 0.0f;

            FireProjectileScript projectileScript = currentPrefabObject.GetComponentInChildren<FireProjectileScript>();
            if (projectileScript != null)
            {
                // make sure we don't collide with other friendly layers
                projectileScript.ProjectileCollisionLayers &= (~UnityEngine.LayerMask.NameToLayer("FriendlyLayer"));
            }

            currentPrefabObject.transform.position = pos;
            currentPrefabObject.transform.rotation = rotation;
        }

        public void StartCurrent()
        {
            StopCurrent();
            BeginEffect();
        }

        private void StopCurrent()
        {
            // if we are running a constant effect like wall of fire, stop it now
            if (currentPrefabScript != null && currentPrefabScript.Duration > 10000)
            {
                currentPrefabScript.Stop();
            }
            currentPrefabObject = null;
            currentPrefabScript = null;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (isGazedAt)
            {
                gazeTime += Time.deltaTime;
            }
            else
            {
                gazeTime = 0;
            }

            UpdateEffect();
        }

        /// Called when the user is looking on a GameObject with this script,
        /// as long as it is set to an appropriate layer (see GvrGaze).
        public void OnGazeEnter()
        {
            SetGazedAt(true);
        }

        /// Called when the user stops looking on the GameObject, after OnGazeEnter
        /// was already called.
        public void OnGazeExit()
        {
            SetGazedAt(false);
        }

        public void SetGazedAt(bool gazedAt)
        {
            isGazedAt = gazedAt;
        }
    }
}