import pandas as pd
import mysql.connector
import os

# Mapeo de archivos físicos a sus respectivas tablas en MySQL
CATALOGOS_MAP = {
    "2.-UNIDAD RESPONSABLE.xls": "Cat_UnidadResponsable",
    "3.-UNIDAD RESPONSABLE OPERATIVA.xls": "Cat_UnidadResponsableOperativa",
    "4.-FINALIDAD.xls": "Cat_Finalidad",
    "5.-FUNCION.xls": "Cat_Funcion",
    "6.-SUBFUNCION.xls": "Cat_Subfuncion",
    "7.-SUBSUBFUNCION.xls": "Cat_Subsubfuncion",
    "8.-PROGRAMA SECTORIAL.xls": "Cat_ProgramaSectorial",
    "9.-PROGRAMA PRESUPUESTARIO.xls": "Cat_ProgramaPresupuestario",
    "10.-ORIGEN DE ASIGNACION.xls": "Cat_OrigenAsignacion",
    "11.-ACTIVIDAD INSTITUCIONAL.xls": "Cat_ActividadInstitucional",
    "13.-MUNICIPIOS.xls": "Cat_Municipio",
    "15.-OBJETO DEL GASTO POR CATEGORIA.xls": "Cat_ObjetoGasto",
    "19.-TIPO DE GASTO.xls": "Cat_TipoGasto",
    "20.-TIPO DE RECURSO.xls": "Cat_TipoRecurso",
    "21.-FUENTE DE FINANCIAMIENTO.xls": "Cat_FuenteFinanciamiento",
    "22.-SUBFUENTE DE FINANCIAMIENTO.xls": "Cat_SubfuenteFinanciamiento",
    "23.-RAMO.xls": "Cat_Ramo",
    "24.-PROGRAMA FONDO.xls": "Cat_ProgramaFondo",
    "26.-DIGITOS DE MINISTRACION.xls": "Cat_DigitosMinistracion"
}

def extract_clave_desc(df):
    """
    Algoritmo heurístico para saltarse logos, títulos, y extraer la Clave y Descripción
    independientemente de cuántas columnas basura haya antes.
    """
    results = set() # Usamos un Set para evitar duplicados
    for index, row in df.iterrows():
        # Filtramos celdas vacías en la fila
        vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
        if len(vals) >= 2:
            # Por la estructura de los Excels del SIAP, la Clave y Descripción 
            # suelen ser las últimas dos columnas con datos útiles en cada renglón.
            clave = vals[-2]
            desc = vals[-1]
            
            # Filtros de limpieza para asegurar que es un dato real y no un título
            if len(clave) <= 15 and len(desc) > 3:
                if clave.lower() not in ["clave", "oe", "tr"] and desc.lower() not in ["descripción", "denominación"]:
                    # La clave suele tener al menos un número o ser letras cortas (ej. X0010, 01)
                    if any(c.isdigit() for c in clave) or (clave.isalpha() and len(clave) <= 5):
                        # Evitar páginas o basura
                        if "Página" not in clave and "Página" not in desc:
                            results.add((clave, desc))
    return list(results)

def migrate_catalogos():
    base_dir = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)"
    print("Iniciando Migración Inteligente de Catálogos...")
    
    try:
        connection = mysql.connector.connect(
            host='localhost',
            database='SIAP_DB',
            user='root',
            password='root'
        )
        if not connection.is_connected():
            print("No se pudo conectar a MySQL.")
            return

        cursor = connection.cursor()
        cursor.execute("SET FOREIGN_KEY_CHECKS = 0") # Apagamos chequeos para poder truncar
        
        archivos_exitosos = 0
        registros_totales = 0
        
        for archivo, tabla in CATALOGOS_MAP.items():
            ruta = os.path.join(base_dir, archivo)
            if not os.path.exists(ruta):
                print(f"[OMITIDO] No se encontró el archivo: {archivo}")
                continue
                
            print(f"\nProcesando: {archivo} -> Tabla: {tabla}")
            try:
                # Usamos xlrd para leer .xls antiguos
                df = pd.read_excel(ruta, header=None)
                registros = extract_clave_desc(df)
                
                if registros:
                    # Limpiamos la tabla
                    cursor.execute(f"TRUNCATE TABLE {tabla}")
                    
                    # Insertamos
                    # Manejo especial porque Municipio tiene columna 'Nombre' y el resto 'Descripcion'
                    col_desc = "Nombre" if tabla == "Cat_Municipio" else "Descripcion"
                    insert_query = f"INSERT IGNORE INTO {tabla} (Clave, {col_desc}) VALUES (%s, %s)"
                    
                    cursor.executemany(insert_query, registros)
                    connection.commit()
                    
                    archivos_exitosos += 1
                    registros_totales += len(registros)
                    print(f"  [OK] Se insertaron {len(registros)} registros limpios.")
                else:
                    print(f"  [AVISO] No se encontró información extraíble en el formato esperado.")
                    
            except Exception as e:
                print(f"  [ERROR] Falló al procesar {archivo}: {str(e)}")
                
        cursor.execute("SET FOREIGN_KEY_CHECKS = 1") # Encendemos cheques
        connection.commit()
        
        print("\n=============================================")
        print(f"MIGRACIÓN FINALIZADA.")
        print(f"Tablas pobladas: {archivos_exitosos} de {len(CATALOGOS_MAP)}")
        print(f"Registros totales insertados: {registros_totales}")
        print("=============================================")
        
    except Error as e:
        print(f"Error crítico en MySQL: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    migrate_catalogos()
