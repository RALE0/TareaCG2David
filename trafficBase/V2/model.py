from mesa import Model
import random
from mesa.time import RandomActivation
from mesa.space import MultiGrid
from agent import *
import json
import requests

class CityModel(Model):
    def __init__(self, numero_coches_max):
        # Cargar el diccionario de datos desde un archivo JSON.
        dataDictionary = json.load(open("trafficBase/V2/city_files/mapDictionary.json"))
        self.traffic_lights = []

        # Cargar el archivo de mapa, donde cada carácter representa un agente.
        with open('trafficBase/V2/city_files/2023_base.txt') as baseFile:
            lines = baseFile.readlines()
            self.width = len(lines[0])-1
            self.height = len(lines)

            self.grid = MultiGrid(self.width, self.height, torus = False) 
            
            self.schedule = RandomActivation(self)
            self.I_locations = []
            self.D_locations = []

            # Crear agentes correspondientes para cada carácter en el archivo de mapa.
            for r, row in enumerate(lines):
                for c, col in enumerate(row):
                    # Crear agentes de carretera, semáforos, obstáculos, puntos de inicio y destino.
                    if col in ["v", "^", ">", "<"]:
                        agent = Road(f"r_{r*self.width+c}", self, dataDictionary[col])
                        self.grid.place_agent(agent, (c, self.height - r - 1))  
                    elif col == "I":
                        agent = Initialization(f"I_{r*self.width+c}", self)
                        self.grid.place_agent(agent, (c, self.height - r - 1))
                        self.I_locations.append((c, self.height - r - 1))
                    elif col == "S":
                        agent = TrafficLight(f"tl_S{r*self.width+c}", self, False, int(dataDictionary[col]), "S")
                        self.grid.place_agent(agent, (c, self.height - r - 1))
                        self.schedule.add(agent)
                        self.traffic_lights.append(agent)
                    elif col == "s":
                        agent = TrafficLight(f"tl_s{r*self.width+c}", self, True, int(dataDictionary[col]), "s")
                        self.grid.place_agent(agent, (c, self.height - r - 1))
                        self.schedule.add(agent)
                        self.traffic_lights.append(agent)
                    elif col == "#":
                        agent = Obstacle(f"ob_{r*self.width+c}", self)
                        self.grid.place_agent(agent, (c, self.height - r - 1))
                    elif col == "D":
                        agent = Destination(f"d_{r*self.width+c}", self)
                        self.grid.place_agent(agent, (c, self.height - r - 1))
                        self.D_locations.append((c, self.height - r - 1))
        
        # Inicializar variables de control, como número de autos, contador de identificación, etc.
        self.num_cars = 0
        self.id_count = 0
        self.numero_coches_max = numero_coches_max
        self.running = True
        self.step_count = 0
        self.used_I_locations = []
        self.arrived_cars = 0
        self.initializeCars()

    def initializeCar(self):
        # Inicializar un coche en una ubicación de inicio aleatoria si es posible.
        if self.I_locations and self.D_locations:
            random_I_location = random.choice(self.I_locations)
            random_D_location = random.choice(self.D_locations)
            # get cell contents from random I location
            # if it contains a car, do not create a new car (return)
            cell_contents = self.grid.get_cell_list_contents([random_I_location])
            for content in cell_contents:
                if isinstance(content, Car):
                    return

            car_agent = Car(1000 + self.id_count, self, random_D_location)  
            self.grid.place_agent(car_agent, random_I_location)
            self.schedule.add(car_agent)
            self.num_cars += 1
            self.id_count += 1
            print("NUMBER OF CARS", self.num_cars)

    def initializeCars(self):
        # Inicializar coches en todas las ubicaciones de inicio disponibles.
        # Itera sobre todas las ubicaciones de inicio
        for i_location in self.I_locations:
            # Verifica si ya hay suficientes agentes
            if self.num_cars >= self.numero_coches_max:
                break

            # Obtener el contenido de la celda en la ubicación actual
            cell_contents = self.grid.get_cell_list_contents([i_location])
            # Verificar si ya hay un carro en esta ubicación
            if not any(isinstance(content, Car) for content in cell_contents):
                # Elegir una ubicación de destino al azar
                random_D_location = random.choice(self.D_locations)
                # Crear un nuevo carro y añadirlo al modelo y a la agenda
                car_agent = Car(1000 + self.id_count, self, random_D_location)  
                self.grid.place_agent(car_agent, i_location)
                self.schedule.add(car_agent)
                self.num_cars += 1
                self.id_count += 1
                print("NUMBER OF CARS", self.num_cars)

    def step(self):
        '''Advance the model by one step.'''
         # Actualizar todos los agentes y realizar acciones según el estado actual del modelo.
        self.schedule.step()
        print ("STEP: ", self.schedule.steps)
        print ("ARRIVED CARS: ", self.arrived_cars)
        self.step_count += 1 

        # Inicializar más coches cada ciertos pasos.
        if self.step_count % 3 == 0:
            self.initializeCars()
        
        # if self.schedule.steps % 100 == 0:
        #     print ("STEP: ", self.schedule.steps)
        #     post(self.arrived_cars)
        #     print("POSTED NUMBER OF CARS")
        
        # Detener la ejecución después de una cantidad específica de pasos.    
        if self.step_count == 1000 or self.schedule.steps == 1000:
            self.running = False
        
    
    def get_agent_data(self):
        # Obtener y devolver los datos de los agentes tipo coche.
        agent_data = []
        for agent in self.schedule.agents:
            if isinstance(agent, Car):  # Puedes ajustar esto según los tipos de agentes que tengas
                agent_info = {
                    "id": agent.unique_id,
                    "x": agent.pos[0],
                    "y": agent.pos[1],
                }
                agent_data.append(agent_info)
        return agent_data
    
    def get_traffic_light_data(self):
        # Obtener y devolver los datos de los semáforos.
        traffic_light_data = []
        for agent in self.schedule.agents:
            if isinstance(agent, TrafficLight):
                traffic_light_info = {
                    "id": agent.unique_id,
                    "x": agent.pos[0],
                    "y": agent.pos[1],
                    "state": agent.state,
                    "lightType": agent.light_type,
                    "timeToChange": agent.timeToChange,
                    "trafficLightStates": agent.traffic_light_states
                }
                traffic_light_data.append(traffic_light_info)
        return traffic_light_data

def post(arrived_cars):
    # Enviar datos (número de coches llegados) a un servidor remoto.
    url = "http://52.1.3.19:8585/api/"
    endpoint = "attempts"

    data = {
        "year" : 2023,
        "classroom" : 302,
        "name" : "Vieyra y Cantú - Compu 2",
        "num_cars": arrived_cars
    }

    headers = {
        "Content-Type": "application/json"
    }

    response = requests.post(url+endpoint, data=json.dumps(data), headers=headers)

    print("Request " + "successful" if response.status_code == 200 else "failed", "Status code:", response.status_code)
    print("Response:", response.json())