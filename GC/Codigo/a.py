# Algoritmo de Stationing Guards in a Rectilinear Art Gallery
import matplotlib.pyplot as plt
import sys

# Algoritmo de Stationing Guards in a Rectilinear Art Gallery
def algoritmo_guardias(vertices):
    vertices_lista = vertices.copy()
    dibujar_poligono(vertices_lista, "Poligono Original")
    # Paso 1: contar la cantidad de vertices reflex
    cantidad_reflex = contar_reflex(vertices)
    # Si la cantidad de vertices reflex es par, entonces agregar uno extra
    vertices_agregados = []
    vertices_quitados = []
    if cantidad_reflex % 2 == 0:
        vertices_lista, vertice1, vertice2, vertice3, vertice4 = agregar_reflex(vertices)
        vertices_agregados.append(vertice1)
        vertices_agregados.append(vertice2)
        vertices_agregados.append(vertice3)
        vertices_quitados.append(vertice4)
    dibujar_poligonos_paso_1(vertices, vertices_lista)
    # Paso 2: hacer barrido de linea para encontrar los cortes horizontales y agregar vertices artificiales
    vertices_barrido = vertices_lista.copy()
    vertices_luego_barrido, vertices_artificiales, cortes_horizontales = barrido_linea(vertices_barrido)
    dibujar_poligonos_paso_2(vertices, vertices_barrido, vertices_artificiales, cortes_horizontales)
    # Paso 3: obtener la paridad de los cortes horizontales
    vertices_reflex = obtener_reflex(vertices_barrido)
    cortes_paridad = obtener_paridad(vertices_luego_barrido, vertices_reflex, vertices_artificiales, cortes_horizontales)
    dibujar_poligonos_paso_3(vertices_barrido, vertices_artificiales, cortes_horizontales, cortes_paridad)
    # Paso 4: hacer cortes en los cortes horizontales impares
    cortes_impares = []
    for corte in cortes_paridad:
        if corte[1] % 2 != 0:
            cortes_impares.append(corte[0])
    poligonos = partir_poligono(vertices_luego_barrido, cortes_impares)
    dibujar_poligonos_paso_4(vertices_barrido, vertices_artificiales, cortes_paridad, poligonos)
    # Paso 5: poner los guardias
    guardias = []
    cortes_verticales = []
    for poligono in poligonos:
        guardias_puestos, cortes_verticales_puestos = poner_guardias(poligono)
        for guardia in guardias_puestos:
            guardias.append(guardia)
        for corte in cortes_verticales_puestos:
            cortes_verticales.append(corte)
    dibujar_poligonos_paso_5(poligonos, guardias, cortes_verticales)
    # Paso 6: quitar el vertice reflex agregado en el paso 1, si es que se agrego
    vertices_final = vertices_barrido.copy()
    if len(vertices_agregados) > 0:
        vertice1 = vertices_agregados[0]
        vertice2 = vertices_agregados[1]
        vertice3 = vertices_agregados[2]
        vertice4 = vertices_quitados[0]
        vertices_final = quitar_reflex(vertices_final, vertice1, vertice2, vertice3, vertice4)
    dibujar_poligonos_paso_6(poligonos, guardias, cortes_verticales, vertices_final)
    # Paso 7: mostrar los resultados
    print("Vertices:", len(vertices_final))
    print("Vertices reflex:" , contar_reflex(vertices_final))
    print("Guardias:", len(guardias))
    dibujar_poligonos_final(vertices, vertices_final, guardias)

# Metodo para contar los vertices reflex de un poligono, un vertice reflex es aquel que tiene un angulo interno mayor a 180 grados
def contar_reflex(vertices):
    cantidad = 0
    n = len(vertices)
    for i in range(n):
        p1 = vertices[i]
        p2 = vertices[(i + 1) % n]
        p3 = vertices[(i + 2) % n]
        # Calcular el producto cruzado
        cross_product = (p2[0] - p1[0]) * (p3[1] - p2[1]) - (p2[1] - p1[1]) * (p3[0] - p2[0])
        if cross_product < 0:
            cantidad += 1
    return cantidad

