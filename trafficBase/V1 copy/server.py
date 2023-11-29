from agent import *
from model import CityModel
from mesa.visualization import CanvasGrid, Slider
from mesa.visualization import ModularServer

def agent_portrayal(agent):
    if agent is None: return
    
    portrayal = {"Shape": "rect",
                 "Filled": "true",
                 "Layer": 1,
                 "w": 1,
                 "h": 1
                 }

    if (isinstance(agent, Road)):
        portrayal["Color"] = "grey"
        portrayal["Layer"] = 0
    
    if (isinstance(agent, Destination)):
        portrayal["Color"] = "lightgreen"
        portrayal["Layer"] = 0

    if (isinstance(agent, Traffic_Light)):
        portrayal["Color"] = "red" if not agent.state else "green"
        portrayal["Layer"] = 0
        portrayal["w"] = 0.8
        portrayal["h"] = 0.8

    if (isinstance(agent, Obstacle)):
        portrayal["Color"] = "cadetblue"
        portrayal["Layer"] = 0
        portrayal["w"] = 0.8
        portrayal["h"] = 0.8

    if (isinstance(agent, Initialization)):
        portrayal["Color"] = "blue"
        portrayal["Layer"] = 0
        portrayal["w"] = 0.8
        portrayal["h"] = 0.8
    
    if (isinstance(agent, Car)):
        dx, dy = 0, 0

        if agent.direction == "Right":
            dx = 1
            dy = 0
        elif agent.direction == "Left":
            dx = -1
            dy = 0
        elif agent.direction == "Up":
            dx = 0
            dy = 1
        elif agent.direction == "Down":
            dx = 0
            dy = -1
        
        if not agent.direction:
            portrayal = {"Shape": "circle",
                                "Filled": "true",
                                "Layer": 4,
                                "Color": "black",
                                "scale": 0.5}
        else:
            portrayal = {"Shape": "arrowHead",
                                "Filled": "true",
                                "Layer": 4,
                                "Color": "black",
                                "scale": 0.5,
                                "heading_x": dx,
                                "heading_y": dy}

    return portrayal

width = 0
height = 0

with open('city_files/2022_base.txt') as baseFile:
    lines = baseFile.readlines()
    width = len(lines[0])-1
    height = len(lines)

# 5 parameters: name, default value, min value, max value, step 
model_params = {"numero_coches_max": Slider("Numero de coches maximo", 10, 0, 100, 1)}

print(width, height)
grid = CanvasGrid(agent_portrayal, width, height, 500, 500)

server = ModularServer(CityModel, [grid], "Traffic Base", model_params)                      
server.port = 8521 # The default
server.launch()