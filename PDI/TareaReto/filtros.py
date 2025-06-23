from skimage.io import imread, imsave
import tkinter as tk
from tkinter import filedialog, messagebox, simpledialog, Menu
from PIL import Image, ImageTk
import numpy as np
#import cv2 # usar con opencv-python

# Este programa requiere tener instalados los siguientes paquetes:
# - tkinter
# - scikit-image
# - numpy
# - pillow
# Para instalar estos paquetes, puedes usar el siguiente comando:
# pip install tk scikit-image numpy pillow
# Nota:
# - Si usas un sistema basado en Linux, como Ubuntu o Fedora, y encuentras problemas con tkinter,
#   es posible que debas instalarlo manualmente. En Fedora, puedes instalarlo con:
#     sudo dnf install python3-tkinter
#   - Para instalar `ImageTk` correctamente en Fedora, tambien necesitas:
#     sudo dnf install python3-pillow-tk
# - Si `ImageTk` no se encuentra, asegurate de tener Pillow instalado correctamente, 
#   en caso de que no funcione.

# Si se usan imagenes muy grandes (como ave1.jpeg), puede que no se muestren correctamente en la interfaz grafica, se recomienda usar imagenes pequenias (como patito.jpg)
# Ademas, los filtros pueden ser tardados en aplicarse en imagenes grandes o medianas

# Funcion para solo dejar el valor R de cada pixel en la imagen
def red(imagen):
    img = imread(imagen)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            img[i, j, 1] = 0
            img[i, j, 2] = 0
    return img

# Funcion para solo dejar el valor G de cada pixel en la imagen
def green(imagen):
    img = imread(imagen)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            img[i, j, 0] = 0
            img[i, j, 2] = 0
    return img

# Funcion para solo dejar el valor B de cada pixel en la imagen
def blue(imagen):
    img = imread(imagen)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            img[i, j, 0] = 0
            img[i, j, 1] = 0
    return img

# Funcion para pasar una imagen a escala de grises
def escala_grises(imagen):
    img = imread(imagen)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            r = int(img[i, j, 0])
            g = int(img[i, j, 1])
            b = int(img[i, j, 2])
            tono = int(0.2126*r + 0.7152*g + 0.0722*b)
            img[i, j, 0] = tono
            img[i, j, 1] = tono
            img[i, j, 2] = tono
    return img

# Funcion para pasar una imagen a escala de grises con un metodo mas simple
def escala_grises_simple(imagen):
    img = imread(imagen)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            r = int(img[i, j, 0])
            g = int(img[i, j, 1])
            b = int(img[i, j, 2])
            tono = (r + g + b) // 3
            img[i, j, 0] = tono
            img[i, j, 1] = tono
            img[i, j, 2] = tono
    return img

# Funcion para aplicar alto contraste a una imagen (primero se pasa a escala de grises y luego se revisa si el tono es mayor o menor a 128)
def alto_contraste(imagen):
    img = escala_grises(imagen)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            if img[i, j, 0] > 128:
                img[i, j, 0] = 255
                img[i, j, 1] = 255
                img[i, j, 2] = 255
            else:
                img[i, j, 0] = 0
                img[i, j, 1] = 0
                img[i, j, 2] = 0
    return img

# Funcion para aplicar el contrario de alto contraste a una imagen
def inverso(imagen):
    img = escala_grises(imagen)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            if img[i, j, 0] <= 128:
                img[i, j, 0] = 255
                img[i, j, 1] = 255
                img[i, j, 2] = 255
            else:
                img[i, j, 0] = 0
                img[i, j, 1] = 0
                img[i, j, 2] = 0
    return img

# Funcion para aplicar el filtro brillo (-255 <= variable <= 255)
def brillo(imagen, variable):
    img = imread(imagen).astype(np.int16)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            for k in range(3):
                valor = img[i, j, k] + variable
                if valor < 0:
                    img[i, j, k] = 0
                elif valor > 255:
                    img[i, j, k] = 255
                else:
                    img[i, j, k] = valor
    return img.astype(np.uint8)

# Funcion para aplicar el filtro componentes RGB (0 <= r, g, b <= 255)
def componentes_rgb(imagen, r, g, b):
    img = imread(imagen).astype(np.uint8)
    for i in range(img.shape[0]):
        for j in range(img.shape[1]):
            img[i, j, 0] = img[i, j, 0] & r
            img[i, j, 1] = img[i, j, 1] & g
            img[i, j, 2] = img[i, j, 2] & b
    return img

