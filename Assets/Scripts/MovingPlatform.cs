using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BraidTimeWalkClone.Ability
{
    public class MovingPlatform : TimeWalkAbility
{
        public Transform[] wayPoints;
        float movementSpeed = 10f;

        float rotationSpeed;

        int currentPoint = 0;


        #region updatefor limited time walk skill
        /*
        // Update is called once per frame
        void Update()
        {
            if (transform.position != wayPoints[currentPoint].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentPoint].position, movementSpeed * Time.deltaTime);
            }
            else
                currentPoint = (currentPoint + 1) % wayPoints.Length;


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
                Debug.DrawLine(timeWalkData[i].objectPosition, timeWalkData[i + 1].objectPosition);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(Cast());

                // Stopping the rotation of player and camera
                //if (isPlayer)
                //    cameraController.Lock(true);
            }
        }
        */
        #endregion

        #region Update For unlimited or last start data

        private void Update()
        {
            if (transform.position != wayPoints[currentPoint].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentPoint].position, movementSpeed * Time.deltaTime);
            }
            else
                currentPoint = (currentPoint + 1) % wayPoints.Length;

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
                Debug.DrawLine(timeWalkData[i].objectPosition, timeWalkData[i + 1].objectPosition);
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
