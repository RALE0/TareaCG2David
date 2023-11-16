import pygame
from pygame.locals import *
from OpenGL.GL import *
from OpenGL.GLUT import *
from OpenGL.GLU import *


def load_obj(filename):
    vertices = []
    faces = []
    with open(filename, 'r') as file:
        for line in file:
            if line.startswith('v '):
                vertex = list(map(float, line.strip().split()[1:]))
                vertices.append(vertex)
            elif line.startswith('f '):
                face = list(map(int, line.strip().split()[1:]))
                faces.append(face)
    return vertices, faces

def display_obj(vertices, faces):
    glBegin(GL_TRIANGLES)
    for face in faces:
        for vertex in face:
            glVertex3fv(vertices[vertex - 1])  # los índices en .obj empiezan en 1
    glEnd()
def main():
    pygame.init()
    display = (800, 600)
    pygame.display.set_mode(display, DOUBLEBUF | OPENGL)
    gluPerspective(45, (display[0] / display[1]), 0.1, 50.0)
    glTranslatef(0.0, 0.0, -5)

    #glPolygonMode(GL_FRONT_AND_BACK, GL_LINE)  # Añadir esta línea

    vertices, faces = load_obj('circulo_triangular.obj')

    rotation_x, rotation_y, rotation_z = 0, 0, 0
    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_w:
                    rotation_x += 5
                elif event.key == pygame.K_s:
                    rotation_x -= 5
                elif event.key == pygame.K_a:
                    rotation_y += 5
                elif event.key == pygame.K_d:
                    rotation_y -= 5
                elif event.key == pygame.K_z:
                    rotation_z += 5
                elif event.key == pygame.K_x:
                    rotation_z -= 5
            elif event.type == pygame.MOUSEBUTTONDOWN:
                if event.button == 4:
                    glTranslatef(0, 0, 1.0)
                elif event.button == 5:
                    glTranslatef(0, 0, -1.0)
            elif event.type == pygame.MOUSEMOTION:
                if event.buttons[0] == 1:
                    glRotatef(event.rel[0], 0, 1, 0)
                    glRotatef(event.rel[1], 1, 0, 0)
            elif event.type == pygame.VIDEORESIZE:
                glViewport(0, 0, event.w, event.h)
                glMatrixMode(GL_PROJECTION)
                glLoadIdentity()
                gluPerspective(45, (event.w / event.h), 0.1, 50.0)
                glMatrixMode(GL_MODELVIEW)
                glLoadIdentity()
                glTranslatef(0.0, 0.0, -5)
            

        glRotatef(rotation_x, 1, 0, 0)
        glRotatef(rotation_y, 0, 1, 0)
        glRotatef(rotation_z, 0, 0, 1)
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT)
        display_obj(vertices, faces)
        pygame.display.flip()
        pygame.time.wait(10)

if __name__ == "__main__":
    main()