# Funcion para aplicar el filtro mosaico, divide la imagen en bloques y saca el promedio de cada bloque
def mosaico(imagen, x, y):
    img = imread(imagen).astype(np.uint8)
    for i in range(0, img.shape[0], x):
        for j in range(0, img.shape[1], y):
            r = np.uint64(0)
            g = np.uint64(0)
            b = np.uint64(0)
            pixeles = 0
            for k in range(i, min(i + x, img.shape[0])):
                for l in range(j, min(j + y, img.shape[1])):
                    r += img[k, l, 0]
                    g += img[k, l, 1]
                    b += img[k, l, 2]
                    pixeles += 1
            r //= pixeles
            g //= pixeles
            b //= pixeles
            for k in range(i, min(i + x, img.shape[0])):
                for l in range(j, min(j + y, img.shape[1])):
                    img[k, l, 0] = r
                    img[k, l, 1] = g
                    img[k, l, 2] = b
    return img


# Funcion para aplicar un filtro donde cada region de pixeles se convierte a un caracter @ en un archivo HTML y respeta el color de la region
def imagen_a_html(imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y):
    img = imread(imagen).astype(np.uint8)
    alto, ancho, canales = img.shape
    with open(salida, "w") as f:
        f.write(f"<html><body style='background-color: white;'><pre style='font-family: monospace; font-size: {tam_letra}px; line-height: {esp_entre_linea}px;'>\n")
        for i in range(0, alto, tam_y):
            for j in range(0, ancho, tam_x):
                r = np.uint64(0)
                g = np.uint64(0)
                b = np.uint64(0)
                pixeles = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j, min(j + tam_x, ancho)):
                        r += img[k, l, 0]
                        g += img[k, l, 1]
                        b += img[k, l, 2]
                        pixeles += 1
                r //= pixeles
                g //= pixeles
                b //= pixeles
                f.write(f"<span style='color: rgb({r},{g},{b})'>{"@"}</span>")
            f.write("<br>\n")
        f.write("</pre></body></html>")

# Funcion para aplicar un filtro donde cada region de pixeles se convierte a un caracter @ en un archivo HTML y usa escala de grises
def imagen_a_html_gris(imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y):
    img = escala_grises(imagen)
    img = img.astype(np.uint8)
    alto, ancho, canales = img.shape
    with open(salida, "w") as f:
        f.write(f"<html><body style='background-color: white;'><pre style='font-family: monospace; font-size: {tam_letra}px; line-height: {esp_entre_linea}px;'>\n")
        for i in range(0, alto, tam_y):
            for j in range(0, ancho, tam_x):
                gris = np.uint64(0)
                pixeles = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j, min(j + tam_x, ancho)):
                        gris += img[k, l, 0]
                        pixeles += 1
                gris //= pixeles
                f.write(f"<span style='color: rgb({gris},{gris},{gris})'>{"@"}</span>")
            f.write("<br>\n")
        f.write("</pre></body></html>")

# Funcion para aplicar un filtro donde cada region de pixeles se convierte en un caracter especifico, dependiendo de su tono en escala de grises, en un archivo HTML
def imagen_a_html_letras(imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y):
    img = escala_grises(imagen)
    img = img.astype(np.uint8)
    alto, ancho, canales = img.shape
    with open(salida, "w") as f:
        f.write(f"<html><body style='background-color: white;'><pre style='font-family: monospace; font-size: {tam_letra}px; line-height: {esp_entre_linea}px;'>\n")
        for i in range(0, alto, tam_y):
            for j in range(0, ancho, tam_x):
                gris = np.uint64(0)
                pixeles = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j, min(j + tam_x, ancho)):
                        gris += img[k, l, 0]
                        pixeles += 1
                gris //= pixeles
                simbolo = gris_a_letra(gris)
                f.write(f"<span>{simbolo}</span>")
            f.write("<br>\n")
        f.write("</pre></body></html>")

# Funcion para obtener la letra dependiendo del tono de gris
def gris_a_letra(tono):
    if tono < 16:
        return "M"
    elif tono < 32:
        return "N"
    elif tono < 48:
        return "H"
    elif tono < 64:
        return "#"
    elif tono < 80:
        return "Q"
    elif tono < 96:
        return "U"
    elif tono < 112:
        return "A"
    elif tono < 128:
        return "D"
    elif tono < 144:
        return "0"
    elif tono < 160:
        return "Y"
    elif tono < 176:
        return "2"
    elif tono < 192:
        return "$"
    elif tono < 208:
        return "%"
    elif tono < 224:
        return "+"
    elif tono < 240:
        return "."
    else:
        return " "

# Funcion para aplicar un filtro donde cada region de pixeles se convierte en un un caracter de una cadena en un archivo HTML y respeta el color de la region
def imagen_a_html_cadena(imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y, cadena):
    img = imread(imagen).astype(np.uint8)
    alto, ancho, canales = img.shape
    with open(salida, "w") as f:
        f.write(f"<html><body style='background-color: white;'><pre style='font-family: monospace; font-size: {tam_letra}px; line-height: {esp_entre_linea}px;'>\n")
        posicion = 0
        for i in range(0, alto, tam_y):
            for j in range(0, ancho, tam_x):
                r = np.uint64(0)
                g = np.uint64(0)
                b = np.uint64(0)
                pixeles = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j, min(j + tam_x, ancho)):
                        r += img[k, l, 0]
                        g += img[k, l, 1]
                        b += img[k, l, 2]
                        pixeles += 1
                r //= pixeles
                g //= pixeles
                b //= pixeles
                simbolo = cadena[posicion % len(cadena)]
                posicion += 1
                f.write(f"<span style='color: rgb({r},{g},{b})'>{simbolo}</span>")
            f.write("<br>\n")
        f.write("</pre></body></html>")

