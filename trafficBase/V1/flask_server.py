from flask import Flask, request, jsonify
from model import CityModel

app = Flask("CityModelServer")
city_model = None

@app.route('/init', methods=['POST'])
def init_model():
    global city_model
    numero_coches_max = int(request.form.get('numero_coches_max', 10))
    city_model = CityModel(numero_coches_max)
    return jsonify({"message": "Modelo de ciudad iniciado con éxito"})

@app.route('/getAgents', methods=['GET'])
def get_agents():
    if city_model is None:
        return jsonify({"error": "Modelo no inicializado"}), 400

    agent_data = city_model.get_agent_data()
    # Modificación aquí para utilizar la clave 'positions'
    return jsonify({'positions': agent_data})

@app.route('/getObstacles', methods=['GET'])
def get_obstacles():
    if city_model is None:
        return jsonify({"error": "Modelo no inicializado"}), 400

    obstacle_data = city_model.get_obstacle_data()
    # Modificación aquí para utilizar la clave 'positions'
    return jsonify({'positions': obstacle_data})

@app.route('/update', methods=['GET'])
def update_model():
    global city_model
    if city_model is None:
        return jsonify({"error": "Modelo no inicializado"}), 400

    city_model.step()
    return jsonify({'message': 'Modelo actualizado'})

if __name__ == '__main__':
    app.run(host="localhost", port=8585, debug=True)
