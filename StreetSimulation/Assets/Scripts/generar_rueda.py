import sys
import math

def generar_modelo_rueda(num_lados, radio, ancho):
    # Inicializar listas para almacenar vértices, normales y caras
    vertices = []
    normales = []

    # Calcular vértices
    for i in range(num_lados):
        angulo = i * (2*math.pi) / num_lados
        x = radio * math.cos(angulo)
        y = radio * math.sin(angulo)
        vertices.extend([(x, y, -ancho/2), (x, y, ancho/2)])
        normales.extend([(x, y, 0), (x, y, 0)])

    # Escribir el archivo OBJ
    with open("rueda4.obj", "w") as archivo:
        # Escribir vértices
        for vertice in vertices:
            archivo.write(f"v {vertice[0]:.4f} {vertice[1]:.4f} {vertice[2]:.4f}\n")

        # Escribir normales
        for normal in normales:
            norma = math.sqrt(normal[0]**2 + normal[1]**2 + normal[2]**2)
            normalizado = (normal[0]/norma, normal[1]/norma, normal[2]/norma)
            archivo.write(f"vn {normalizado[0]:.4f} {normalizado[1]:.4f} {normalizado[2]:.4f}\n")

        # Escribir caras
        for i in range(0, len(vertices), 4):
            archivo.write(f"f {i+1}//{i+1} {i+2}//{i+2} {i+4 if i+4 <= len(vertices) else 2}//{i+4 if i+4 <= len(vertices) else 2} {i+3}//{i+3}\n")

if __name__ == "__main__":
    num_lados = 18
    radio = 1.0
    ancho = 0.5

    generar_modelo_rueda(num_lados, radio, ancho)
    print("Modelo de rueda generado con éxito en el archivo 'rueda4.obj'.")