# Funcion para aplicar un filtro donde cada dos regiones de pixeles se convierten en una pieza de domino horizontal en un archivo HTML
def imagen_a_domino(imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y):
    img = escala_grises(imagen)
    img = img.astype(np.uint8)
    alto, ancho, canales = img.shape
    with open(salida, "w") as f:
        f.write(f"<html><body style='background-color: white;'><pre style='font-family: monospace; font-size: {tam_letra}px; line-height: {esp_entre_linea}px;'>\n")
        for i in range(0, alto, tam_y):
            for j in range(0, ancho, tam_x * 2):
                gris_parte1 = np.uint64(0)
                pixeles_parte1 = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j, min(j + tam_x, ancho)):
                        gris_parte1 += img[k, l, 0]
                        pixeles_parte1 += 1
                if pixeles_parte1 == 0:
                    continue
                else:
                    gris_parte1 //= pixeles_parte1
                gris_parte2 = np.uint64(0)
                pixeles_parte2 = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j + tam_x, min(j + tam_x * 2, ancho)):
                        gris_parte2 += img[k, l, 0]
                        pixeles_parte2 += 1
                if pixeles_parte2 == 0:
                    gris_parte2 = gris_parte1
                else:
                    gris_parte2 //= pixeles_parte2
                simbolo = piezas_domino[(gris_a_numero_domino(gris_parte1), gris_a_numero_domino(gris_parte2))]
                f.write(f"<span>{simbolo}</span>")
            f.write("<br>\n")
        f.write("</pre></body></html>")

# Funcion para obtener el numero de domino dependiendo del tono de gris
def gris_a_numero_domino(tono):
    if tono < 37:
        return 6
    elif tono < 74:
        return 5
    elif tono < 111:
        return 4
    elif tono < 148:
        return 3
    elif tono < 185:
        return 2
    elif tono < 222:
        return 1
    else:
        return 0

# Diccionario para obtener el codigo HTML de una pieza de domino vertical dependiendo de sus numeros
piezas_domino = {
    (0, 0): "&#127025;",
    (0, 1): "&#127026;",
    (0, 2): "&#127027;",
    (0, 3): "&#127028;",
    (0, 4): "&#127029;",
    (0, 5): "&#127030;",
    (0, 6): "&#127031;",
    (1, 0): "&#127032;",
    (1, 1): "&#127033;",
    (1, 2): "&#127034;",
    (1, 3): "&#127035;",
    (1, 4): "&#127036;",
    (1, 5): "&#127037;",
    (1, 6): "&#127038;",
    (2, 0): "&#127039;",
    (2, 1): "&#127040;",
    (2, 2): "&#127041;",
    (2, 3): "&#127042;",
    (2, 4): "&#127043;",
    (2, 5): "&#127044;",
    (2, 6): "&#127045;",
    (3, 0): "&#127046;",
    (3, 1): "&#127047;",
    (3, 2): "&#127048;",
    (3, 3): "&#127049;",
    (3, 4): "&#127050;",
    (3, 5): "&#127051;",
    (3, 6): "&#127052;",
    (4, 0): "&#127053;",
    (4, 1): "&#127054;",
    (4, 2): "&#127055;",
    (4, 3): "&#127056;",
    (4, 4): "&#127057;",
    (4, 5): "&#127058;",
    (4, 6): "&#127059;",
    (5, 0): "&#127060;",
    (5, 1): "&#127061;",
    (5, 2): "&#127062;",
    (5, 3): "&#127063;",
    (5, 4): "&#127064;",
    (5, 5): "&#127065;",
    (5, 6): "&#127066;",
    (6, 0): "&#127067;",
    (6, 1): "&#127068;",
    (6, 2): "&#127069;",
    (6, 3): "&#127070;",
    (6, 4): "&#127071;",
    (6, 5): "&#127072;",
    (6, 6): "&#127073;"
}

