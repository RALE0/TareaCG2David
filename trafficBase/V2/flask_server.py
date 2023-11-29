from flask import Flask, request, jsonify
from model import CityModel

app = Flask("CityModelServer")
city_model = None

@app.route('/init', methods=['POST'])
def init_model():
    global city_model
    print("Received request to initialize model.")
    numero_coches_max = int(request.form.get('numero_coches_max', 10))
    print(f"Initializing model with max number of cars: {numero_coches_max}")
    city_model = CityModel(numero_coches_max)
    print("Model initialized successfully.")
    return jsonify({"message": "Modelo de ciudad iniciado con éxito"})

@app.route('/getAgents', methods=['GET'])
def get_agents():
    if city_model is None:
        print("Error: Model not initialized.")
        return jsonify({"error": "Modelo no inicializado"}), 400

    print("Getting agent data.")
    agent_data = city_model.get_agent_data()
    print(f"Agent data retrieved: {agent_data}")
    return jsonify({'positions': agent_data})

@app.route('/getObstacles', methods=['GET'])
def get_obstacles():
    if city_model is None:
        print("Error: Model not initialized.")
        return jsonify({"error": "Modelo no inicializado"}), 400

    print("Getting obstacle data.")
    obstacle_data = city_model.get_obstacle_data()
    print(f"Obstacle data retrieved: {obstacle_data}")
    return jsonify({'positions': obstacle_data})

@app.route('/update', methods=['GET'])
def update_model():
    global city_model
    if city_model is None:
        print("Error: Model not initialized.")
        return jsonify({"error": "Modelo no inicializado"}), 400

    print("Updating model.")
    city_model.step()
    print("Model updated.")
    return jsonify({'message': 'Modelo actualizado'})

if __name__ == '__main__':
    print("Starting CityModelServer on http://localhost:8585")
    app.run(host="localhost", port=8585, debug=True)
