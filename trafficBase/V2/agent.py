from mesa import Agent
import networkx as nx

# Definición de la clase Car (Coche)
class Car(Agent):
    def __init__(self, unique_id, model, destination_pos):
        super().__init__(unique_id, model)
        self.destination_pos = destination_pos  # La posición del nodo de destino del coche
        self.path = None  # El camino que el coche tomará para llegar a su destino
        self.dx, self.dy = 0, 0  # La dirección en la que se mueve el coche

    # Función para determinar el tipo de nodo en una celda
    def defineNodeType(self, cell_contents):
        # Inicialmente se establece el tipo de nodo como espacio vacío (' ')
        node_type = ' '  
        for agent in cell_contents:
            if isinstance(agent, Obstacle):
                return '#'  # Si hay un obstáculo en la celda, el nodo es una pared ('#')
            elif isinstance(agent, Road):
                if agent.direction == "Right":
                    return '>'  # Si hay una carretera hacia la derecha, el nodo es una flecha apuntando a la derecha ('>')
                elif agent.direction == "Left":
                    return '<'  # Si hay una carretera hacia la izquierda, el nodo es una flecha apuntando a la izquierda ('<')
                elif agent.direction == "Up":
                    return '^'  # Si hay una carretera hacia arriba, el nodo es una flecha apuntando hacia arriba ('^')
                elif agent.direction == "Down":
                    return 'v'  # Si hay una carretera hacia abajo, el nodo es una flecha apuntando hacia abajo ('v')
            elif isinstance(agent, Initialization):
                return "I"  # Si hay una posición inicial en la celda, el nodo es una 'I'
            elif isinstance(agent, Destination):
                return "D"  # Si hay un destino en la celda, el nodo es una 'D'
            elif isinstance(agent, TrafficLight):
                if agent.unique_id.startswith("tl_S"):
                    node_type = 'S'  # Si hay un semáforo de tipo 'S', el nodo es una 'S'
                elif agent.unique_id.startswith("tl_s"):
                    node_type = 's'  # Si hay un semáforo de tipo 's', el nodo es una 's'
        return node_type

    # Función para crear un grafo basado en la cuadrícula
    def GraphMaker(self):
        grid = self.model.grid
        G = nx.DiGraph()
        rows, cols = grid.height, grid.width

        for y in range(rows):
            for x in range(cols):
                cell_contents = grid.get_cell_list_contents([(x, y)])
                node = self.defineNodeType(cell_contents)
                if node == '>':
                    # Conexión hacia la derecha (>)
                    if x + 1 < cols:
                        right_cell_contents = grid.get_cell_list_contents([(x + 1, y)])
                        right_node = self.defineNodeType(right_cell_contents)
                        if right_node in ['s', 'D', '>', 'I','^' ]:
                            G.add_edge((x, y), (x + 1, y))
                    # Conexión hacia arriba (^) si es posible
                    if y + 1 < rows:
                        up_cell_contents = grid.get_cell_list_contents([(x, y + 1)])
                        up_node = self.defineNodeType(up_cell_contents)
                        if up_node in ['^', 'D']:
                            G.add_edge((x, y), (x, y + 1))
                    # Conexión hacia abajo (v) si es posible
                    if y - 1 > 0:
                        up_cell_contents = grid.get_cell_list_contents([(x, y - 1)])
                        up_node = self.defineNodeType(up_cell_contents)
                        if up_node in ['v', 'D', "I"]:
                            G.add_edge((x, y), (x, y - 1))
                    # Conexión hacia arriba-derecha (>) si es posible
                    if y - 1 >= 0 and x + 1 < cols:
                        up_right_cell_contents = grid.get_cell_list_contents([(x + 1, y - 1)])
                        up_right_node = self.defineNodeType(up_right_cell_contents)
                        if up_right_node == '>':
                            G.add_edge((x, y), (x + 1, y - 1))
                    # Conexión hacia arriba-izquierda (>) si es posible
                    if y + 1 < rows and x + 1 < cols:
                        up_left_cell_contents = grid.get_cell_list_contents([(x + 1, y + 1)])
                        up_left_node = self.defineNodeType(up_left_cell_contents)
                        if up_left_node == '>':
                            G.add_edge((x, y), (x + 1, y + 1))

                if node == '<':
                    if x - 1 >= 0:
                        left_cell_contents = grid.get_cell_list_contents([(x - 1, y)])
                        left_node = self.defineNodeType(left_cell_contents)
                        if left_node in ['s', 'D', '<', 'I', "v"]:
                            G.add_edge((x, y), (x - 1, y))
                    if y + 1 < rows:
                        up_cell_contents = grid.get_cell_list_contents([(x, y + 1)])
                        up_node = self.defineNodeType(up_cell_contents)
                        if up_node in ['^', 'D']:
                            G.add_edge((x, y), (x, y + 1))
                    if y - 1 > 0:
                        up_cell_contents = grid.get_cell_list_contents([(x, y - 1)])
                        up_node = self.defineNodeType(up_cell_contents)
                        if up_node in ['v', 'D']:
                            G.add_edge((x, y), (x, y - 1))
                    if y + 1 < rows and x - 1 > 0:
                        down_left_cell_contents = grid.get_cell_list_contents([(x - 1, y + 1)])
                        down_left_node = self.defineNodeType(down_left_cell_contents)
                        if down_left_node == '<':
                            G.add_edge((x, y), (x - 1, y + 1))
                    if y - 1 > 0 and x - 1 > 0:
                        down_right_cell_contents = grid.get_cell_list_contents([(x - 1, y - 1)])
                        down_right_node = self.defineNodeType(down_right_cell_contents)
                        if down_right_node == '<':
                            G.add_edge((x, y), (x - 1, y - 1))
                            
                if node == '^':
                    if y + 1 < rows:
                        up_cell_contents = grid.get_cell_list_contents([(x, y + 1)])
                        up_node = self.defineNodeType(up_cell_contents)
                        if up_node in ['^', 'D', 'I', 'S', "<", ">"]:
                            G.add_edge((x, y), (x, y + 1))
                    if x - 1 > 0:
                        left_cell_contents = grid.get_cell_list_contents([(x - 1, y)])
                        left_node = self.defineNodeType(left_cell_contents)
                        if left_node in ['D', '<', 'I']:
                            G.add_edge((x, y), (x - 1, y))
                    if x + 1 < cols:
                        right_cell_contents = grid.get_cell_list_contents([(x + 1, y)])
                        right_node = self.defineNodeType(right_cell_contents)
                        if right_node in ['D', '>', 'I']:
                            G.add_edge((x, y), (x + 1, y))
                    if y + 1  < rows and x + 1 < cols:
                        up_left_cell_contents = grid.get_cell_list_contents([(x + 1, y + 1)])
                        up_left_node = self.defineNodeType(up_left_cell_contents)
                        if up_left_node == '^':
                            G.add_edge((x, y), (x + 1, y + 1))
                    if y + 1 < rows and x - 1 > 0:
                        down_left_cell_contents = grid.get_cell_list_contents([(x - 1, y + 1)])
                        down_left_node = self.defineNodeType(down_left_cell_contents)
                        if down_left_node == '^':
                            G.add_edge((x, y), (x - 1, y + 1))

                if node == 'v':
                    if y - 1 > 0:
                        down_cell_contents = grid.get_cell_list_contents([(x, y - 1)])
                        down_node = self.defineNodeType(down_cell_contents)
                        if down_node in ['v', 'D', 'I', 'S', ">", "<"]:
                            G.add_edge((x, y), (x, y - 1))
                    if x + 1 < cols:
                        right_cell_contents = grid.get_cell_list_contents([(x + 1, y)])
                        right_node = self.defineNodeType(right_cell_contents)
                        if right_node in ['>', 'I', 'D']:
                            G.add_edge((x, y), (x + 1, y))
                    if x - 1 > 0:
                        left_cell_contents = grid.get_cell_list_contents([(x - 1, y)])
                        left_node = self.defineNodeType(left_cell_contents)
                        if left_node in ['<', 'I', 'D']:
                            G.add_edge((x, y), (x - 1, y))
                    
                    if y - 1 > 0 and x - 1 >= 0:
                        left_cell_contents = grid.get_cell_list_contents([(x - 1, y-1)])
                        left_node = self.defineNodeType(left_cell_contents)
                        if left_node == 'v':
                            G.add_edge((x, y), (x - 1, y-1))
                    if y - 1 > 0 and x + 1 < cols:
                        left_cell_contents = grid.get_cell_list_contents([(x + 1, y-1)])
                        left_node = self.defineNodeType(left_cell_contents)
                        if left_node == 'v':
                            G.add_edge((x, y), (x + 1, y-1))

                if node == 'S':
                    if y - 1 > 0:
                        down_cell_contents = grid.get_cell_list_contents([(x, y - 1)])
                        down_node = self.defineNodeType(down_cell_contents)
                        if down_node in ['v', ">"]:
                            G.add_edge((x, y), (x, y - 1))
                    if y + 1 < rows:
                        up_cell_contents = grid.get_cell_list_contents([(x, y + 1)])
                        up_node = self.defineNodeType(up_cell_contents)
                        if up_node in ['^', "<"]:
                            G.add_edge((x, y), (x, y + 1))
                if node == 's':
                    if x + 1 < cols:
                        right_cell_contents = grid.get_cell_list_contents([(x + 1, y)])
                        right_node = self.defineNodeType(right_cell_contents)
                        if right_node in ['>', '^', "v"]:
                            G.add_edge((x, y), (x + 1, y))
                    if x - 1 > 0:
                        left_cell_contents = grid.get_cell_list_contents([(x - 1, y)])
                        left_node = self.defineNodeType(left_cell_contents)
                        if left_node in  ['<', '^', "v"]:
                            G.add_edge((x, y), (x - 1, y))

                if node == 'I':
                    if y + 1 < rows:
                        up_cell_contents = grid.get_cell_list_contents([(x, y + 1)])
                        up_node = self.defineNodeType(up_cell_contents)
                        if up_node == '^':
                            G.add_edge((x, y), (x, y + 1))
                    if y - 1 > 0:
                        down_cell_contents = grid.get_cell_list_contents([(x, y - 1)])
                        down_node = self.defineNodeType(down_cell_contents)
                        if down_node == 'v':
                            G.add_edge((x, y), (x, y - 1))
                    if x - 1 > 0:
                        left_cell_contents = grid.get_cell_list_contents([(x - 1, y)])
                        left_node = self.defineNodeType(left_cell_contents)
                        if left_node == '<':
                            G.add_edge((x, y), (x - 1, y))
                    if x + 1 < cols:
                        right_cell_contents = grid.get_cell_list_contents([(x + 1, y)])
                        right_node = self.defineNodeType(right_cell_contents)
                        if right_node == '>':
                            G.add_edge((x, y), (x + 1, y))
        return G

    # Función para encontrar el camino utilizando A* en el grafo
    def createPath(self, start, end):
        G = self.GraphMaker()
        start_node_type = self.defineNodeType(self.model.grid.get_cell_list_contents([start]))
        end_node_type = self.defineNodeType(self.model.grid.get_cell_list_contents([end]))
        try:
            path = nx.astar_path(G, start, end, heuristic=self.euclideanHeuristic)
            print(path)
            return path
        except nx.NetworkXNoPath:
            print("No path found.")
            return []

    def euclideanHeuristic(self, a, b):
        # Calcula la distancia euclidiana entre dos nodos
        return ((a[0] - b[0])**2 + (a[1] - b[1])**2)**0.5
    
    def availableMove(self, next_position):
        cell_contents = self.model.grid.get_cell_list_contents(next_position)
        for agent in cell_contents:
            if isinstance(agent, TrafficLight) and not agent.state:
                return False  # Cannot move if there's a red traffic light
            
            if isinstance(agent, Car) and agent is not self:
                return False
        return True
    
    def trip_completed(self):
        # si estas donde un semaforo y esta rojo
        cell_contents = self.model.grid.get_cell_list_contents(self.pos)
        for agent in cell_contents:
            if isinstance(agent, TrafficLight) and not agent.state:
                raise Exception(f"Car {self.unique_id} at {self.pos}: Encountered a red traffic light at destination")
                
        if self.pos == self.destination_pos:
            self.model.grid.remove_agent(self)
            self.model.num_cars -= 1
            self.model.arrived_cars += 1
            self.model.schedule.remove(self)

    def move(self):
        if self.path is None or len(self.path) == 0:
            self.path = self.createPath(self.pos, self.destination_pos)
        if self.path and len(self.path) > 0:
            next_position = self.path[0]  # Get the next position
            if self.availableMove(next_position):
                self.path.pop(0)  # Remove the next position from the path
                self.dx, self.dy = next_position[0] - self.pos[0], next_position[1] - self.pos[1]
                self.model.grid.move_agent(self, next_position)  # Move the car to the next position

    def step(self):
        if self.pos == self.destination_pos:
            self.trip_completed()
        else:
            self.move()


class TrafficLight(Agent):
    def __init__(self, unique_id, model, state = False, timeToChange = 10, light_type = "S"):
        super().__init__(unique_id, model)
        self.state = state
        self.timeToChange = timeToChange
        self.light_type = light_type
        self.traffic_light_states = {'S': False, 's': True}

    def colorChange(self, light_type):
        # Toggle the state of the specified light type
        previous_state = self.traffic_light_states[light_type]
        if light_type == 'S':
            self.traffic_light_states['S'] = not self.traffic_light_states['S']
            self.traffic_light_states['s'] = not self.traffic_light_states['S']
        elif light_type == 's':
            self.traffic_light_states['s'] = not self.traffic_light_states['s']
            self.traffic_light_states['S'] = not self.traffic_light_states['s']    

    def step(self):
        if self.model.schedule.steps % self.timeToChange == 0:
            self.colorChange(self.light_type)
            self.state = self.traffic_light_states[self.light_type]

class Road(Agent):
    def __init__(self, unique_id, model, direction= "Left"):
        super().__init__(unique_id, model)
        self.direction = direction

    def step(self):
        pass
    
class Obstacle(Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)

    def step(self):
        pass

class Destination(Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
    def step(self):
        pass

class Initialization(Agent):
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
    def step(self):
        pass