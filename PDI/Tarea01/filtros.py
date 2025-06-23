from skimage.io import imread
import tkinter as tk
from tkinter import filedialog, messagebox, simpledialog, Menu
from PIL import Image, ImageTk
import numpy as np

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

# Diccionario de filtros
filtros = {
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

# Funciones para la interfaz grafica

# Funcion para aplicar un filtro sin parametros extras
def aplicar_filtro(tipo):
    global ruta_imagen, panel_modificada, img_modificada
    if not ruta_imagen:
        messagebox.showerror("Error", "Primero selecciona una imagen")
        return
    img_array = filtros[tipo](ruta_imagen)
    img_modificada = Image.fromarray(img_array.astype(np.uint8))
    img_tk = ImageTk.PhotoImage(img_modificada)
    panel_modificada.config(image=img_tk)
    panel_modificada.image = img_tk

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
    img_array = mosaico(ruta_imagen, x, y)
    img_modificada = Image.fromarray(img_array.astype(np.uint8))
    img_tk = ImageTk.PhotoImage(img_modificada)
    panel_modificada.config(image=img_tk)
    panel_modificada.image = img_tk

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
    ruta_salida = filedialog.asksaveasfilename(defaultextension=".png", filetypes=[("Archivos de imagen", "*.png;*.jpg;*.jpeg")])
    if ruta_salida:
        img_modificada.save(ruta_salida)
        messagebox.showinfo("Listo", "La imagen ha sido guardada correctamente.")


# Interfaz grafica

root = tk.Tk()
root.title("Filtro de Imagenes")
root.geometry("800x600")

# Menu
menubar = Menu(root)
root.config(menu=menubar)

# Menu Archivo
menu_archivo = Menu(menubar, tearoff=0)
menu_archivo.add_command(label="Abrir Imagen", command=seleccionar_imagen)
menu_archivo.add_command(label="Guardar Imagen", command=guardar_imagen)
menubar.add_cascade(label="Archivo", menu=menu_archivo)

# Menu Filtros
menu_filtros = Menu(menubar, tearoff=0)
for nombre, filtro in filtros.items():
    if filtro.__code__.co_argcount == 1:
        menu_filtros.add_command(label=nombre, command=lambda nombre=nombre: aplicar_filtro(nombre))
    elif nombre == "Brillo":
        menu_filtros.add_command(label=nombre, command=aplicar_brillo)
    elif nombre == "Componentes RGB":
        menu_filtros.add_command(label=nombre, command=aplicar_componentes_rgb)
    elif nombre == "Mosaico":
        menu_filtros.add_command(label=nombre, command=aplicar_mosaico)
menubar.add_cascade(label="Filtros", menu=menu_filtros)

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