# Metodo para agregar un vertice reflex extra a un poligono
def agregar_reflex(vertices):
    # Paso 1: elegir un vertice convex cualquiera
    listo = False
    v = None
    while not listo:
        for i in range(len(vertices)):
            p1 = vertices[i]
            p2 = vertices[(i + 1) % len(vertices)]
            p3 = vertices[(i + 2) % len(vertices)]
            # Calcular el producto cruzado
            cross_product = (p2[0] - p1[0]) * (p3[1] - p2[1]) - (p2[1] - p1[1]) * (p3[0] - p2[0])
            if cross_product > 0:
                v = p2
                listo = True
                break
    # Paso 2: comparar las coordenadas de v con las de los demas vertices para encontrar la menor separacion (diferente a cero)
    triangulo_x = sys.maxsize
    triangulo_x_abs = sys.maxsize
    triangulo_y = sys.maxsize
    triangulo_y_abs = sys.maxsize
    for i in range(len(vertices)):
        p = vertices[i]
        if p != v:
            # Calcular la separacion horizontal
            if triangulo_x == sys.maxsize or abs(p[0] - v[0]) < triangulo_x_abs:
                # Si la separacion es menor a cero, entonces no se considera
                if abs(p[0] - v[0]) > 0:
                    triangulo_x_abs = abs(p[0] - v[0])
                    triangulo_x = p[0] - v[0]
            # Calcular la separacion vertical
            if triangulo_y == sys.maxsize or abs(p[1] - v[1]) < triangulo_y_abs:
                # Si la separacion es menor a cero, entonces no se considera
                if abs(p[1] - v[1]) > 0:
                    triangulo_y_abs = abs(p[1] - v[1])
                    triangulo_y = p[1] - v[1]
    # Paso 3: borrar al vertice v y agregar tres vertices nuevos (x + 🛆x/2, y + 🛆y/2), (x, y + 🛆y/2) y (x + 🛆x/2, y)
    nuevo_1 = (v[0] + triangulo_x / 2, v[1] + triangulo_y / 2)
    nuevo_2 = (v[0], v[1] + triangulo_y / 2)
    nuevo_3 = (v[0] + triangulo_x / 2, v[1])
    # Revisamos si el vertice nuevo_1 esta dentro del poligono, en caso de que no, cambiamos los signos de triangulo_x y triangulo_y
    if not punto_en_poligono(nuevo_1, vertices):
        nuevo_1 = (v[0] - triangulo_x / 2, v[1] + triangulo_y / 2)
        nuevo_2 = (v[0], v[1] + triangulo_y / 2)
        nuevo_3 = (v[0] - triangulo_x / 2, v[1])
        if not punto_en_poligono(nuevo_1, vertices):
            nuevo_1 = (v[0] - triangulo_x / 2, v[1] - triangulo_y / 2)
            nuevo_2 = (v[0], v[1] - triangulo_y / 2)
            nuevo_3 = (v[0] - triangulo_x / 2, v[1])
            if not punto_en_poligono(nuevo_1, vertices):
                nuevo_1 = (v[0] + triangulo_x / 2, v[1] - triangulo_y / 2)
                nuevo_2 = (v[0], v[1] - triangulo_y / 2)
                nuevo_3 = (v[0] + triangulo_x / 2, v[1])
    vertices_copia = vertices.copy()
    indice = vertices.index(v)
    # Obtenemos los vertices siguientes y anteriores
    siguiente = vertices[(indice + 1) % len(vertices)]
    anterior = vertices[(indice - 1) % len(vertices)]
    # Obtenemos la direccion del vertice reflex con respecto al vertice
    if nuevo_1[0] < v[0]:
        if nuevo_1[1] < v[1]:
            direccion = "izq-abajo"
        else:
            direccion = "izq-arriba"
    else:
        if nuevo_1[1] < v[1]:
            direccion = "der-abajo"
        else:
            direccion = "der-arriba"
    # Revisamos si la arista formada de vertice con anterior es horizontal o vertical
    if v[0] == anterior[0]:
        arista_anterior = "vertical"
    else:
        arista_anterior = "horizontal"
    # Revisamos si la arista formada de vertice con siguiente es horizontal o vertical
    if v[0] == siguiente[0]:
        arista_siguiente = "vertical"
    else:
        arista_siguiente = "horizontal"
    # Dependiendo de la direccion y las aristas vemos cuales vertices agregar primero
    if direccion == "izq-abajo":
        if arista_anterior == "horizontal" and arista_siguiente == "vertical":
            primero = nuevo_2
            segundo = nuevo_3
        elif arista_anterior == "vertical" and arista_siguiente == "horizontal":
            primero = nuevo_3
            segundo = nuevo_2
    elif direccion == "izq-arriba":
        if arista_anterior == "horizontal" and arista_siguiente == "vertical":
            primero = nuevo_2
            segundo = nuevo_3
        elif arista_anterior == "vertical" and arista_siguiente == "horizontal":
            primero = nuevo_3
            segundo = nuevo_2
    elif direccion == "der-abajo":
        if arista_anterior == "horizontal" and arista_siguiente == "vertical":
            primero = nuevo_2
            segundo = nuevo_3
        elif arista_anterior == "vertical" and arista_siguiente == "horizontal":
            primero = nuevo_3
            segundo = nuevo_2
    elif direccion == "der-arriba":
        if arista_anterior == "horizontal" and arista_siguiente == "vertical":
            primero = nuevo_2
            segundo = nuevo_3
        elif arista_anterior == "vertical" and arista_siguiente == "horizontal":
            primero = nuevo_3
            segundo = nuevo_2
    vertices_copia.pop(indice)
    vertices_copia.insert(indice, primero)
    vertices_copia.insert(indice, nuevo_1)
    vertices_copia.insert(indice, segundo)
    return vertices_copia, nuevo_1, nuevo_2, nuevo_3, v

# Metodo para ver si un punto esta dentro de un poligono
def punto_en_poligono(punto, vertices):
    x, y = punto
    n = len(vertices)
    dentro = False
    p1x, p1y = vertices[0]
    for i in range(n + 1):
        p2x, p2y = vertices[i % n]
        if y > min(p1y, p2y):
            if y <= max(p1y, p2y):
                if x <= max(p1x, p2x):
                    if p1y != p2y:
                        xinters = (y - p1y) * (p2x - p1x) / (p2y - p1y) + p1x
                    if p1x == p2x or x <= xinters:
                        dentro = not dentro
        p1x, p1y = p2x, p2y
    return dentro

