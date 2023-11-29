from flask import Flask, request, jsonify
from model import CityModel  # Asegúrate de importar tu modelo de Mesa aquí

app = Flask(__name__)
model = None

@app.route('/init', methods=['POST'])
def init_model():
    global model
    # Aquí, puedes agregar código para personalizar la inicialización si es necesario
    model = CityModel(20)
    # model.run_model()  # O cualquier método que tengas para iniciar tu modelo
    return jsonify({"message": "Modelo inicializado"})

@app.route('/getAgents', methods=['GET'])
def get_agents():
    global model
    # Aquí, necesitas un método para obtener los datos de los agentes de tu modelo
    agent_data = model.get_agent_data()  # Este método debe ser implementado en tu modelo
    return jsonify(agent_data)

@app.route('/getObstacles', methods=['GET'])
def get_obstacles():
    global model
    # Similar a get_agents, pero para obtener datos de obstáculos
    obstacle_data = model.get_obstacle_data()  # Este método debe ser implementado en tu modelo
    return jsonify(obstacle_data)

@app.route('/update', methods=['GET'])
def update_model():
    global model
    model.step()  # Avanza un paso en la simulación
    return jsonify({"message": "Modelo actualizado"})

if __name__ == '__main__':
    app.run(debug=True)  # Puedes cambiar el modo de depuración según sea necesario
