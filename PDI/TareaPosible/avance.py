
# Funcion para aplicar un filtro de convolucion
def convolucion(imagen, matriz, factor, bias, tam):
    img = imread(imagen).astype(np.uint8)
    alto, ancho, canales = img.shape
    img_nueva = np.copy(img)
    for i in range(alto):
        for j in range(ancho):
            for k in range(3):
                suma = 0.0
                for x in range(tam):
                    for y in range(tam):
                        nx = min(max(i - tam // 2 + x, 0), alto - 1)
                        ny = min(max(j - tam // 2 + y, 0), ancho - 1)
                        suma += img[nx, ny, k] * matriz[x][y]
                img_nueva[i, j, k] = np.clip(int(suma * factor + bias), 0, 255)
    return img_nueva

# Funcion para aplicar el filtro blur
def blur(imagen):
    """
    matriz = [[0.0, 0.2, 0.0], [0.2, 0.2, 0.2], [0.0, 0.2, 0.0]]
    factor = 1.0
    bias = 0.0
    num = 3
    """
    matriz = [[0.0, 0.0, 1.0, 0.0, 0.0], 
              [0.0, 1.0, 1.0, 1.0, 0.0], 
              [1.0, 1.0, 1.0, 1.0, 1.0], 
              [0.0, 1.0, 1.0, 1.0, 0.0], 
              [0.0, 0.0, 1.0, 0.0, 0.0]]
    factor = 1.0 / 13.0
    bias = 0.0
    num = 5
    return convolucion(imagen, matriz, factor, bias, num)

# Funcion para aplicar el filtro Gaussian blur
def gauss_blur(imagen):
    """
    matriz = [[1.0, 2.0, 1.0], [2.0, 4.0, 2.0], [1.0, 2.0, 1.0]]
    factor = 1.0 / 16.0
    bias = 0.0
    num = 3
    """
    matriz = [[1.0, 4.0, 6.0, 4.0, 1.0], 
              [4.0, 16.0, 24.0, 16.0, 4.0], 
              [6.0, 24.0, 36.0, 24.0, 6.0], 
              [4.0, 16.0, 24.0, 16.0, 4.0], 
              [1.0, 4.0, 6.0, 4.0, 1.0]]
    factor = 1.0 / 256.0
    bias = 0.0
    num = 5
    return convolucion(imagen, matriz, factor, bias, num)

# Funcion para aplicar el filtro motion blur
def motion_blur(imagen):
    matriz = [[1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0], 
              [0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0], 
              [0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0], 
              [0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0], 
              [0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0], 
              [0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0], 
              [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0], 
              [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0], 
              [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0], 
              [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0]]
    factor = 1.0 / 9.0
    bias = 0.0
    num = 9
    return convolucion(imagen, matriz, factor, bias, num)

# Funcion para aplicar el filtro find edges
def find_edges(imagen):
    matriz = [[-1.0, -1.0, -1.0], 
              [-1.0, 8.0, -1.0], 
              [-1.0, -1.0, -1.0]]
    factor = 1.0
    bias = 0.0
    num = 3
    return convolucion(imagen, matriz, factor, bias, num)

# Funcion para aplicar el filtro sharpen
def sharpen(imagen):
    matriz = [[-1.0, -1.0, -1.0], 
              [-1.0, 9.0, -1.0], 
              [-1.0, -1.0, -1.0]]
    factor = 1.0
    bias = 0.0
    num = 3
    return convolucion(imagen, matriz, factor, bias, num)

# Funcion para aplicar el filtro sharpen fuerte
def sharpen_fuerte(imagen):
    matriz = [[1.0, 1.0, 1.0], 
              [1.0, -7.0, 1.0], 
              [1.0, 1.0, 1.0]]
    factor = 1.0
    bias = 0.0
    num = 3
    return convolucion(imagen, matriz, factor, bias, num)

# Funcion para aplicar el filtro emboss
def emboss(imagen):
    matriz = [[-1.0, -1.0, 0.0], 
              [-1.0, 0.0, 1.0], 
              [0.0, 1.0, 1.0]]
    factor = 1.0
    bias = 128.0
    num = 3
    return convolucion(imagen, matriz, factor, bias, num)

# Funcion para aplicar el filtro emboss fuerte
def emboss_fuerte(imagen):
    matriz = [[-1.0, -1.0, -1.0, -1.0, 0.0], 
              [-1.0, -1.0, -1.0, 0.0, 1.0], 
              [-1.0, -1.0, 0.0, 1.0, 1.0], 
              [-1.0, 0.0, 1.0, 1.0, 1.0], 
              [0.0, 1.0, 1.0, 1.0, 1.0]]
    factor = 1.0
    bias = 128.0
    num = 5
    return convolucion(imagen, matriz, factor, bias, num)

# Funcion para aplicar el filtro promedio
def promedio(imagen):
    matriz = [[1.0, 1.0, 1.0], 
              [1.0, 1.0, 1.0], 
              [1.0, 1.0, 1.0]]
    factor = 1.0 / 9.0
    bias = 0.0
    num = 3
    return convolucion(imagen, matriz, factor, bias, num)

# Funcion para aplicar el filtro de mediana
def mediana(imagen):
    img = imread(imagen).astype(np.uint8)
    alto, ancho, canales = img.shape
    img_nueva = np.copy(img)
    tam = 9
    elegir = tam * tam // 2
    for i in range(alto):
        for j in range(ancho):
            for k in range(3):
                valores = []
                for x in range(tam):
                    for y in range(tam):
                        nx = min(max(i - tam // 2 + x, 0), alto - 1)
                        ny = min(max(j - tam // 2 + y, 0), ancho - 1)
                        valores += [img[nx, ny, k]]
                valores.sort()
                img_nueva[i, j, k] = valores[elegir]
    return img_nueva


# Diccionario de filtros convolucionales
filtros_convolucionales = {
    "Blur": blur,
    "Gaussian blur": gauss_blur,
    "Motion blur": motion_blur,
    "Find edges": find_edges,
    "Sharpen": sharpen,
    "Sharpen fuerte": sharpen_fuerte,
    "Emboss": emboss,
    "Emboss fuerte": emboss_fuerte,
    "Promedio": promedio,
    "Mediana": mediana
}


# Funcion para aplicar un filtro convolucional
def aplicar_filtro_convolucional(tipo):
    global ruta_imagen, panel_modificada, img_modificada
    if not ruta_imagen:
        messagebox.showerror("Error", "Primero selecciona una imagen")
        return
    img_array = filtros_convolucionales[tipo](ruta_imagen)
    img_modificada = Image.fromarray(img_array.astype(np.uint8))
    img_tk = ImageTk.PhotoImage(img_modificada)
    panel_modificada.config(image=img_tk)
    panel_modificada.image = img_tk


# Menu Filtros Convolucionales
menu_filtros_convolucionales = Menu(menubar, tearoff=0)
for nombre, filtro in filtros_convolucionales.items():
    menu_filtros_convolucionales.add_command(label=nombre, command=lambda nombre=nombre: aplicar_filtro_convolucional(nombre))
menubar.add_cascade(label="Filtros Convolucionales", menu=menu_filtros_convolucionales)