# Metodo para realizar el barrido de linea y encontrar los cortes horizontales
def barrido_linea(vertices):
    # Paso 1: ordenar los vertices por coordenada y (si hay empate, por coordenada x)
    vertices_ordenados = sorted(vertices, key=lambda x: (-x[1], x[0]))
    # Paso 2: creamos un estado de la linea de barrido, una lista de aristas verticales (solo seran los vertices de inicio y fin)
    estado_linea = []
    # Paso 3: creamos una lista de eventos, donde cada evento es un vertice, su tipo (inicio o fin) y la arista vertical
    eventos = []
    for i in range(len(vertices_ordenados)):
        p = vertices_ordenados[i]
        # Si el vertice es de inicio, lo agregamos a la lista de eventos
        es_inicio, arista, lado_c, reflex = revisar_evento(p, vertices)
        if es_inicio:
            eventos.append((p, "inicio", arista, lado_c, reflex))
        # Si el vertice es de fin, lo agregamos a la lista de eventos
        else:
            eventos.append((p, "fin", arista, lado_c, reflex))
    # Paso 4: recorrer los eventos
    vertices_artificiales = []
    cortes_horizontales = []
    vertices_combinados = vertices.copy()
    total = len(eventos)
    for i in range(total):
        # Obtenemos el evento actual
        evento = eventos[i]
        punto = evento[0]
        tipo = evento[1]
        arista = evento[2]
        lado_c = evento[3]
        reflex = evento[4]
        # Si no es reflex
        if not reflex:
            # Revisamos si es de inicio o fin
            if tipo == "inicio":
                # Agregamos la arista a la lista de eventos, de manera ordenada con respecto a la coordenada x
                agregar_arista(arista, estado_linea)
            else:
                # Buscamos la arista en la lista de eventos y la eliminamos
                arista = buscar_arista(punto, estado_linea)
                estado_linea.remove(arista)
        # Si es reflex
        else:
            # Revisamos si es de inicio o fin
            if tipo == "inicio":
                # Agregamos la arista a la lista de eventos, de manera ordenada con respecto a la coordenada x
                agregar_arista(arista, estado_linea)
            # Buscamos la arista a la izquierda o derecha de la arista relacionada al evento
            if lado_c == "izq":
                if tipo == "fin":
                    arista = buscar_arista(punto, estado_linea)
                # Buscamos la arista a la izquierda
                arista_izquierda = estado_linea[estado_linea.index(arista) - 1]
                # Obtenemos la interseccion entre la arista encontrada y el punto
                interseccion = (arista_izquierda[0][0], punto[1])
                # Agregamos el nuevo vertice a la lista de vertices artificiales
                vertices_artificiales.append(interseccion)
                # Agregamos el nuevo vertice a la lista de vertices, entre el vertice de inicio y fin de la arista encontrada
                vertice1 = arista_izquierda[0]
                vertice2 = arista_izquierda[1]
                # Buscamos el vertice en la lista de vertices
                indice1 = vertices_combinados.index(vertice1)
                indice2 = vertices_combinados.index(vertice2)
                indice3 = min(indice1, indice2)
                # Si los indices1 y 2 son el indice 0 y len(vertices_combinados) - 1, entonces el indice a insertar es 0
                if indice1 == 0 and indice2 == len(vertices_combinados) - 1:
                    indice3 = -1
                # Si los indices1 y 2 son el indice len(vertices_combinados) - 1 y 0, entonces el indice a insertar es 0
                elif indice1 == len(vertices_combinados) - 1 and indice2 == 0:
                    indice3 = -1
                # Agregamos el nuevo vertice a la lista de vertices
                vertices_combinados.insert((indice3 + 1) % len(vertices_combinados), interseccion)
                # Actualizamos el estado de la linea de barrido, reemplazando la arista encontrada por la arista del vertice artificial al vertice de fin
                arista_nueva = (interseccion, vertice2)
                # Buscamos la arista en la lista de eventos y la eliminamos
                estado_linea.remove(arista_izquierda)
                # Agregamos la nueva arista a la lista de eventos, de manera ordenada con respecto a la coordenada x
                agregar_arista(arista_nueva, estado_linea)
                # Agregamos el nuevo corte horizontal a la lista de cortes horizontales
                if interseccion[0] < punto[0]:
                    cortes_horizontales.append((interseccion, punto))
                else:
                    cortes_horizontales.append((punto, interseccion))
            else:
                if tipo == "fin":
                    arista = buscar_arista(punto, estado_linea)
                # Buscamos la arista a la derecha
                arista_derecha = estado_linea[estado_linea.index(arista) + 1]
                # Obtenemos la interseccion entre la arista encontrada y el punto
                interseccion = (arista_derecha[0][0], punto[1])
                # Agregamos el nuevo vertice a la lista de vertices artificiales
                vertices_artificiales.append(interseccion)
                # Agregamos el nuevo vertice a la lista de vertices, entre el vertice de inicio y fin de la arista encontrada
                vertice1 = arista_derecha[0]
                vertice2 = arista_derecha[1]
                # Buscamos el vertice en la lista de vertices
                indice1 = vertices_combinados.index(vertice1)
                indice2 = vertices_combinados.index(vertice2)
                indice3 = min(indice1, indice2)
                # Si los indices1 y 2 son el indice 0 y len(vertices_combinados) - 1, entonces el indice a insertar es 0
                if indice1 == 0 and indice2 == len(vertices_combinados) - 1:
                    indice3 = -1
                # Si los indices1 y 2 son el indice len(vertices_combinados) - 1 y 0, entonces el indice a insertar es 0
                elif indice1 == len(vertices_combinados) - 1 and indice2 == 0:
                    indice3 = -1
                # Agregamos el nuevo vertice a la lista de vertices
                vertices_combinados.insert((indice3 + 1) % len(vertices_combinados), interseccion)
                # Actualizamos el estado de la linea de barrido, reemplazando la arista encontrada por la arista del vertice artificial al vertice de fin
                arista_nueva = (interseccion, vertice2)
                # Buscamos la arista en la lista de eventos y la eliminamos
                estado_linea.remove(arista_derecha)
                # Agregamos la nueva arista a la lista de eventos, de manera ordenada con respecto a la coordenada x
                agregar_arista(arista_nueva, estado_linea)
                # Agregamos el nuevo corte horizontal a la lista de cortes horizontales
                if interseccion[0] < punto[0]:
                    cortes_horizontales.append((interseccion, punto))
                else:
                    cortes_horizontales.append((punto, interseccion))
            # Revisamos si es de fin
            if tipo == "fin":
                # Buscamos la arista en la lista de eventos y la eliminamos
                estado_linea.remove(arista)
        #print(estado_linea)
    return vertices_combinados, vertices_artificiales, cortes_horizontales