# Funcion para aplicar un filtro donde cada dos regiones de pixeles se convierten en una pieza de domino horizontal en un archivo HTML, usando una fuente
def imagen_a_domino_fuente(imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y):
    img = escala_grises(imagen)
    img = img.astype(np.uint8)
    alto, ancho, canales = img.shape
    with open(salida, "w") as f:
        f.write(f"""<html><head><style>@font-face {{font-family: 'Lasvwd';src: url('/imagenes/Lasvbld_.ttf') format('truetype');}}body {{background-color: white;}}
                pre {{font-family: 'Lasvwd', monospace;font-size: {tam_letra}px;line-height: {esp_entre_linea}px;}}</style></head><body><pre>""")
        for i in range(0, alto, tam_y):
            for j in range(0, ancho, tam_x * 2):
                gris_parte1 = np.uint64(0)
                pixeles_parte1 = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j, min(j + tam_x, ancho)):
                        gris_parte1 += img[k, l, 0]
                        pixeles_parte1 += 1
                if pixeles_parte1 == 0:
                    continue
                else:
                    gris_parte1 //= pixeles_parte1
                gris_parte2 = np.uint64(0)
                pixeles_parte2 = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j + tam_x, min(j + tam_x * 2, ancho)):
                        gris_parte2 += img[k, l, 0]
                        pixeles_parte2 += 1
                if pixeles_parte2 == 0:
                    gris_parte2 = gris_parte1
                else:
                    gris_parte2 //= pixeles_parte2
                simbolo = piezas_domino_fuente[(gris_a_numero_domino_fuente(gris_parte1), gris_a_numero_domino_fuente(gris_parte2))]
                f.write(f"<span style='font-family: Lasvwd;'>{simbolo}</span>")
            f.write("<br>\n")
        f.write("</pre></body></html>")

# Funcion para obtener el numero de domino dependiendo del tono de gris
def gris_a_numero_domino_fuente(tono):
    if tono < 25:
        return 0
    elif tono < 51:
        return 1
    elif tono < 77:
        return 2
    elif tono < 103:
        return 3
    elif tono < 129:
        return 4
    elif tono < 155:
        return 5
    elif tono < 181:
        return 6
    elif tono < 207:
        return 7
    elif tono < 233:
        return 8
    else:
        return 9

# Diccionario para obtener el codigo HTML de una pieza de domino vertical dependiendo de sus numeros
piezas_domino_fuente = {
    (0, 0): "00",
    (0, 1): "01",
    (0, 2): "02",
    (0, 3): "03",
    (0, 4): "04",
    (0, 5): "05",
    (0, 6): "06",
    (0, 7): "07",
    (0, 8): "08",
    (0, 9): "09",
    (1, 0): "10",
    (1, 1): "11",
    (1, 2): "12",
    (1, 3): "13",
    (1, 4): "14",
    (1, 5): "15",
    (1, 6): "16",
    (1, 7): "17",
    (1, 8): "18",
    (1, 9): "19",
    (2, 0): "20",
    (2, 1): "21",
    (2, 2): "22",
    (2, 3): "23",
    (2, 4): "24",
    (2, 5): "25",
    (2, 6): "26",
    (2, 7): "27",
    (2, 8): "28",
    (2, 9): "29",
    (3, 0): "30",
    (3, 1): "31",
    (3, 2): "32",
    (3, 3): "33",
    (3, 4): "34",
    (3, 5): "35",
    (3, 6): "36",
    (3, 7): "37",
    (3, 8): "38",
    (3, 9): "39",
    (4, 0): "40",
    (4, 1): "41",
    (4, 2): "42",
    (4, 3): "43",
    (4, 4): "44",
    (4, 5): "45",
    (4, 6): "46",
    (4, 7): "47",
    (4, 8): "48",
    (4, 9): "49",
    (5, 0): "50",
    (5, 1): "51",
    (5, 2): "52",
    (5, 3): "53",
    (5, 4): "54",
    (5, 5): "55",
    (5, 6): "56",
    (5, 7): "57",
    (5, 8): "58",
    (5, 9): "59",
    (6, 0): "60",
    (6, 1): "61",
    (6, 2): "62",
    (6, 3): "63",
    (6, 4): "64",
    (6, 5): "65",
    (6, 6): "66",
    (6, 7): "67",
    (6, 8): "68",
    (6, 9): "69",
    (7, 0): "70",
    (7, 1): "71",
    (7, 2): "72",
    (7, 3): "73",
    (7, 4): "74",
    (7, 5): "75",
    (7, 6): "76",
    (7, 7): "77",
    (7, 8): "78",
    (7, 9): "79",
    (8, 0): "80",
    (8, 1): "81",
    (8, 2): "82",
    (8, 3): "83",
    (8, 4): "84",
    (8, 5): "85",
    (8, 6): "86",
    (8, 7): "87",
    (8, 8): "88",
    (8, 9): "89",
    (9, 0): "90",
    (9, 1): "91",
    (9, 2): "92",
    (9, 3): "93",
    (9, 4): "94",
    (9, 5): "95",
    (9, 6): "96",
    (9, 7): "97",
    (9, 8): "98",
    (9, 9): "99"
}

