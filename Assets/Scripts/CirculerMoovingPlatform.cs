using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BraidTimeWalkClone.Ability
{
    public class CirculerMoovingPlatform : TimeWalkAbility
    {
        public float rotationSpeed = 50f;
        public Transform centerPoint;

        #region update for limited time ability
        // Update is called once per frame
        
        void Update()
        {
            transform.localPosition = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up) * transform.localPosition;

            currentDataTimer += Time.deltaTime;

            // Checking if the player is not currently time walking
            if (canCollectTimeWalkData)
            {
                // 
                if (currentDataTimer >= secondsBTWData)
                {
                    if (timeWalkData.Count >= maxTimeWalkData)
                    {
                        // Romove data from the start of the list
                        timeWalkData.RemoveAt(0);
                    }

                    // Getting data
                    timeWalkData.Add(GetTimeWalkData());

                    currentDataTimer = 0f;
                }
            }

            // Drawing gizmos of the data we stored earlier
            for (int i = 0; i < timeWalkData.Count - 1; i++)
            {
                Debug.DrawLine(timeWalkData[i].objectPosition, timeWalkData[i + 1].objectPosition, Color.blue);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(Cast());

                // Stopping the rotation of player and camera
                //if (isPlayer)
                //    cameraController.Lock(true);
            }
        }
        
        #endregion

        #region update for unlimited time
        /*
        private void Update()
        {
            transform.localPosition = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up) * transform.localPosition;

            // Adding time when the update starts
            currentDataTimer += Time.deltaTime;
            totalTimeBeforeAbility += Time.deltaTime;

            // Checking if the player is not currently time walking
            if (canCollectTimeWalkData)
            {
                if (currentDataTimer >= secondsBTWData)
                {
                    // Getting data
                    timeWalkData.Add(GetTimeWalkData());

                    currentDataTimer = 0f;
                }
            }

            // Drawing gizmos of the data we stored earlier
            for (int i = 0; i < timeWalkData.Count - 1; i++)
            {
                Debug.DrawLine(timeWalkData[i].objectPosition, timeWalkData[i + 1].objectPosition, Color.blue);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Stop collecting data when ability is performed
                canCollectTimeWalkData = false;

                StartCoroutine(Cast());

                // Stopping the rotation of player and camera
                //if (isPlayer)
                //    cameraController.Lock(true);
            }

            else if (Input.GetKeyUp(KeyCode.E))
            {
                StopCoroutine(Cast());
                timeWalkData.Clear();
                totalTimeBeforeAbility = 0f;

                //if (isPlayer)
                //    cameraController.Lock(false);

                canCollectTimeWalkData = true;
            }
        }
        */
        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.parent = transform;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.parent = null;
            }
        }
    }
}