# Metodo para revisar si un vertice es de inicio o fin y obtener la arista vertical
def revisar_evento(p, vertices):
    # Buscamos el vertice en la lista de vertices
    indice = vertices.index(p)
    # Revisamos los vertices anteriores y siguientes para ver si es de inicio o fin de la arista vertical
    siguiente = vertices[(indice + 1) % len(vertices)]
    anterior = vertices[(indice - 1) % len(vertices)]
    siguente_2 = vertices[(indice + 2) % len(vertices)]
    # Revisamos si el vertice es reflex
    reflex = False
    # Calcular el producto cruzado
    cross_product = (siguiente[0] - anterior[0]) * (p[1] - anterior[1]) - (siguiente[1] - anterior[1]) * (p[0] - anterior[0])
    if cross_product > 0:
        reflex = True
    # Revisamos si el vertice forma una arista vertical con el vertice siguiente o anterior
    if p[0] == siguiente[0]:
        # Checamos si el vertice es de inicio o fin
        if p[1] > siguiente[1]:
            # Revisamos si el vertice anterior esta a la izquierda o derecha
            if p[0] > anterior[0]:
                return True, (p, siguiente), "der", reflex
            else:
                return True, (p, siguiente), "izq", reflex
        else:
            # Revisamos si el vertice anterior esta a la izquierda o derecha
            if p[0] > anterior[0]:
                return False, (siguiente, p), "der", reflex
            else:
                return False, (siguiente, p), "izq", reflex
    elif p[0] == anterior[0]:
        # Checamos si el vertice es de inicio o fin
        if p[1] > anterior[1]:
            # Revisamos si el vertice siguiente esta a la izquierda o derecha
            if p[0] > siguiente[0]:
                return True, (p, anterior), "der", reflex
            else:
                return True, (p, anterior), "izq", reflex
        else:
            # Revisamos si el vertice siguiente esta a la izquierda o derecha
            if p[0] > siguiente[0]:
                return False, (anterior, p), "der", reflex
            else:
                return False, (anterior, p), "izq", reflex
    else:
        # Si no es ni inicio ni fin, lo ignoramos
        return False, None, None, reflex
    
# Metodo para agregar una arista vertical a la lista de eventos de manera ordenada con respecto a la coordenada x
def agregar_arista(arista, lista_eventos):
    # Buscamos el lugar donde agregar la arista
    for i in range(len(lista_eventos)):
        if lista_eventos[i][0][0] > arista[0][0]:
            lista_eventos.insert(i, arista)
            return
    # Si no se encontro un lugar, lo agregamos al final
    lista_eventos.append(arista)

# Metodo para buscar una arista vertical en la lista de eventos dado un vertice que la forma
def buscar_arista(vertice, lista_eventos):
    # Buscamos el vertice en la lista de eventos
    for i in range(len(lista_eventos)):
        if lista_eventos[i][0] == vertice or lista_eventos[i][1] == vertice:
            return lista_eventos[i]
    return None

# Metodo para obtener la lista de los vertices reflex de un poligono
def obtener_reflex(vertices):
    vertices_reflex = []
    n = len(vertices)
    for i in range(n):
        p1 = vertices[i]
        p2 = vertices[(i + 1) % n]
        p3 = vertices[(i + 2) % n]
        # Calcular el producto cruzado
        cross_product = (p2[0] - p1[0]) * (p3[1] - p2[1]) - (p2[1] - p1[1]) * (p3[0] - p2[0])
        if cross_product < 0:
            vertices_reflex.append(p2)
    return vertices_reflex