# Funcion para aplicar un filtro donde cada region de pixeles se convierten en una carta en un archivo HTML
def imagen_a_carta(imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y):
    img = escala_grises(imagen)
    img = img.astype(np.uint8)
    alto, ancho, canales = img.shape
    with open(salida, "w") as f:
        f.write(f"<html><body style='background-color: white;'><pre style='font-family: monospace; font-size: {tam_letra}px; line-height: {esp_entre_linea}px;'>\n")
        for i in range(0, alto, tam_y):
            for j in range(0, ancho, tam_x):
                gris = np.uint64(0)
                pixeles = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j, min(j + tam_x, ancho)):
                        gris += img[k, l, 0]
                        pixeles += 1
                gris //= pixeles
                simbolo = cartas[gris_a_numero_carta(gris)]
                f.write(f"<span>{simbolo}</span>")
            f.write("<br>\n")
        f.write("</pre></body></html>")

# Funcion para obtener el numero de carta dependiendo del tono de gris
def gris_a_numero_carta(tono):
    if tono < 23:
        return 11
    elif tono < 46:
        return 10
    elif tono < 69:
        return 9
    elif tono < 92:
        return 8
    elif tono < 115:
        return 7
    elif tono < 138:
        return 6
    elif tono < 161:
        return 5
    elif tono < 184:
        return 4
    elif tono < 207:
        return 3
    elif tono < 230:
        return 2
    else:
        return 1

# Diccionario para obtener el codigo HTML de una carta dependiendo de su numero
cartas = {
    (1): "&#127169;",
    (2): "&#127170;",
    (3): "&#127171;",
    (4): "&#127172;",
    (5): "&#127173;",
    (6): "&#127174;",
    (7): "&#127175;",
    (8): "&#127176;",
    (9): "&#127177;",
    (10): "&#127178;",
    (11): "&#127180;"
}

# Funcion para aplicar un filtro donde cada region de pixeles se convierten en una carta en un archivo HTML, usando una fuente
def imagen_a_carta_fuente(imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y):
    img = escala_grises(imagen)
    img = img.astype(np.uint8)
    alto, ancho, canales = img.shape
    with open(salida, "w") as f:
        f.write(f"""<html><head><style>@font-face {{font-family: 'PlayCards';src: url('/imagenes/Playcrds.ttf') format('truetype');}}body {{background-color: white;}}
                pre {{font-family: 'PlayCards', monospace;font-size: {tam_letra}px;line-height: {esp_entre_linea}px;}}</style></head><body><pre>""")
        for i in range(0, alto, tam_y):
            for j in range(0, ancho, tam_x):
                gris = np.uint64(0)
                pixeles = 0
                for k in range(i, min(i + tam_y, alto)):
                    for l in range(j, min(j + tam_x, ancho)):
                        gris += img[k, l, 0]
                        pixeles += 1
                gris //= pixeles
                simbolo = cartas[gris_a_numero_carta(gris)]
                f.write(f"<span style='font-family: PlayCards;'>{simbolo}</span>")
            f.write("<br>\n")
        f.write("</pre></body></html>")

# Funcion para obtener el numero de carta dependiendo del tono de gris
def gris_a_numero_carta_fuente(tono):
    if tono < 25:
        return 0
    elif tono < 51:
        return 1
    elif tono < 77:
        return 2
    elif tono < 103:
        return 3
    elif tono < 129:
        return 4
    elif tono < 155:
        return 5
    elif tono < 181:
        return 6
    elif tono < 207:
        return 7
    elif tono < 233:
        return 8
    else:
        return 9

# Diccionario para obtener el codigo HTML de una carta dependiendo de su numero
cartas_fuente = {
    (0): "m",
    (1): "i",
    (2): "h",
    (3): "g",
    (4): "f",
    (5): "e",
    (6): "d",
    (7): "c",
    (8): "b",
    (9): "a"
}

# Filtro para quitar la marca de agua roja de una imagen en escala de grises
def quitar_marca_agua(imagen):
    img = imread(imagen).astype(np.uint32)
    img_gris = escala_grises(imagen)
    alto, ancho, canales = img.shape
    mascara = np.zeros((alto, ancho), dtype=np.uint8)
    tole = 5
    for i in range(alto):
        for j in range(ancho):
            if img[i, j, 0] > img[i, j, 1] + tole and img[i, j, 0] > img[i, j, 2] + tole:
                mascara[i, j] = 255
    """
    color, opacidad = estimar_marca(img_gris, mascara, img)
    color[0] = color[0].astype(np.uint32)
    color[1] = color[1].astype(np.uint32)
    color[2] = color[2].astype(np.uint32)
    """
    for i in range(alto):
        for j in range(ancho):
            if mascara[i, j] == 255:
                r_o = img[i, j, 0]
                g_o = img[i, j, 1]
                b_o = img[i, j, 2]
                gris = (r_o + g_o + b_o) // 3
                gris += estimar_brillo(img_gris, i, j, mascara)
                if gris < 0:
                    gris = 0
                elif gris > 255:
                    gris = 255
                img[i, j, 0] = gris
                img[i, j, 1] = gris
                img[i, j, 2] = gris
                """
                r_a = img[i, j, 0]
                g_a = img[i, j, 1]
                b_a = img[i, j, 2]
                r_n = (r_a - color[0] * opacidad) / (1 - opacidad)
                g_n = (g_a - color[1] * opacidad) / (1 - opacidad)
                b_n = (b_a - color[2] * opacidad) / (1 - opacidad)
                if r_n < 0:
                    r_n = 0
                elif r_n > 255:
                    r_n = 255
                if g_n < 0:
                    g_n = 0
                elif g_n > 255:
                    g_n = 255
                if b_n < 0:
                    b_n = 0
                elif b_n > 255:
                    b_n = 255
                img[i, j, 0] = r_n
                img[i, j, 1] = g_n
                img[i, j, 2] = b_n
                """
    return img.astype(np.uint8)

