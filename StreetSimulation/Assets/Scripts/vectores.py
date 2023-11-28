#Producto punto 3D

#Importamos la libreria math
import math
import matplotlib.pyplot as plt



#Definimos la funcion producto_punto_3d
def producto_punto_3d(vector1, vector2):
    #Definimos la variable que guardará el resultado del producto_punto
    producto_punto = 0

    #Definimos el ciclo for para recorrer los vectores
    for i in range(len(vector1)):
        #Definimos la variable producto_punto
        producto_punto += vector1[i] * vector2[i]

    #Retornamos el producto punto
    return producto_punto

#Normalizamos vectores
def normalizar(vector):
    #Definimos la variable que guardará el resultado del vector normalizado
    vectorNormalizado = []
    
    magnitud = math.sqrt(vector[0]**2 + vector[1]**2 + vector[2]**2)
    
    for i in range(len(vector)):
        vectorNormalizado.append(vector[i] / magnitud)
    # print ("Vector original: ", vector)
    # print("Vector normalizado: ", vectorNormalizado)
    return vectorNormalizado

def obtener_angulo_entre_vectores(vector1, vector2):
    #Definimos la variable que guardará el resultado del producto_punto
    producto_punto = producto_punto_3d(vector1, vector2)

    # Obtenemos la magnitud de los vectores
    # magnitudv = math.sqrt(pow(2, vector1[0]) + pow(2, vector1[1]) + pow(2, vector1[2]))
    # magnitudu = math.sqrt(pow(2, vector2[0]) + pow(2, vector2[1]) + pow(2, vector2[2]))
    magnitudv = math.sqrt(vector1[0]**2 + vector1[1]**2 + vector1[2]**2)
    magnitudu = math.sqrt(vector2[0]**2 + vector2[1]**2 + vector2[2]**2)
    
    # Sabemos que el producto punto es igual a la magnitud de los vectores por el coseno del angulo entre ellos
    # Por lo tanto, despejamos el angulo entre ellos
    angulo = math.acos(producto_punto / (magnitudv * magnitudu))
        
    # Confirmamos el resultado sustituyendo en la formula del producto punto
    producto_punto2 = magnitudv * magnitudu * math.cos(angulo)
    
    angulo = angulo * 180 / math.pi
    #Retornamos el producto punto
    return angulo


def productoCruz(vector1, vector2):
    #Definimos la variable que guardará el resultado del producto cruz
    productoCruz = []
    
    productoCruz.append(vector1[1] * vector2[2] - vector1[2] * vector2[1])
    productoCruz.append(vector1[2] * vector2[0] - vector1[0] * vector2[2])
    productoCruz.append(vector1[0] * vector2[1] - vector1[1] * vector2[0])
    
    #Retornamos el producto cruz
    return productoCruz

def encontrarVectorParalelo(vector):
    #Definimos la variable que guardará el resultado del vector paralelo
    vectorParalelo = []
    
    #Definimos la variable que guardará el resultado del vector paralelo
    vectorParalelo.append(vector[0] * -1)
    vectorParalelo.append(vector[1] * -1)
    vectorParalelo.append(vector[2] * -1)

    return vectorParalelo

def multiplicarVector3DPorEscalar(vector, escalar):
    if len(vector) != 3:
        raise ValueError("El vector debe tener exactamente 3 componentes (x, y, z)")
    
    #Definimos la variable que guardará el resultado del vector paralelo
    vector = []
    for i in range(len(vector)):
        vector.append(vector[i] * escalar)
    return vector
    
    #Retornamos el vector paralelo
    # return vectorParalelo
    
def multMatrices(matriz1, matriz2):
    #Definimos la variable que guardará el resultado de la multiplicacion de matrices
    matrizResultado = []
    

    for i in range(len(matriz1)):
        matrizResultado.append([])
        for j in range(len(matriz2[0])):
            matrizResultado[i].append(0)
            for k in range(len(matriz2)):
                matrizResultado[i][j] += matriz1[i][k] * matriz2[k][j]


    return matrizResultado 


vector1 = [1.077, 4.501, 7.523]
vector2 = [-6.530, -1.382, 2.369]
print("Vector 1: ", vector1)
print("Vector 2: ", vector2)
resultadoPP = producto_punto_3d(vector1, vector2)
print("\nProducto Punto: ",resultadoPP)
print("Angulo entre vectores: ",obtener_angulo_entre_vectores(vector1, vector2))



vector1normalizado = normalizar(vector1)
vector2normalizado = normalizar(vector2)
print("\nVector 1 normalizado: ", vector1normalizado)
print("Vector 2 normalizado: ", vector2normalizado)
resultadoPP2 = producto_punto_3d(vector1normalizado, vector2normalizado)
print("Producto Punto de vectores normalizados: ",resultadoPP2)
print("Angulo entre vectores normalizados: ",obtener_angulo_entre_vectores(vector1normalizado, vector2normalizado))
# Ejemplo de dos vectores ortogonales


print("\nProducto cruz: ", productoCruz(vector1, vector2))


# Matrices 
# Matriz1 = [[7,2,2],[6,1,1],[0,6,-2]]
# Matriz2 = [[6,7,6],[-2,3,2],[5,-1,-2]]
Matriz1 = [[0,7,3],[0,0,1]]
Matriz2 = [[4,7],[3,5],[7,1]]


productoDMatrices=multMatrices(Matriz1, Matriz2)
print(productoDMatrices)
# # Crear la figura y los ejes 3D
# n = [1,1,2]
# print("n 0: ", n[0])
# vector = [2,1,1]
# # vectorParalelo = encontrarVectorParalelo(vector)
# productoPunto2 = producto_punto_3d(vector, n)
# print(productoPunto2)
# vectorTimesEscalar = multiplicarVector3DPorEscalar(n, productoPunto2)
# print("Vector times escalar: ", vectorTimesEscalar)


# fig = plt.figure()
# ax = fig.add_subplot(111, projection='3d')

# # Graficar el vector en 3D
# ax.quiver(0, 0, 0, vector[0], vector[1], vector[2], color='red', label='Vector')
# ax.quiver(0, 0, 0, n[0], n[1], n[2], color='green', label='n')
# # ax.quiver(0, 0, 0, vectorParalelo[0], vectorParalelo[1], vectorParalelo[2], color='blue', label='Vector Paralelo')

# # Mostrar el gráfico
# plt.show()