# Metodo para obtener la paridad de los cortes horizontales
def obtener_paridad(vertices, vertices_reflex, vertices_artificiales, cortes_horizontales):
    cortes_reflex_artificial_paridad = []
    for corte in cortes_horizontales:
        cortes_reflex_artificial_paridad.append([corte, -1, -1, -1])
    contador = 0
    # Paso 1: recorrer los vertices del poligono
    for i in range(len(vertices)):
        # Obtenemos el vertice actual
        vertice = vertices[i]
        # Revisamos si es artificial
        if vertice in vertices_artificiales:
            # Si es artificial, lo marcamos con el valor actual del contador
            for corte in cortes_reflex_artificial_paridad:
                if corte[0][0] == vertice or corte[0][1] == vertice:
                    corte[2] = contador
        # Revisamos si es reflex
        elif vertice in vertices_reflex:
            # Si es reflex, aumentamos en uno el contador y lo marcamos con el nuevo valor del contador
            for corte in cortes_reflex_artificial_paridad:
                if corte[0][0] == vertice or corte[0][1] == vertice:
                    contador += 1
                    corte[1] = contador
        # Revisamos si es convex
        else:
            # Si es convex, no hacemos nada
            pass
    # Paso 2: recorrer los vertices para obtener la paridad de los cortes horizontales
    for i in range(len(vertices)):
        # Obtenemos el vertice actual
        vertice = vertices[i]
        # Revisamos si es artificial
        if vertice in vertices_artificiales:
            # Si es artificial, buscamos el corte que lo contiene
            corte_cont = None
            for corte in cortes_reflex_artificial_paridad:
                if corte[0][0] == vertice or corte[0][1] == vertice:
                    corte_cont = corte
                    break
            # Si el corte no tiene paridad aun, entonces la calculamos
            if corte_cont[3] == -1:
                paridad = corte_cont[1] - corte_cont[2] - 1
                corte_cont[3] = paridad
        # Revisamos si es reflex
        elif vertice in vertices_reflex:
            # Si es reflex, buscamos el corte que lo contiene
            corte_cont = None
            for corte in cortes_reflex_artificial_paridad:
                if corte[0][0] == vertice or corte[0][1] == vertice:
                    corte_cont = corte
                    break
            # Si el corte no tiene paridad aun, entonces la calculamos
            if corte_cont[3] == -1:
                paridad = corte_cont[2] - corte_cont[1]
                corte_cont[3] = paridad
        # Revisamos si es convex
        else:
            # Si es convex, no hacemos nada
            pass
    cortes_paridad = []
    for corte in cortes_reflex_artificial_paridad:
        cortes_paridad.append((corte[0], corte[3]))
    return cortes_paridad

# Metodo para partir un poligono en varios poligonos en los cortes horizontales impares
def partir_poligono(vertices, cortes_horizontales):
    # Paso 1: crear una lista de poligonos
    poligonos = [vertices]
    # Paso 2: recorrer los cortes horizontales
    for corte in cortes_horizontales:
        # Obtenemos el vertice de inicio y fin
        vertice_inicio = corte[0]
        vertice_fin = corte[1]
        # Buscamos el poligono que contiene el corte
        poligono_cont = None
        for poligono in poligonos:
            if vertice_inicio in poligono and vertice_fin in poligono:
                poligono_cont = poligono
                break
        # Si encontramos el poligono, lo partimos en dos poligonos
        if poligono_cont is not None:
            # Buscamos el indice del vertice de inicio y fin
            indice_inicio = poligono_cont.index(vertice_inicio)
            indice_fin = poligono_cont.index(vertice_fin)
            # Si el indice de inicio es mayor al de fin, entonces los intercambiamos
            if indice_inicio > indice_fin:
                indice_inicio, indice_fin = indice_fin, indice_inicio
            # Partimos el poligono en dos
            poligono1 = poligono_cont[indice_inicio:indice_fin + 1]
            poligono2 = poligono_cont[indice_fin:] + poligono_cont[:indice_inicio + 1]
            # Agregamos los nuevos poligonos a la lista de poligonos
            poligonos.remove(poligono_cont)
            poligonos.append(poligono1)
            poligonos.append(poligono2)
    return poligonos