# Funcion para estimar el brillo de un pixel usando sus vecinos que no esten en la mascara
def estimar_brillo(img, i, j, mascara):
    vecinos = []
    radio = 1
    radio_max = 10
    alto, ancho, canales = img.shape
    cantidad_vec = 3
    for radio in range(1, radio_max + 1):
        if len(vecinos) >= cantidad_vec:
            break
        for k in range(i - radio, i + radio + 1):
            for l in range(j - radio, j + radio + 1):
                if 0 <= k < alto and 0 <= l < ancho and (k != i or l != j) and mascara[k, l] == 0:
                    vecinos.append(img[k, l, 0])
    if len(vecinos) == 0:
        return 0
    promedio = np.int32(0)
    for v in vecinos:
        promedio += v
    promedio //= len(vecinos)
    if promedio < 42:
        return -75
    elif promedio < 84:
        return -50
    elif promedio < 126:
        return -25
    elif promedio < 168:
        return 0
    elif promedio < 210:
        return 25
    elif promedio < 252:
        return 50
    else:
        return 75

"""
# Funcion para estimar el color y opacidad de la marca de agua de color rojo en una imagen en escala de grises
def estimar_marca(imagen, mascara, imagen_color):
    #return np.array([180, 0, 30]), 0.3
    #return np.array([200, 0, 12]), 0.32
    #return np.array([190, 0, 15]), 0.31
    return None, None
"""

# Diccionario de filtros basicos
filtros_basicos = {
    "Rojo": red,
    "Verde": green,
    "Azul": blue,
    "Escala de grises": escala_grises,
    "Escala de grises simple": escala_grises_simple,
    "Alto contraste": alto_contraste,
    "Inverso": inverso,
    "Brillo": brillo,
    "Componentes RGB": componentes_rgb,
    "Mosaico": mosaico
}

# Diccionario de filtros de imagen a HTML
filtros_html = {
    "Simbolo en colores": imagen_a_html,
    "Simbolo en escala de grises": imagen_a_html_gris,
    "Letras en escala de grises": imagen_a_html_letras,
    "Texto en colores": imagen_a_html_cadena,
    "Dominos": imagen_a_domino,
    "Dominos con fuente": imagen_a_domino_fuente,
    "Cartas": imagen_a_carta,
    "Cartas con fuente": imagen_a_carta_fuente
}

# Diccionario de filtros de quitar marca de agua
filtros_marca_agua = {
    "Quitar marca de agua imagen especifica": quitar_marca_agua
}


# Funciones para la interfaz grafica

# Funcion para aplicar un filtro sin parametros extras
def aplicar_filtro(tipo):
    global ruta_imagen, panel_modificada, img_modificada
    if not ruta_imagen:
        messagebox.showerror("Error", "Primero selecciona una imagen")
        return
    img_array = filtros_basicos[tipo](ruta_imagen)
    img_modificada = Image.fromarray(img_array.astype(np.uint8))
    img_tk = ImageTk.PhotoImage(img_modificada)
    panel_modificada.config(image=img_tk)
    panel_modificada.image = img_tk
    messagebox.showinfo("Listo", "La imagen ha sido modificada correctamente.")

# Funcion para aplicar el filtro brillo
def aplicar_brillo():
    global ruta_imagen, panel_modificada, img_modificada
    if not ruta_imagen:
        messagebox.showerror("Error", "Primero selecciona una imagen")
        return
    valor_brillo = simpledialog.askinteger("Brillo", "Introduce un valor (-255 a 255):", minvalue=-255, maxvalue=255)
    if valor_brillo is None:
        return
    img_array = brillo(ruta_imagen, valor_brillo)
    img_modificada = Image.fromarray(img_array.astype(np.uint8))
    img_tk = ImageTk.PhotoImage(img_modificada)
    panel_modificada.config(image=img_tk)
    panel_modificada.image = img_tk
    messagebox.showinfo("Listo", "La imagen ha sido modificada correctamente.")

