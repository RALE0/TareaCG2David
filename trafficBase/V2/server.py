from agent import *
from model import CityModel
from mesa.visualization import CanvasGrid, Slider, TextElement, ModularServer

class CarTextElement(TextElement):
    def __init__(self):
        pass

    def render(self, model):
        return "Carros: " + str(model.num_cars)
        
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
        print(agent.dx, agent.dy)
        if agent.dx == 0 and agent.dy == 0:
            portrayal["Shape"] = "circle"
            portrayal["Color"] = "black"
            portrayal["Layer"] = 0
            portrayal["r"] = 0.5
        else:
            if agent.dy != 0:
                agent.dx = 0
            # arrowHead
            portrayal = {"Shape": "arrowHead",
                        "Filled": "true",
                        "Layer": 4,
                        "Color": "black",
                        "scale": 0.5,
                        "heading_x": agent.dx,
                        "heading_y": agent.dy}


    return portrayal

width = 0
height = 0

with open('trafficBase/V2/city_files/2023_base.txt') as baseFile:
    lines = baseFile.readlines()
    width = len(lines[0])-1
    height = len(lines)

# 5 parameters: name, default value, min value, max value, step
model_params = {"numero_coches_max": Slider("Numero de coches maximo", 500, 1, 1000, 1)}

print(width, height)
grid = CanvasGrid(agent_portrayal, width, height, 500, 500)

server = ModularServer(CityModel, [grid, CarTextElement()
                                   ], "Traffic Base", model_params)                      
server.port = 8521 # The default
server.launch()