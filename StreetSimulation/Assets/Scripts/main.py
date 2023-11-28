# David Santiago Vieyra García A01656030
# Para correr el código acceder al directorio dónde se encuentra el archivo y correr el siguiente comando:
# python main.py 8 1.0 0.5 (lados, radio, ancho)
# o simplemente correr el siguiente comando para desplegar un menú y poder ingresar los valores o usar los valores por defecto:
# python main.py 

# Función que solicita al usuario el número de lados del círculo
def solicitar_lados():
    while True:
        lados = input("Numero de lados del circulo (entero entre 3 y 360): ")
        if lados.isdigit() and 3 <= int(lados) <= 360:
            return int(lados)
        print("Por favor ingresa un entero entre 3 y 360.")

# Función que solicita al usuario el radio del círculo
def solicitar_radio():
    while True:
        radio = input("Radio del circulo (flotante positivo): ")
        try:
            radio = float(radio)
            if radio > 0:
                return radio
        except ValueError:
            pass
        print("Por favor ingresa un número flotante positivo.")

# Función que solicita al usuario el ancho del cilindro
def solicitar_ancho():
    while True:
        ancho = input("Ancho de la rueda (flotante positivo): ")
        try:
            ancho = float(ancho)
            if ancho > 0:
                return ancho
        except ValueError:
            pass
        print("Por favor ingresa un número flotante positivo.")
        
def calcular_normal(v1, v2, v3):
    """
    Calcula la normal de una cara a partir de sus vértices
    """
    # Calcula los vectores entre los vértices
    vector1 = [v2[0] - v1[0], v2[1] - v1[1], v2[2] - v1[2]]
    vector2 = [v3[0] - v1[0], v3[1] - v1[1], v3[2] - v1[2]]

    # Calcula el producto cruz entre los vectores
    normal_x = vector1[1] * vector2[2] - vector1[2] * vector2[1]
    normal_y = vector1[2] * vector2[0] - vector1[0] * vector2[2]
    normal_z = vector1[0] * vector2[1] - vector1[1] * vector2[0]

    # Normaliza el vector resultante
    magnitud = (normal_x**2 + normal_y**2 + normal_z**2)**0.5
    return (normal_x / magnitud, normal_y / magnitud, normal_z / magnitud)


def generar_archivo_obj(num_triangulos, radio, ancho):
    import math
    with open("ruedaFinal.obj", "w") as archivo_obj:
        archivo_obj.write(f"# OBJ file by David Vieyra\n")
        archivo_obj.write(f"# Vertices: {2*num_triangulos + 2}\n")
        # Escribir vértices del círculo inferior
        for i in range(num_triangulos):
            angulo1 = 2 * math.pi * i / num_triangulos # Calcula el ángulo entre cada vértice del círculo en radianes para formar los triángulos del círculo          
            y1 = radio * math.cos(angulo1)  # y ahora es la coordenada del radio
            z1 = radio * math.sin(angulo1)  # z ahora es la coordenada del radio
            archivo_obj.write(f"v 0.0000 {y1:.4f} {z1:.4f}\n") # Escribir vértice, x1, y1 y z1 con 4 decimales

        # Escribir vértices del círculo superior
        for i in range(num_triangulos):
            angulo1 = 2 * math.pi * i / num_triangulos
            x1 = ancho  # x ahora es la coordenada de la distancia en z
            y1 = radio * math.cos(angulo1)  # y ahora es la coordenada del radio
            z1 = radio * math.sin(angulo1)  # z ahora es la coordenada del radio
            archivo_obj.write(f"v {x1:.4f} {y1:.4f} {z1:.4f}\n")

        # Escribir vértices del centro de las bases
        archivo_obj.write(f"v 0.0000 0.0000 0.0000\n")
        archivo_obj.write(f"v {ancho:.4f} 0.0000 0.0000\n")
        
        archivo_obj.write("vn 0.0000 0.0000 -1.0000\n")  # Normal de la base inferior
        archivo_obj.write("vn 0.0000 0.0000 1.0000\n")   # Normal de la base superior
        # Escribir normales de las caras
        archivo_obj.write("# Normales de las caras\n")
        for i in range(num_triangulos):
            siguiente = (i + 1) % num_triangulos
            normal = calcular_normal(
                (0.0, radio * math.cos(2 * math.pi * i / num_triangulos), radio * math.sin(2 * math.pi * i / num_triangulos)),
                (ancho, radio * math.cos(2 * math.pi * siguiente / num_triangulos), radio * math.sin(2 * math.pi * siguiente / num_triangulos)),
                (ancho, radio * math.cos(2 * math.pi * i / num_triangulos), radio * math.sin(2 * math.pi * i / num_triangulos))
            )
            archivo_obj.write(f"vn {normal[0]:.4f} {normal[1]:.4f} {normal[2]:.4f}\n")

        # # Calcular y escribir normales
        # archivo_obj.write("vn 0.0000 0.0000 -1.0000\n")  # Normal de la base inferior
        # archivo_obj.write("vn 0.0000 0.0000 1.0000\n")   # Normal de la base superior

        # # Normales de la superficie lateral
        # archivo_obj.write(f"# Normals: {num_triangulos+2}\n")
        # for i in range(num_triangulos):
        #     angulo1 = 2 * math.pi * i / num_triangulos
        #     normal_x = math.cos(angulo1)
        #     normal_y = math.sin(angulo1)
        #     normal_z = 0.0
        #     archivo_obj.write(f"vn {normal_x:.4f} {normal_y:.4f} {normal_z:.4f}\n")

        archivo_obj.write(f"# Faces: {num_triangulos*3}\n")
        # Faces of the inferior base
        archivo_obj.write("# Faces of the inferior base\n")
        for i in range(num_triangulos):
            siguiente = (i + 1) % num_triangulos
            archivo_obj.write(f"f {siguiente+1}//1 {i+1}//1 {2*num_triangulos+1}//1\n")

        # Faces of the superior base
        archivo_obj.write("# Faces of the superior base\n")
        for i in range(num_triangulos):
            siguiente = (i + 1) % num_triangulos
            archivo_obj.write(f"f {num_triangulos+i+1}//2 {num_triangulos+siguiente+1}//2 {2*num_triangulos+2}//2\n")

        # Lateral faces
        archivo_obj.write("# Lateral faces\n")
        for i in range(num_triangulos):
            siguiente = (i + 1) % num_triangulos
            archivo_obj.write(f"f {i+1}//3 {num_triangulos+i+2}//3 {siguiente+1}//3\n")
            archivo_obj.write(f"f {siguiente+1}//3 {num_triangulos+i+2}//3 {siguiente+2}//3\n")





    print(f"Archivo cilindro.obj creado exitosamente con {num_triangulos} triángulos, radio de {radio} unidades y ancho de {ancho} unidades.")

if __name__ == "__main__":
    import sys

    if len(sys.argv) >= 4:
        lados = int(sys.argv[1])
        radio = float(sys.argv[2])
        ancho = float(sys.argv[3])
    else:
        usar_default = input("¿Deseas usar los valores por defecto? (s/n): ").lower() == 's'    
        if usar_default:
            lados = 8
            radio = 1.0
            ancho = 0.5
        else:
            lados = solicitar_lados()
            radio = solicitar_radio()
            ancho = solicitar_ancho()

    print(f"\nLados: {lados}\nRadio: {radio}\nAncho: {ancho}")
    generar_archivo_obj(lados, radio, ancho)
    