# Metodo para poner los guardias en un poligono
def poner_guardias(poligono):
    # Paso 1: obtenemos los vertices reflex del poligono
    vertices_reflex = obtener_reflex(poligono)
    # Paso 2: revisamos cuantos vertices reflex tiene el poligono
    cantidad_reflex = len(vertices_reflex)
    cortes_verticales = []
    guardias = []
    # Paso 3: si la cantidad de vertices reflex es 0, entonces ponemos un guardia en cualquier vertice
    if cantidad_reflex == 0:
        guardia = poligono[0]
        return [guardia], []
    # Paso 4: si la cantidad de vertices reflex es 1, entonces ponemos un guardia en el vertice reflex
    elif cantidad_reflex == 1:
        guardia = vertices_reflex[0]
        return [guardia], []
    # Paso 5: si la cantidad de vertices reflex es mayor a 1, entonces ordenamos los vertices reflex por coordenada x
    else:
        vertices_reflex = sorted(vertices_reflex, key=lambda x: x[0])
        # Paso 6: recorremos los vertices reflex
        for i in range(len(vertices_reflex)):
            # Obtenemos el vertice actual
            vertice = vertices_reflex[i]
            # Si el vertice esta en la posicion impar, ponemos un guardia en el vertice
            if i % 2 == 0:
                guardia = vertice
                # Agregamos el guardia a la lista de guardias
                guardias.append(guardia)
            # Si el vertice esta en la posicion par, entonces ponemos un corte vertical en el vertice
            else:
                # Obtenemos el indice del vertice en el poligono
                indice = poligono.index(vertice)
                # Obtenemos el vertice anterior y siguiente
                vertice_anterior = poligono[(indice - 1) % len(poligono)]
                vertice_siguiente = poligono[(indice + 1) % len(poligono)]
                # Revisamos si el corte vertical debe ser hacia arriba o hacia abajo
                direccion = None
                if vertice[0] == vertice_anterior[0]:
                    # Si el vertice anterior y el vertice estan en la misma coordenada x, revisamos cual esta arriba
                    if vertice[1] > vertice_anterior[1]:
                        # Si el vertice esta arriba, entonces el corte es hacia arriba
                        direccion = "arriba"
                    else:
                        # Si el vertice anterior esta arriba, entonces el corte es hacia abajo
                        direccion = "abajo"
                elif vertice[0] == vertice_siguiente[0]:
                    # Si el vertice siguiente y el vertice estan en la misma coordenada x, revisamos cual esta arriba
                    if vertice[1] > vertice_siguiente[1]:
                        # Si el vertice esta arriba, entonces el corte es hacia arriba
                        direccion = "arriba"
                    else:
                        # Si el vertice siguiente esta arriba, entonces el corte es hacia abajo
                        direccion = "abajo"
                else:
                    direccion = None
                    # si la direccion es None, entonces no se hace nada
                if direccion is not None:
                    # Buscamos la arista horizontal que se intersecta con el corte vertical y es la mas cercana al vertice en la direccion
                    arista = None
                    for j in range(len(poligono)):
                        # Obtenemos el vertice actual
                        vertice_actual = poligono[j]
                        vertice_siguiente = poligono[(j + 1) % len(poligono)]
                        # Si el vertice actual o el siguente son el vertice, los ignoramos
                        if vertice_actual == vertice or vertice_siguiente == vertice:
                            continue
                        # Revisamos si el vertice actual y el siguiente forman una arista horizontal
                        if vertice_actual[1] == vertice_siguiente[1]:
                            # Revisamos si el corte vertical intersecta la arista horizontal
                            if vertice[0] >= min(vertice_actual[0], vertice_siguiente[0]) and vertice[0] <= max(vertice_actual[0], vertice_siguiente[0]):
                                # Si el corte vertical intersecta la arista horizontal, revisamos si la interseeccion es la mas cercana al vertice en la direccion
                                if direccion == "arriba":
                                    if arista is None:
                                        # Revisamos que la nueva interseccion este arriba del vertice
                                        interseccion_nueva = (vertice[0], vertice_actual[1])
                                        if vertice[1] < interseccion_nueva[1]:
                                            arista = (vertice_actual, vertice_siguiente)
                                    else:
                                        interseccion_actual = (vertice[0], arista[0][1])
                                        interseccion_nueva = (vertice[0], vertice_actual[1])
                                        # Revisamos si la nueva interseccion es la mas cercana al vertice en la direccion
                                        if abs(vertice[1] - interseccion_actual[1]) < abs(vertice[1] - interseccion_nueva[1]):
                                            # Revisamos si la nueva interseccion esta arriba del vertice
                                            if vertice[1] < interseccion_nueva[1]:
                                                arista = (vertice_actual, vertice_siguiente)
                                elif direccion == "abajo":
                                    if arista is None:
                                        # Revisamos que la nueva interseccion este abajo del vertice
                                        interseccion_nueva = (vertice[0], vertice_actual[1])
                                        if vertice[1] > interseccion_nueva[1]:
                                            arista = (vertice_actual, vertice_siguiente)
                                    else:
                                        interseccion_actual = (vertice[0], arista[0][1])
                                        interseccion_nueva = (vertice[0], vertice_actual[1])
                                        # Revisamos si la nueva interseccion es la mas cercana al vertice en la direccion
                                        if abs(vertice[1] - interseccion_actual[1]) < abs(vertice[1] - interseccion_nueva[1]):
                                            # Revisamos si la nueva interseccion esta abajo del vertice
                                            if vertice[1] > interseccion_nueva[1]:
                                                arista = (vertice_actual, vertice_siguiente)
                    # Si encontramos la arista, obtenemos el vertice de interseccion
                    if arista is not None:
                        # Obtenemos el vertice de interseccion
                        vertice_interseccion = (vertice[0], arista[0][1])
                        # Agregamos el corte vertical a la lista de cortes verticales
                        cortes_verticales.append((vertice, vertice_interseccion))
    return guardias, cortes_verticales

# Metodo para quitar un vertice reflex
def quitar_reflex(vertices, vertice1, vertice2, vertice3, vertice4):
    # Paso 1: buscar el vertice en la lista de vertices
    indice1 = vertices.index(vertice1)
    # Paso 2: agregar el vertice4 en la posicion del vertice1
    vertices.insert(indice1, vertice4)
    # Paso 3: borrar los vertices 1, 2 y 3
    vertices.remove(vertice1)
    vertices.remove(vertice2)
    vertices.remove(vertice3)
    return vertices

# Metodo para volver un poligono de texto a una lista de parejas que representan los vertices
def convertir_a_lista(poligono_texto):
    # Eliminar los paréntesis y dividir por los guiones
    vertices = poligono_texto.replace("(", "").replace(")", "").split("-")
    # Convertir cada par de coordenadas en una tupla de enteros
    return [tuple(map(int, vertex.split(","))) for vertex in vertices if vertex.strip()]

# Metodo para dibujar el poligono usando una lista de vertices
def dibujar_poligono(vertices, texto):
    # Separar las coordenadas x e y
    x, y = zip(*vertices)
    # Aniadir el primer punto al final para cerrar el poligono
    x += (x[0],)
    y += (y[0],)
    # Dibujar el poligono
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Mostrar el texto
    plt.title(texto)
    plt.tight_layout()
    plt.show()

