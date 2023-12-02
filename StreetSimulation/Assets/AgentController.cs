// TC2008B. Sistemas Multiagentes y Gráficas Computacionales
// C# client to interact with Python. Based on the code provided by Sergio Ruiz.
// Octavio Navarro. October 2023

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
// import for text mesh pro
using TMPro;

[Serializable]
public class AgentData
{
    public string id;
    public float x, y, z;

    public AgentData(string id, float x, float y, float z)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.z = z;
    }
}



[Serializable]

public class AgentsData
{
    public List<AgentData> positions;

    public AgentsData() => this.positions = new List<AgentData>();
}

[Serializable]
public class TrafficLightData
{
    public string id;
    public float x, y, z;
    public bool state;
    public string lightType;
    public float timeToChange;
    public Dictionary<string, bool> trafficLightStates;

    public TrafficLightData(string id, float x, float y, float z, bool state, string lightType, float timeToChange, Dictionary<string, bool> trafficLightStates)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.z = z;
        this.state = state;
        this.lightType = lightType;
        this.timeToChange = timeToChange;
        this.trafficLightStates = trafficLightStates;
    }
}

[Serializable]
public class TrafficLightsData
{
    public List<TrafficLightData> positions;

    public TrafficLightsData() => this.positions = new List<TrafficLightData>();
}


public class AgentController : MonoBehaviour
{
    string serverUrl = "http://localhost:8585";
    string getAgentsEndpoint = "/getAgents";
    string getTrafficLightsEndpoint = "/getTrafficLights";
    string sendConfigEndpoint = "/init";
    string updateEndpoint = "/update";
    AgentsData agentsData;
    Dictionary<string, GameObject> agents;
    Dictionary<string, Vector3> prevPositions, currPositions;

    bool updated = false, started = false;

    public GameObject agentPrefab, floor, trafficLightPrefab;
    public int NAgents;
    private int width = 26, height = 26;
    public float timeToUpdate = 5.0f;
    private float timer, dt;

    // we declare a public text for count
    public TextMeshProUGUI countText;
    

    void Start()
    {
        agentsData = new AgentsData();

        prevPositions = new Dictionary<string, Vector3>();
        currPositions = new Dictionary<string, Vector3>();

        agents = new Dictionary<string, GameObject>();

        floor.transform.localScale = new Vector3((float)width / 10, 1, (float)height / 10);
        floor.transform.localPosition = new Vector3((float)width / 2 - 0.5f, 0, (float)height / 2 - 0.5f);

        timer = timeToUpdate;

        StartCoroutine(SendConfiguration());
    }

    private void Update()
    {
        if (timer < 0)
        {
            timer = timeToUpdate;
            updated = false;
            StartCoroutine(UpdateSimulation());

        }

        if (updated)
        {
            timer -= Time.deltaTime;
            dt = 1.0f - (timer / timeToUpdate);

            foreach (var agent in currPositions)
            {
                Vector3 currentPosition = agent.Value;
                currentPosition.y = 0.2f;

                Vector3 previousPosition = prevPositions[agent.Key];

                agents[agent.Key].transform.localPosition = Vector3.Lerp(previousPosition, currentPosition, dt);
                // Vector3 interpolated = Vector3.Lerp(previousPosition, currentPosition, dt);
                // Vector3 direction = currentPosition - interpolated;
                ApplyTransforms applyTransforms = agents[agent.Key].GetComponent<ApplyTransforms>();

                if (applyTransforms != null)
                {
                    applyTransforms.DoTransform();
                }
                else
                {
                    Debug.LogError("No ApplyTransforms component found");
                }

                // agents[agent.Key].transform.localPosition = interpolated;
                // agents[agent.Key].GetComponent<ApplyTransforms>().DoTransform();
                // if (direction != Vector3.zero) agents[agent.Key].transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
                
                // Quaternion currentRotation = agents[agent.Key].transform.rotation;
                // Vector3 currentEulerAngles = currentRotation.eulerAngles;
                
                // Vector3 newEulerAngles = new Vector3(0, currentEulerAngles.y, 0);
                // agents[agent.Key].transform.rotation = Quaternion.Euler(newEulerAngles);
            }
        }
    }

