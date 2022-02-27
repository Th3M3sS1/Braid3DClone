using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BraidTimeWalkClone.Ability
{
    public class TimeWalkAbility : Ability
    {
        // Setting fields for ability
        //[SerializeField]
        //private int maxTimeWalkData = 30;
        //[SerializeField]
        //private float secondsBTWData = 0.1f;
        //[SerializeField]
        //private float timeWalkDuration = 1.25f;

        public int maxTimeWalkData = 30;
        //[SerializeField]
        public float secondsBTWData = 0.1f;
        //[SerializeField]
        public float timeWalkDuration = 1.25f;

        // Setting fields for player
        private CameraController cameraController;
        //private bool canCollectTimeWalkData = true;
        //private float currentDataTimer = 0f;

        public bool canCollectTimeWalkData = true;
        public float currentDataTimer = 0f;
        public float totalTimeBeforeAbility = 0f;
        // Setting class for storing timewalk data
        //[System.Serializable]
        //private class TimeWalkData
        //{
        //    public Vector3 objectPosition;
        //    public Quaternion objectRotation;
        //    public Vector3 cameraPosition;
        //    public Quaternion cameraRotation;
        //}

        public class TimeWalkData
        //    public Vector3 objectPosition;
        {
            public Vector3 objectPosition;
            public Quaternion objectRotation;
            public Vector3 cameraPosition;
            public Quaternion cameraRotation;
        }

        // Setting list to store timewalk data
        [SerializeField]
        //private List<TimeWalkData> timeWalkData = new List<TimeWalkData>();
        public List<TimeWalkData> timeWalkData = new List<TimeWalkData>();
        //Is this game object Player or not
        public bool isPlayer;

        public virtual void Start()
        {
            if (gameObject.GetComponent<PlayerController>())
            {
                isPlayer = true;
                cameraController = GetComponentInChildren<CameraController>();
            }
            else
                isPlayer = false;
                
        }

        #region Region for limited TIME WALK ability
        
        private void Update()
        {
            // Adding time when the update starts
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
                Debug.DrawLine(timeWalkData[i].objectPosition, timeWalkData[i + 1].objectPosition, Color.red);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(Cast());

                // Stopping the rotation of player and camera
                if(isPlayer)
                    cameraController.Lock(true);
            }
        }

        // Getting data
        public TimeWalkData GetTimeWalkData()
        {
            if (isPlayer)
            {
                return new TimeWalkData
                {
                    objectPosition = transform.position,
                    objectRotation = transform.rotation,
                    cameraPosition = cameraController.transform.position,
                    cameraRotation = cameraController.transform.rotation
                };
            }
            else
            {
                return new TimeWalkData
                {
                    objectPosition = transform.position,
                    objectRotation = transform.rotation,
                };
            }            
        }

        // Overriding the cast method from Ability
        public override IEnumerator Cast()
        {
            // Stop collecting data when ability is performed
            canCollectTimeWalkData = false;

            // Seeting fields for calculation
            float secondsForEachData = timeWalkDuration / timeWalkData.Count;
            Vector3 currentDataStartPosition = transform.position;
            Quaternion currentDataStartRotation = transform.rotation;
            Vector3 currentDataCameraPosotion = Vector3.zero;
            Quaternion currentDataCameraRotation = Quaternion.identity;

            if (isPlayer)
            {
                currentDataCameraPosotion = cameraController.transform.position;
                currentDataCameraRotation = cameraController.transform.rotation;
            }
            

            // Only perform time walk if there are any data in list
            while (timeWalkData.Count > 0)
            {
                // Set the ability time to zero
                float abilityTime = 0;

                // Performing the ability untill next list's data
                while (abilityTime < secondsForEachData)
                {
                    transform.position = Vector3.Lerp(currentDataStartPosition, timeWalkData[timeWalkData.Count - 1].objectPosition, abilityTime / secondsForEachData);

                    transform.rotation = Quaternion.Slerp(currentDataStartRotation, timeWalkData[timeWalkData.Count - 1].objectRotation, abilityTime / secondsForEachData);

                    if (isPlayer)
                    {
                        //cameraController.transform.position = Vector3.Lerp(currentDataCameraPosotion, timeWalkData[timeWalkData.Count - 1].cameraPosition, abilityTime / secondsForEachData);

                        cameraController.transform.rotation = Quaternion.Slerp(currentDataCameraRotation, timeWalkData[timeWalkData.Count - 1].cameraRotation, abilityTime / secondsForEachData);
                    }                    

                    abilityTime += Time.deltaTime;

                    yield return null;
                }

                // Adding next list's data to current data
                currentDataStartPosition = timeWalkData[timeWalkData.Count - 1].objectPosition;
                currentDataStartRotation = timeWalkData[timeWalkData.Count - 1].objectRotation;

                if (isPlayer)
                {
                    currentDataCameraPosotion = timeWalkData[timeWalkData.Count - 1].cameraPosition;
                    currentDataCameraRotation = timeWalkData[timeWalkData.Count - 1].cameraRotation;
                }
                
                //positionCam = timeWalkData[0].cameraPosition;

                // Remove performed data
                timeWalkData.RemoveAt(timeWalkData.Count - 1);                
            }

            if(isPlayer)
                cameraController.Lock(false);

            canCollectTimeWalkData = true;
        }
        #endregion

        #region Region for Unlimited TIMEWALK/ OR LAST STARTING POINT abiity
        /*
        private void Update()
        {
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
                Debug.DrawLine(timeWalkData[i].objectPosition, timeWalkData[i + 1].objectPosition, Color.red);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Stop collecting data when ability is performed
                canCollectTimeWalkData = false;

                StartCoroutine(Cast());                

                // Stopping the rotation of player and camera
                if (isPlayer)
                    cameraController.Lock(true);
            }

            else if (Input.GetKeyUp(KeyCode.E))
            {
                StopCoroutine(Cast());
                timeWalkData.Clear();                
                totalTimeBeforeAbility = 0f;

                if (isPlayer)
                    cameraController.Lock(false);

                canCollectTimeWalkData = true;
            }
        }

        // Getting data
        public TimeWalkData GetTimeWalkData()
        {
            if (isPlayer)
            {
                return new TimeWalkData
                {
                    objectPosition = transform.position,
                    objectRotation = transform.rotation,
                    cameraPosition = cameraController.transform.position,
                    cameraRotation = cameraController.transform.rotation
                };
            }
            else
            {
                return new TimeWalkData
                {
                    objectPosition = transform.position,
                    objectRotation = transform.rotation,
                };
            }
        }

        // Overriding the cast method from Ability
        public override IEnumerator Cast()
        {
            // Stop collecting data when ability is performed
            //canCollectTimeWalkData = false;

            // Seeting fields for calculation
            float secondsForEachData = totalTimeBeforeAbility / timeWalkData.Count;
            Vector3 currentDataStartPosition = transform.position;
            Quaternion currentDataStartRotation = transform.rotation;
            Vector3 currentDataCameraPosotion = Vector3.zero;
            Quaternion currentDataCameraRotation = Quaternion.identity;

            if (isPlayer)
            {
                currentDataCameraPosotion = cameraController.transform.position;
                currentDataCameraRotation = cameraController.transform.rotation;
            }

            // Only perform time walk if there are any data in list
            while (timeWalkData.Count > 0)
            {
                // Set the ability time to zero
                float abilityTime = 0;

                // Performing the ability untill next list's data
                while (abilityTime < secondsForEachData && !canCollectTimeWalkData)
                {
                    transform.position = Vector3.Lerp(currentDataStartPosition, timeWalkData[timeWalkData.Count - 1].objectPosition, abilityTime / secondsForEachData);

                    transform.rotation = Quaternion.Slerp(currentDataStartRotation, timeWalkData[timeWalkData.Count - 1].objectRotation, abilityTime / secondsForEachData);

                    if (isPlayer)
                    {
                        //cameraController.transform.position = Vector3.Lerp(currentDataCameraPosotion, timeWalkData[timeWalkData.Count - 1].cameraPosition, abilityTime / secondsForEachData);

                        cameraController.transform.rotation = Quaternion.Slerp(currentDataCameraRotation, timeWalkData[timeWalkData.Count - 1].cameraRotation, abilityTime / secondsForEachData);
                    }

                    abilityTime += Time.deltaTime;

                    yield return null;
                }

                // Adding next list's data to current data
                if (!canCollectTimeWalkData)
                {
                    currentDataStartPosition = timeWalkData[timeWalkData.Count - 1].objectPosition;
                    currentDataStartRotation = timeWalkData[timeWalkData.Count - 1].objectRotation;

                    if (isPlayer)
                    {
                        currentDataCameraPosotion = timeWalkData[timeWalkData.Count - 1].cameraPosition;
                        currentDataCameraRotation = timeWalkData[timeWalkData.Count - 1].cameraRotation;
                    }
                }
                //positionCam = timeWalkData[0].cameraPosition;

                // Remove performed data
                if (!canCollectTimeWalkData)
                    timeWalkData.RemoveAt(timeWalkData.Count - 1);
            }

            //if (isPlayer)
            //    cameraController.Lock(false);
            //
            //canCollectTimeWalkData = true;
        }*/
        #endregion
    }
}