# Metodo para dibujar el poligono original y el poligono modificado, uno al lado del otro
def dibujar_poligonos_paso_1(vertices_original, vertices_modificado):
    # Dibujar el poligono original
    plt.subplot(1, 2, 1)
    x, y = zip(*vertices_original)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar el poligono modificado
    plt.subplot(1, 2, 2)
    x, y = zip(*vertices_modificado)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Mostrar el texto
    plt.suptitle("Paso 1")
    plt.tight_layout()
    plt.show()

# Metodo para dibujar el poligono original y el poligono modificado, uno al lado del otro
def dibujar_poligonos_paso_2(vertices_original, vertices_modificados, vertices_artificiales, cortes_horizontales):
    # Dibujar el poligono original
    plt.subplot(1, 2, 1)
    x, y = zip(*vertices_original)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar el poligono modificado
    plt.subplot(1, 2, 2)
    x, y = zip(*vertices_modificados)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar los vertices artificiales de color rojo
    x_artificiales, y_artificiales = zip(*vertices_artificiales)
    plt.scatter(x_artificiales, y_artificiales, color='red', label='Vertices Artificiales')
    # Dibujar los cortes horizontales de color morado
    for corte in cortes_horizontales:
        x_corte, y_corte = zip(*corte)
        plt.plot(x_corte, y_corte, color='purple', linestyle='--', label='Corte Horizontal')
    # Mostrar el texto
    plt.suptitle("Paso 2")
    plt.tight_layout()
    plt.show()

# Metodo para dibujar el poligono original y el poligono modificado, uno al lado del otro
def dibujar_poligonos_paso_3(vertices, vertices_artificiales, cortes_horizontales, cortes_paridad):
    # Dibujar el poligono original
    plt.subplot(1, 2, 1)
    x, y = zip(*vertices)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar los vertices artificiales de color rojo
    x_artificiales, y_artificiales = zip(*vertices_artificiales)
    plt.scatter(x_artificiales, y_artificiales, color='red', label='Vertices Artificiales')
    # Dibujar los cortes horizontales de color morado
    for corte in cortes_horizontales:
        x_corte, y_corte = zip(*corte)
        plt.plot(x_corte, y_corte, color='purple', linestyle='--', label='Corte Horizontal')
    # Dibujar el poligono modificado
    plt.subplot(1, 2, 2)
    x, y = zip(*vertices)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar los vertices artificiales de color rojo
    x_artificiales, y_artificiales = zip(*vertices_artificiales)
    plt.scatter(x_artificiales, y_artificiales, color='red', label='Vertices Artificiales')
    # Dibujar los cortes horizontales de color morado
    for corte, paridad in cortes_paridad:
        x_corte, y_corte = zip(*corte)
        plt.plot(x_corte, y_corte, color='purple', linestyle='--')
        # Mostrar texto de paridad en el centro del corte
        x_centro = sum(x_corte) / 2
        y_centro = sum(y_corte) / 2
        offset_vertical = 0.1 # Desplazamiento vertical para el texto
        plt.text(
            x_centro, y_centro + offset_vertical, paridad,
            color='black', fontsize=8, ha='center', va='bottom',
            bbox=dict(facecolor='white', alpha=0.6, edgecolor='none')
        )
    # Mostrar el texto
    plt.suptitle("Paso 3")
    plt.tight_layout()
    plt.show()

# Metodo para dibujar el poligono original y los poligonos resultantes de los cortes, uno al lado del otro
def dibujar_poligonos_paso_4(vertices, vertices_artificiales, cortes_paridad, poligonos):
    # Dibujar el poligono original
    plt.subplot(1, 2, 1)
    x, y = zip(*vertices)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar los vertices artificiales de color rojo
    x_artificiales, y_artificiales = zip(*vertices_artificiales)
    plt.scatter(x_artificiales, y_artificiales, color='red', label='Vertices Artificiales')
    # Dibujar los cortes horizontales de color morado
    for corte, paridad in cortes_paridad:
        x_corte, y_corte = zip(*corte)
        plt.plot(x_corte, y_corte, color='purple', linestyle='--')
        # Mostrar texto de paridad en el centro del corte
        x_centro = sum(x_corte) / 2
        y_centro = sum(y_corte) / 2
        offset_vertical = 0.1 # Desplazamiento vertical para el texto
        plt.text(
            x_centro, y_centro + offset_vertical, paridad,
            color='black', fontsize=8, ha='center', va='bottom',
            bbox=dict(facecolor='white', alpha=0.6, edgecolor='none')
        )
    # Dibujar los poligonos resultantes de los cortes, cada uno con un color diferente
    plt.subplot(1, 2, 2)
    # Paleta de colores para los poligonos
    cmap = plt.colormaps.get_cmap('tab10_r').resampled(len(poligonos))
    if len(poligonos) > 10:
        cmap = plt.colormaps.get_cmap('tab20_r').resampled(len(poligonos))
    # Dibujar cada poligono
    for i, poligono in enumerate(poligonos):
        if len(poligono) < 3:
            continue 
        x, y = zip(*poligono)
        x += (x[0],)
        y += (y[0],)
        plt.plot(x, y, marker='o', color=cmap(i))
        plt.fill(x, y, alpha=0.3, color=cmap(i))
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Mostrar el texto
    plt.suptitle("Paso 4")
    plt.tight_layout()
    plt.show()

