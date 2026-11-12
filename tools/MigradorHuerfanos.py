import pandas as pd
import mysql.connector
import os

HUERFANOS_MAP = {
    "1.-CLASIFICACION ADMINISTRATIVA.xls": "Cat_ClasificacionAdministrativa",
    "12..PROYECTOS ESTRATEGICOS.xls": "Cat_ProyectoEstrategico",
    "14.-ASIGNACION DEL GASTO.xls": "Cat_AsignacionGasto",
    "16.-OBJETO DEL GASTO INTEGRAR.xlsx": "Cat_ObjetoGastoIntegrar",
    "17.-ORIENTACION DEL GASTO.xls": "Cat_OrientacionGasto",
    "18.-AÑO DEL GASTO.xls": "Cat_AnioGasto",
    "25.-RUBRO DE INGRESOS.xls": "Cat_RubroIngresos",
    "CAPITULOS DEL GASTO.xls": "Cat_CapituloGasto",
    "REGIONES.xls": "Cat_Region",
    "TIPO DE ADECUACION.xls": "Cat_TipoAdecuacion",
    "TIPO DE MINISTRACION.xls": "Cat_TipoMinistracion",
    "TIPO DE RECURSO DE LA FUENTE DE FINANCIAMIENTO.xls": "Cat_TipoRecursoFuenteFinanciamiento"
}

def extract_clave_desc(df):
    results = set()
    for index, row in df.iterrows():
        vals = [str(x).strip() for x in row.values if not pd.isna(x) and str(x).strip() != '']
        if len(vals) >= 2:
            clave = vals[-2]
            desc = vals[-1]
            if len(clave) <= 20 and len(desc) > 3:
                if clave.lower() not in ["clave", "oe", "tr"] and desc.lower() not in ["descripción", "denominación"]:
                    if any(c.isdigit() for c in clave) or (clave.isalpha() and len(clave) <= 5):
                        if "Página" not in clave and "Página" not in desc:
                            results.add((clave, desc))
    return list(results)

def migrate_huerfanos():
    base_dir = r"SISTEMA EXCEL\CATALOGO SISTEMA INTERNO (SIGHA 2026)"
    print("Iniciando Rescate de Catálogos Huérfanos...")
    
    try:
        connection = mysql.connector.connect(
            host='localhost', database='SIAP_DB', user='root', password='root'
        )
        cursor = connection.cursor()
        
        archivos_exitosos = 0
        registros_totales = 0
        
        for archivo, tabla in HUERFANOS_MAP.items():
            ruta = os.path.join(base_dir, archivo)
            if not os.path.exists(ruta):
                print(f"[OMITIDO] No se encontró el archivo: {archivo}")
                continue
                
            print(f"\nProcesando: {archivo} -> Tabla: {tabla}")
            try:
                # CREAR LA TABLA AL VUELO SI NO EXISTE
                cursor.execute(f"""
                    CREATE TABLE IF NOT EXISTS {tabla} (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Clave VARCHAR(20),
                        Descripcion VARCHAR(255)
                    );
                """)
                
                # Leemos el archivo. Si es xlsx, pandas usa openpyxl, si es xls usa xlrd.
                # Auto-detecta basado en la extensión.
                df = pd.read_excel(ruta, header=None)
                registros = extract_clave_desc(df)
                
                if registros:
                    cursor.execute(f"TRUNCATE TABLE {tabla}")
                    # Insert ignore para no fallar por duplicados
                    insert_query = f"INSERT IGNORE INTO {tabla} (Clave, Descripcion) VALUES (%s, %s)"
                    
                    cursor.executemany(insert_query, registros)
                    connection.commit()
                    
                    archivos_exitosos += 1
                    registros_totales += len(registros)
                    print(f"  [OK] Se insertaron {len(registros)} registros limpios en {tabla}.")
                else:
                    print(f"  [AVISO] No se extrajeron registros.")
                    
            except Exception as e:
                print(f"  [ERROR] Falló al procesar {archivo}: {str(e)}")
                
        print("\n=============================================")
        print(f"MIGRACIÓN DE HUÉRFANOS FINALIZADA.")
        print(f"Tablas nuevas creadas y pobladas: {archivos_exitosos} de {len(HUERFANOS_MAP)}")
        print(f"Registros totales insertados: {registros_totales}")
        print("=============================================")
        
    except Exception as e:
        print(f"Error MySQL: {e}")
    finally:
        if 'connection' in locals() and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    migrate_huerfanos()