# Funcion para aplicar el filtro componentes RGB
def aplicar_componentes_rgb():
    global ruta_imagen, panel_modificada, img_modificada
    if not ruta_imagen:
        messagebox.showerror("Error", "Primero selecciona una imagen")
        return
    valores = simpledialog.askstring("Componentes RGB", "Introduce los valores de R, G, B (0 a 255) separados por comas (ejemplo: 255,0,0):")
    if valores is None:
        return
    try:
        r, g, b = map(int, valores.split(","))
        if not (0 <= r <= 255 and 0 <= g <= 255 and 0 <= b <= 255):
            raise ValueError
    except ValueError:
        messagebox.showerror("Error", "Ingresa tres numeros entre 0 y 255 separados por comas.")
        return
    img_array = componentes_rgb(ruta_imagen, r, g, b)
    img_modificada = Image.fromarray(img_array.astype(np.uint8))
    img_tk = ImageTk.PhotoImage(img_modificada)
    panel_modificada.config(image=img_tk)
    panel_modificada.image = img_tk
    messagebox.showinfo("Listo", "La imagen ha sido modificada correctamente.")

# Funcion para aplicar el filtro mosaico
def aplicar_mosaico():
    global ruta_imagen, panel_modificada, img_modificada
    if not ruta_imagen:
        messagebox.showerror("Error", "Primero selecciona una imagen")
        return
    valores = simpledialog.askstring("Mosaico", "Introduce el tamanio de los bloques (x, y) separados por comas (ejemplo: 10,10):")
    if valores is None:
        return
    try:
        x, y = map(int, valores.split(","))
        if not (x > 0 and y > 0):
            raise ValueError
    except ValueError:
        messagebox.showerror("Error", "Ingresa dos numeros enteros positivos separados por comas.")
        return
    alto, ancho, canales = imread(ruta_imagen).shape
    if x > ancho or y > alto:
        messagebox.showerror("Error", "El tamanio de los bloques no puede ser mayor al tamanio de la imagen.")
        return
    img_array = mosaico(ruta_imagen, x, y)
    img_modificada = Image.fromarray(img_array.astype(np.uint8))
    img_tk = ImageTk.PhotoImage(img_modificada)
    panel_modificada.config(image=img_tk)
    panel_modificada.image = img_tk
    messagebox.showinfo("Listo", "La imagen ha sido modificada correctamente.")

# Funcion para aplicar un filtro de imagen a HTML
def aplicar_filtro_html(tipo):
    global ruta_imagen
    if not ruta_imagen:
        messagebox.showerror("Error", "Primero selecciona una imagen")
        return
    valores = simpledialog.askstring("Imagen a HTML", "Introduce el tamano de letra, espacio entre lineas, tamanio de los bloques (x, y) separados por comas (ejemplo: 14,4,3,3):")
    if valores is None:
        return
    try:
        tam_letra, esp_entre_linea, tam_x, tam_y = map(int, valores.split(","))
        if not (tam_letra > 0 and esp_entre_linea > 0 and tam_x > 0 and tam_y > 0):
            raise ValueError
    except ValueError:
        messagebox.showerror("Error", "Ingresa cuatro numeros enteros positivos separados por comas.")
        return
    alto, ancho, canales = imread(ruta_imagen).shape
    if tam_x > ancho or tam_y > alto:
        messagebox.showerror("Error", "El tamanio de los bloques no puede ser mayor al tamanio de la imagen.")
        return
    salida = filedialog.asksaveasfilename(filetypes=[("Archivos HTML", "*.html")])
    if not salida:
        return
    filtros_html[tipo](ruta_imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y)
    messagebox.showinfo("Listo", "La imagen ha sido guardada como HTML correctamente.")

# Funcion para aplicar un filtro de imagen a HTML pidiento una cadena de caracteres
def aplicar_filtro_html_texto():
    global ruta_imagen
    if not ruta_imagen:
        messagebox.showerror("Error", "Primero selecciona una imagen")
        return
    valores = simpledialog.askstring("Imagen a HTML", "Introduce el tamano de letra, espacio entre lineas, tamanio de los bloques (x, y) separados por comas (ejemplo: 14,4,3,3):")
    if valores is None:
        return
    try:
        tam_letra, esp_entre_linea, tam_x, tam_y = map(int, valores.split(","))
        if not (tam_letra > 0 and esp_entre_linea > 0 and tam_x > 0 and tam_y > 0):
            raise ValueError
    except ValueError:
        messagebox.showerror("Error", "Ingresa cuatro numeros enteros positivos separados por comas.")
        return
    alto, ancho, canales = imread(ruta_imagen).shape
    if tam_x > ancho or tam_y > alto:
        messagebox.showerror("Error", "El tamanio de los bloques no puede ser mayor al tamanio de la imagen.")
        return
    texto = simpledialog.askstring("Imagen a HTML", "Introduce el texto a usar:")
    if texto is None:
        return
    salida = filedialog.asksaveasfilename(filetypes=[("Archivos HTML", "*.html")])
    if not salida:
        return
    imagen_a_html_cadena(ruta_imagen, salida, tam_letra, esp_entre_linea, tam_x, tam_y, texto)
    messagebox.showinfo("Listo", "La imagen ha sido guardada como HTML correctamente.")