# Metodo para dibujar los poligonos resultantes de los cortes y los poligonos con guardias, uno al lado del otro
def dibujar_poligonos_paso_5(poligonos, guardias, cortes_verticales):
    # Dibujar los poligonos resultantes de los cortes
    plt.subplot(1, 2, 1)
    # Paleta de colores para los poligonos
    cmap = plt.colormaps.get_cmap('tab10_r').resampled(len(poligonos))
    if len(poligonos) > 10:
        cmap = plt.colormaps.get_cmap('tab20_r').resampled(len(poligonos))
    # Dibujar cada poligono
    for i, poligono in enumerate(poligonos):
        if len(poligono) < 3:
            continue 
        x, y = zip(*poligono)
        x += (x[0],)
        y += (y[0],)
        plt.plot(x, y, marker='o', color=cmap(i))
        plt.fill(x, y, alpha=0.3, color=cmap(i))
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar los poligonos con guardias
    plt.subplot(1, 2, 2)
    # Paleta de colores para los poligonos
    cmap = plt.colormaps.get_cmap('tab10_r').resampled(len(poligonos))
    if len(poligonos) > 10:
        cmap = plt.colormaps.get_cmap('tab20_r').resampled(len(poligonos))
    # Dibujar cada poligono
    for i, poligono in enumerate(poligonos):
        if len(poligono) < 3:
            continue 
        x, y = zip(*poligono)
        x += (x[0],)
        y += (y[0],)
        plt.plot(x, y, marker='o', color=cmap(i))
        plt.fill(x, y, alpha=0.3, color=cmap(i))
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar los guardias de color negro
    x_guardias, y_guardias = zip(*guardias)
    plt.scatter(x_guardias, y_guardias, color='black', label='Guardias', zorder=5)
    # Dibujar los cortes verticales de color verde
    for corte in cortes_verticales:
        x_corte, y_corte = zip(*corte)
        plt.plot(x_corte, y_corte, color='green', linestyle='--', label='Corte Vertical')
    # Mostrar el texto
    plt.suptitle("Paso 5")
    plt.tight_layout()
    plt.show()

# Metodo para dibujar los poligonos con guardias y el poligono original con guardias, uno al lado del otro
def dibujar_poligonos_paso_6(poligonos, guardias, cortes_verticales, vertices):
    # Dibujar los poligonos con guardias
    plt.subplot(1, 2, 1)
    # Paleta de colores para los poligonos
    cmap = plt.colormaps.get_cmap('tab10_r').resampled(len(poligonos))
    if len(poligonos) > 10:
        cmap = plt.colormaps.get_cmap('tab20_r').resampled(len(poligonos))
    # Dibujar cada poligono
    for i, poligono in enumerate(poligonos):
        if len(poligono) < 3:
            continue 
        x, y = zip(*poligono)
        x += (x[0],)
        y += (y[0],)
        plt.plot(x, y, marker='o', color=cmap(i))
        plt.fill(x, y, alpha=0.3, color=cmap(i))
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar los guardias de color negro
    x_guardias, y_guardias = zip(*guardias)
    plt.scatter(x_guardias, y_guardias, color='black', label='Guardias', zorder=5)
    # Dibujar los cortes verticales de color verde
    for corte in cortes_verticales:
        x_corte, y_corte = zip(*corte)
        plt.plot(x_corte, y_corte, color='green', linestyle='--', label='Corte Vertical')
    # Dibujar el poligono original con guardias
    plt.subplot(1, 2, 2)
    x, y = zip(*vertices)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar los guardias de color
    x_guardias, y_guardias = zip(*guardias)
    plt.scatter(x_guardias, y_guardias, color='black', label='Guardias', zorder=5)
    # Mostrar el texto
    plt.suptitle("Paso 6")
    plt.tight_layout()
    plt.show()

# Metodo para dibujar el poligono original y el poligono con guardias, uno al lado del otro
def dibujar_poligonos_final(vertices_original, vertices_final, guardias):
    # Dibujar el poligono original
    plt.subplot(1, 2, 1)
    x, y = zip(*vertices_original)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar el poligono original con guardias
    plt.subplot(1, 2, 2)
    x, y = zip(*vertices_final)
    x += (x[0],)
    y += (y[0],)
    plt.plot(x, y, marker='o')
    plt.fill(x, y, alpha=0.3)
    plt.xlabel("X")
    plt.ylabel("Y")
    plt.grid()
    # Dibujar los guardias de color
    x_guardias, y_guardias = zip(*guardias)
    plt.scatter(x_guardias, y_guardias, color='red', label='Guardias', zorder=5)
    # Mostrar el texto
    plt.suptitle("Fin")
    plt.tight_layout()
    plt.show()

# Metodo para abrir un archivo de texto y obtener su contenido
def abrir_archivo(archivo):
    with open(archivo, 'r') as f:
        contenido = f.read()
    return contenido

# Main para recibir el arxhivo de texto como argumento
if __name__ == "__main__":
    # Recibir el archivo de texto como argumento
    if len(sys.argv) < 2:
        print("Uso: python a.py <archivo>")
        sys.exit(1)
    archivo = sys.argv[1]
    # Abrir el archivo y obtener su contenido
    contenido = abrir_archivo(archivo)
    # Convertir el poligono de texto a una lista de vertices
    vertices = convertir_a_lista(contenido)
    # Ejecutar el algoritmo de guardias
    algoritmo_guardias(vertices)