    IEnumerator UpdateSimulation()
    {
        UnityWebRequest www = UnityWebRequest.Get(serverUrl + updateEndpoint);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log(www.error);
        else
        {
            StartCoroutine(GetAgentsData());
            StartCoroutine(GetTrafficLightsData());
        }
    }

    IEnumerator SendConfiguration()
    {
        WWWForm form = new WWWForm();

        form.AddField("numero_coches_max", NAgents.ToString());

        UnityWebRequest www = UnityWebRequest.Post(serverUrl + sendConfigEndpoint, form);
        www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Configuration upload complete!");
            Debug.Log("Getting Agents positions");

            StartCoroutine(GetAgentsData());
            StartCoroutine(GetTrafficLightsData());
        }
    }

    IEnumerator ActivateAgentWithDelay(GameObject agent, float delay)
    {
        yield return new WaitForSeconds(delay);
        agent.SetActive(true);
    }


    IEnumerator GetAgentsData()
    {
        UnityWebRequest www = UnityWebRequest.Get(serverUrl + getAgentsEndpoint);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            AgentsData newData = JsonUtility.FromJson<AgentsData>(www.downloadHandler.text);
            HashSet<string> receivedAgentIds = new HashSet<string>();

            // set text to
            countText.text = "Cars: " + newData.positions.Count.ToString();

            foreach (AgentData agentData in newData.positions)
            {
                receivedAgentIds.Add(agentData.id);

                Vector3 agentPosition = new Vector3(agentData.x, agentData.z, agentData.y);

                if (!agents.ContainsKey(agentData.id))
                {
                    GameObject newAgent = Instantiate(agentPrefab, agentPosition, Quaternion.identity);

                    agents.Add(agentData.id, newAgent);
                    prevPositions.Add(agentData.id, agentPosition);
                    agents[agentData.id].SetActive(false);
                }
                else
                {
                    prevPositions[agentData.id] = agents[agentData.id].transform.position;

                }

                currPositions[agentData.id] = agentPosition;

                // si la current position es diferente a la previous position
                if (currPositions[agentData.id] != prevPositions[agentData.id])
                {
                    // if not active setActive to true
                    if (!agents[agentData.id].activeSelf)
                    {
                        StartCoroutine(ActivateAgentWithDelay(agents[agentData.id], timeToUpdate));
                    }
                }
            }

            List<string> keysToRemove = new List<string>();
            foreach (var existingAgent in agents)
            {
                if (!receivedAgentIds.Contains(existingAgent.Key))
                {
                    // if it is a car
                    if(!existingAgent.Key.Contains("tl_"))
                    {
                        keysToRemove.Add(existingAgent.Key);
                    }
                    
                }
            }

            foreach (var key in keysToRemove)
            {
                Destroy(agents[key]);
                agents.Remove(key);
                prevPositions.Remove(key);
                currPositions.Remove(key);
            }

            updated = true;
            if (!started) started = true;
        }
    }

    IEnumerator GetTrafficLightsData()
    {
        UnityWebRequest www = UnityWebRequest.Get(serverUrl + getTrafficLightsEndpoint);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("[GetTrafficLightsData] Error: " + www.error);
        }
        else
        {
            TrafficLightsData newData = JsonUtility.FromJson<TrafficLightsData>(www.downloadHandler.text);
            HashSet<string> receivedTrafficLightIds = new HashSet<string>();

            foreach (TrafficLightData trafficLightData in newData.positions)
            {

                receivedTrafficLightIds.Add(trafficLightData.id);

                if (!agents.ContainsKey(trafficLightData.id))
                {
                    GameObject newTrafficLight = Instantiate(trafficLightPrefab, new Vector3(trafficLightData.x, trafficLightData.z, trafficLightData.y), Quaternion.identity);
                    agents.Add(trafficLightData.id, newTrafficLight);
                }
                else
                {
                    // get the traffic light and set the state
                    agents[trafficLightData.id].GetComponent<TrafficLightController>().isGreen = trafficLightData.state;
                }
            }
        }
    }
}