# Funcion para aplicar un filtro de quitado de marca de agua
def aplicar_filtro_marca_agua(tipo):
    global ruta_imagen, panel_modificada, img_modificada
    if not ruta_imagen:
        messagebox.showerror("Error", "Primero selecciona una imagen")
        return
    img_array = filtros_marca_agua[tipo](ruta_imagen)
    img_modificada = Image.fromarray(img_array.astype(np.uint8))
    img_tk = ImageTk.PhotoImage(img_modificada)
    panel_modificada.config(image=img_tk)
    panel_modificada.image = img_tk
    messagebox.showinfo("Listo", "La imagen ha sido modificada correctamente.")

# Funcion para seleccionar una imagen
def seleccionar_imagen():
    global ruta_imagen, panel_original, panel_modificada
    ruta_imagen = filedialog.askopenfilename(filetypes=[("Archivos de imagen", "*.png"), ("Archivos JPG", "*.jpg"), ("Archivos JPEG", "*.jpeg")] )
    if not ruta_imagen:
        return
    img_original = Image.open(ruta_imagen).convert("RGB")
    img_tk = ImageTk.PhotoImage(img_original)
    panel_original.config(image=img_tk)
    panel_original.image = img_tk
    panel_modificada.config(image=img_tk)
    panel_modificada.image = img_tk

# Funcion para guardar la imagen modificada
def guardar_imagen():
    global img_modificada
    if img_modificada is None:
        messagebox.showerror("Error", "No hay imagen modificada para guardar")
        return
    ruta_salida = filedialog.asksaveasfilename(filetypes=[("Archivos de imagen", "*.png"), ("Archivos JPG", "*.jpg"), ("Archivos JPEG", "*.jpeg")])
    if ruta_salida:
        img_modificada.save(ruta_salida)
        messagebox.showinfo("Listo", "La imagen ha sido guardada correctamente.")

# Interfaz grafica

root = tk.Tk()
root.title("Filtro de Imagenes")
root.geometry("900x700")

# Menu
menubar = Menu(root)
root.config(menu=menubar)

# Menu Archivo
menu_archivo = Menu(menubar, tearoff=0)
menu_archivo.add_command(label="Abrir Imagen", command=seleccionar_imagen)
menu_archivo.add_command(label="Guardar Imagen", command=guardar_imagen)
menubar.add_cascade(label="Archivo", menu=menu_archivo)

# Menu Filtros Basicos
menu_filtros = Menu(menubar, tearoff=0)
for nombre, filtro in filtros_basicos.items():
    if filtro.__code__.co_argcount == 1:
        menu_filtros.add_command(label=nombre, command=lambda nombre=nombre: aplicar_filtro(nombre))
    elif nombre == "Brillo":
        menu_filtros.add_command(label=nombre, command=aplicar_brillo)
    elif nombre == "Componentes RGB":
        menu_filtros.add_command(label=nombre, command=aplicar_componentes_rgb)
    elif nombre == "Mosaico":
        menu_filtros.add_command(label=nombre, command=aplicar_mosaico)
menubar.add_cascade(label="Filtros Basicos", menu=menu_filtros)

# Menu Filtros HTML
menu_filtros_html = Menu(menubar, tearoff=0)
for nombre, filtro in filtros_html.items():
    if filtro.__code__.co_argcount == 6:
        menu_filtros_html.add_command(label=nombre, command=lambda nombre=nombre: aplicar_filtro_html(nombre))
    elif nombre == "Texto en colores":
        menu_filtros_html.add_command(label=nombre, command=aplicar_filtro_html_texto)
menubar.add_cascade(label="Filtros HTML", menu=menu_filtros_html)

# Menu Filtros Quitar marca de agua
menu_filtros_marca_agua = Menu(menubar, tearoff=0)
for nombre, filtro in filtros_marca_agua.items():
    menu_filtros_marca_agua.add_command(label=nombre, command=lambda nombre=nombre: aplicar_filtro_marca_agua(nombre))
menubar.add_cascade(label="Filtros Marca de Agua", menu=menu_filtros_marca_agua)

# Seccion de imagenes
frame_imagenes = tk.Frame(root)
frame_imagenes.pack(pady=10)

panel_original = tk.Label(frame_imagenes)
panel_original.pack(side="left", padx=10)

panel_modificada = tk.Label(frame_imagenes)
panel_modificada.pack(side="left", padx=10)

ruta_imagen = None
img_modificada = None

root.mainloop()