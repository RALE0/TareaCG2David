import requests

# Configuración de la URL base del servidor
BASE_URL = "http://127.0.0.1:5000"

def test_init_model():
    response = requests.post(f"{BASE_URL}/init")
    print("Init Model:", response.json())

def test_get_agents():
    response = requests.get(f"{BASE_URL}/getAgents")
    print("Get Agents:", response.json())

def test_get_obstacles():
    response = requests.get(f"{BASE_URL}/getObstacles")
    print("Get Obstacles:", response.json())

def test_update_model():
    response = requests.get(f"{BASE_URL}/update")
    print("Update Model:", response.json())

if __name__ == "__main__":
    test_init_model()
    test_get_agents()
    test_get_obstacles()
    test_update_model()
