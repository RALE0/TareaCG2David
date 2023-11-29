import requests

# Configuración de la URL base del servidor
BASE_URL = "http://localhost:8585"

def test_init_model():
    response = requests.post(f"{BASE_URL}/init", data={'numero_coches_max': 10})
    print("Init Model Response:", response.json())

def test_get_agents():
    response = requests.get(f"{BASE_URL}/getAgents")
    print("Get Agents Response:", response.json())

def test_get_obstacles():
    response = requests.get(f"{BASE_URL}/getObstacles")
    print("Get Obstacles Response:", response.json())

def test_update_model():
    response = requests.get(f"{BASE_URL}/update")
    print("Update Model Response:", response.json())


if __name__ == "__main__":
    test_init_model()
    while input("Presiona enter para continuar o escribe 'q' para salir: ") != 'q':
        # test_get_agents()
        test_get_obstacles()
        test_update_model()
